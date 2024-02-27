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
        private GPIOController _controller;
        private CancellationTokenSource _cts;
        public RoomSensorServices(ILogger<RoomSensorServices> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);

            MCP23Controller.Init(true);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinMode.Output);
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
            _controller = new GPIOController();
            // Init the Pin's
            _controller.Setup(VariableControlService.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin4, PinMode.InputPullDown);
            _controller.Setup(VariableControlService.PIRPin2, PinMode.InputPullDown);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _logger.LogInformation("RoomSensorServices Started");
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
                bool isAnyOfRIPSensorActive = PIR1 || PIR2 || PIR3 || PIR4 || isTheirAreSomeOneInTheRoom;
                if (isAnyOfRIPSensorActive && !isTheirAreSomeOneInTheRoom)
                {
                    _logger.LogTrace("Some One In The Room");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.Low);
                    RGBLight.SetColor(RGBColor.Blue);
                    Console.WriteLine("Switch Light On"); // To Do
                    isTheirAreSomeOneInTheRoom = true;// rise a flag 
                }


                // IF Enable Going To The Next room 
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    _logger.LogTrace("Open The Door");
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    isTheirAreSomeOneInTheRoom = false;
                }
                await Task.Delay(TimeSpan.FromMilliseconds(100), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
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
