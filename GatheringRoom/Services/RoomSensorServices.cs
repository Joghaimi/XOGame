using GatheringRoom.Controllers;
using Iot.Device.Mcp3428;
using Library;
using Library.GPIOLib;
using Library.PinMapping;
using Library.RFIDLib;
using Library.RGBLib;
using Microsoft.AspNetCore.Mvc;
using NAudio.SoundFont;
using System.Device.Gpio;

namespace GatheringRoom.Services
{
    public class RoomSensorServices : IHostedService, IDisposable
    {
        private readonly ILogger<RoomSensorServices> _logger;
        public bool isTheirAreSomeOneInTheRoom = false;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private bool isLightOn = false;
        private GPIOController _controller;
        private CancellationTokenSource _cts;
        public RoomSensorServices(ILogger<RoomSensorServices> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            // Init the Pin's
            _controller.Setup(VariableControlService.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin4, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin2, PinMode.InputPullDown);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _logger.LogInformation("RoomSensorServices Started");
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Gathering);
            MCP23Controller.Init(Room.Gathering);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6, PinMode.Output);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(VariableControlService.PIRPin1);
                PIR2 = _controller.Read(VariableControlService.PIRPin2);
                PIR3 = _controller.Read(VariableControlService.PIRPin3);
                PIR4 = _controller.Read(VariableControlService.PIRPin4);
                bool isAnyOfRIPSensorActive = PIR1 || PIR2 || PIR3 || PIR4;// || isTheirAreSomeOneInTheRoom;
                //Console.WriteLine($"PIR1 {PIR1} PIR2 {PIR2} PIR3 {PIR3} PIR4 {PIR4}");
                if (isAnyOfRIPSensorActive && !isLightOn)
                {
                    _logger.LogTrace("Some One In The Room");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
                    RGBLight.SetColor(RGBColor.Blue);
                    Console.WriteLine("Switch Light On"); // To Do
                    isTheirAreSomeOneInTheRoom = true;// rise a flag 
                    isLightOn = true;
                }
                else if (!isAnyOfRIPSensorActive && isLightOn)
                {
                    _logger.LogTrace("No One In The Room");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.High);
                    RGBLight.SetColor(RGBColor.Off);
                    Console.WriteLine("Switch Light Off"); // To Do
                    isLightOn = false;
                }

                // IF Enable Going To The Next room 
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    _logger.LogTrace("Open The Door");
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    isTheirAreSomeOneInTheRoom = false;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.High);
            RGBLight.SetColor(RGBColor.Off);
            _logger.LogInformation("RoomSensorServices Stopped");
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _logger.LogInformation("RoomSensorServices Disposed");
            _cts.Dispose();
        }
    }
}
