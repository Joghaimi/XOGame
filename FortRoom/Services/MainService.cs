﻿using Iot.Device.Mcp3428;
using Library;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;

namespace FortRoom.Services
{

    // The Main Part of the game

    public class MainService : IHostedService, IDisposable
    {
        private readonly ILogger<MainService> _logger;

        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }

        private GPIOController _controller;
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts, cts2;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Fort);
            AudioPlayer.Init(Room.Fort);
            MCP23Controller.Init(true);
            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            //AudioPlayer.PIBackgroundSound(SoundType.Background);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6, PinMode.Output);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => CheckIFRoomIsEmpty(cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool thereAreBackgroundSoundPlays = false;
            bool thereAreInstructionSoundPlays = false;
            RGBLight.SetColor(RGBColor.Red);
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(MasterDI.PIRPin1);
                PIR2 = _controller.Read(MasterDI.PIRPin2);
                PIR3 = _controller.Read(MasterDI.PIRPin3);
                PIR4 = _controller.Read(MasterDI.PIRPin4);
                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
                // Control Background Audio
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
        public Task StopAsync(CancellationToken cancellationToken)
        {
            //_cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            //_cts.Dispose();
        }


    }
}
