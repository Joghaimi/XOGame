﻿using Iot.Device.Mcp3428;
using Library;
using Library.DoorControl;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using Microsoft.VisualBasic;
using System.Device.Gpio;
using System.Diagnostics;

namespace DarkRoom.Services
{
    public class MainService : IHostedService, IDisposable
    {

        private GPIOController _controller;
        private CancellationTokenSource _cts, _cts2, _cts3;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private readonly ILogger<MainService> _logger;
        bool thereAreBackgroundSoundPlays = false;
        bool thereAreInstructionSoundPlays = false;
        private MCP23Pin DoorPin = MasterOutputPin.OUTPUT7;
        Stopwatch GameTiming = new Stopwatch();

        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Dark);
            AudioPlayer.Init(Room.Dark);
            MCP23Controller.Init(Room.Dark);
            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            DoorControl.Status(DoorPin, false);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => CheckIFRoomIsEmpty(_cts2.Token));
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => GameTimingService(_cts3.Token));

            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);

            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(MasterDI.PIRPin1);
                PIR2 = _controller.Read(MasterDI.PIRPin2);
                PIR3 = _controller.Read(MasterDI.PIRPin3);
                PIR4 = _controller.Read(MasterDI.PIRPin4);
                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
                ControlRoomAudio();
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    _logger.LogDebug("Open The Door");
                    DoorControl.Status(DoorPin, true);
                    while (PIR1 || PIR2 || PIR3 || PIR4)
                    {
                        PIR1 = _controller.Read(MasterDI.PIRPin1);
                        PIR2 = _controller.Read(MasterDI.PIRPin2);
                        PIR3 = _controller.Read(MasterDI.PIRPin3);
                        PIR4 = _controller.Read(MasterDI.PIRPin4);
                    }
                    Thread.Sleep(30000);
                    DoorControl.Status(DoorPin, false);
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    VariableControlService.IsTheirAnyOneInTheRoom = false;
                    VariableControlService.IsTheGameStarted = false;
                    RGBLight.SetColor(RGBColor.Off);
                    _logger.LogDebug("No One In The Room , All Gone To The Next Room");
                }

                Thread.Sleep(10);
            }
        }

      
        // Control Starting All the Threads
        private async Task GameTimingService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheGameStarted && !VariableControlService.IsGameTimerStarted)
                {
                    GameTiming.Restart();
                    VariableControlService.IsGameTimerStarted = true;
                }
                bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.RoomTiming;
                bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.IsGameTimerStarted;

                if (GameFinishedByTimer || VariableControlService.IsTheGameFinished)
                    StopTheGame();
            }
        }

        private async Task CheckIFRoomIsEmpty(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.IsTheirAnyOneInTheRoom && !VariableControlService.IsTheGameStarted)
                {
                    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4;
                    Thread.Sleep(10000);
                }

            }
        }
        private void ControlRoomAudio()
        {
            if (VariableControlService.IsOccupied && !VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished && !thereAreInstructionSoundPlays)
            {
                _logger.LogTrace("Start Instruction Audio");
                thereAreInstructionSoundPlays = true;
                AudioPlayer.PIBackgroundSound(SoundType.instruction);
            }
            else if (VariableControlService.IsOccupied && VariableControlService.IsTheGameStarted
                && !VariableControlService.IsTheGameFinished
                && thereAreInstructionSoundPlays && !thereAreBackgroundSoundPlays)
            {
                // Stop Background Audio 
                _logger.LogTrace("Stop Instruction Audio");
                thereAreInstructionSoundPlays = false;
                AudioPlayer.PIStopAudio();
                Thread.Sleep(500);
                // Start Background Audio
                _logger.LogTrace("Start Background Audio");
                thereAreBackgroundSoundPlays = true;
                AudioPlayer.PIBackgroundSound(SoundType.Background);
            }
            else if (VariableControlService.IsTheGameFinished && thereAreBackgroundSoundPlays)
            {
                // Game Finished .. 
                _logger.LogTrace("Stop Background Audio");
                thereAreBackgroundSoundPlays = false;
                AudioPlayer.PIStopAudio();
            }
        }
        private void StopTheGame()
        {

            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsTheGameFinished = true;
        }
        private void ResetTheGame()
        {
            VariableControlService.EnableGoingToTheNextRoom = false;
            VariableControlService.IsTheirAnyOneInTheRoom = false;
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsGameTimerStarted = false;
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stop Dark Room  ***");
            DoorControl.Status(DoorPin, false);
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
        }

    }

}
