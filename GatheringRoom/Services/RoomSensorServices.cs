using GatheringRoom.Controllers;
using Iot.Device.Board;
using Iot.Device.Mcp3428;
using Library;
using Library.DoorControl;
using Library.GPIOLib;
using Library.Media;
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
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private bool isLightOn = false;
        private GPIOController _controller;
        private CancellationTokenSource _cts, cts2,_cts3;
        private MCP23Pin DoorPin = MasterOutputPin.OUTPUT7;



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
            cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            _logger.LogInformation("RoomSensorServices Started");
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Gathering);
            MCP23Controller.Init(Room.Gathering);
            AudioPlayer.Init(Room.Gathering);

            DoorControl.Control(DoorPin, DoorStatus.Close);
            
            Task.Run(() => CheckIFRoomIsEmpty(cts2.Token));
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => DoorLockControl(_cts3.Token));

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

                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;


                if (VariableControlService.IsTheirAnyOneInTheRoom && !isLightOn)
                {
                    _logger.LogDebug("Some One In The Room");
                    RGBLight.SetColor(RGBColor.Blue);
                    _logger.LogDebug("Switch Light On"); // To Do
                    VariableControlService.IsTheirAnyOneInTheRoom = true;// rise a flag 
                    isLightOn = true;
                }
                else if (!VariableControlService.IsTheirAnyOneInTheRoom && isLightOn)
                {
                    _logger.LogInformation("No One In The Room");
                    Console.WriteLine("Switch Light Off"); // To Do
                    RGBLight.SetColor(RGBColor.Off);
                    isLightOn = false;
                }

                // IF Enable Going To The Next room 
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    _logger.LogDebug("Open The Door");
                    //DoorControl.Status(DoorPin, true);
                    DoorControl.Control(DoorPin, DoorStatus.Open);

                    //while (PIR1 || PIR2 || PIR3 || PIR4)
                    //{
                    //    PIR1 = _controller.Read(VariableControlService.PIRPin1);
                    //    PIR2 = _controller.Read(VariableControlService.PIRPin2);
                    //    PIR3 = _controller.Read(VariableControlService.PIRPin3);
                    //    PIR4 = _controller.Read(VariableControlService.PIRPin4);
                    //}
                    Thread.Sleep(30000);
                    DoorControl.Control(DoorPin, DoorStatus.Close);

                    //DoorControl.Status(DoorPin, false);
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    VariableControlService.IsTheirAnyOneInTheRoom = false;
                    VariableControlService.IsTheGameStarted = false;
                    RGBLight.SetColor(RGBColor.Off);
                    _logger.LogDebug("No One In The Room , All Gone To The Next Room");

                }
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogDebug("Turn Off RGB Light");
            RGBLight.SetColor(RGBColor.Off);
            _logger.LogDebug("Open The Door");
            DoorControl.Control(DoorPin, DoorStatus.Open);
            _logger.LogDebug("RoomSensorServices Stopped");
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _logger.LogDebug("RoomSensorServices Disposed");
            _cts.Dispose();
        }


        //public void DoorStatus(MCP23Pin doorPin, bool status)
        //{
        //    if (!status)
        //    {
        //        MCP23Controller.PinModeSetup(doorPin, PinMode.Output);
        //        MCP23Controller.Write(doorPin, PinState.High);
        //    }
        //    else
        //    {
        //        MCP23Controller.PinModeSetup(doorPin, PinMode.Input);
        //        MCP23Controller.Write(doorPin, PinState.Low);
        //    }
        //}
        private async Task CheckIFRoomIsEmpty(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.IsTheirAnyOneInTheRoom && !VariableControlService.IsTheGameStarted)
                {

                    Thread.Sleep(30000);
                    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4;
                    _logger.LogDebug($"Check if Their any in the Room {VariableControlService.IsTheirAnyOneInTheRoom}");
                }

            }
        }
        private async Task DoorLockControl(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.CurrentDoorStatus != VariableControlService.NewDoorStatus)
                {
                    _logger.LogTrace($"Door Status Changes :{VariableControlService.NewDoorStatus.ToString()}");
                    DoorControl.Control(DoorPin, VariableControlService.NewDoorStatus);
                    VariableControlService.CurrentDoorStatus = VariableControlService.NewDoorStatus;
                }
                Thread.Sleep(500);
            }
        }
    }
}
