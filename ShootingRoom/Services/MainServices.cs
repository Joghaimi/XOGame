using Iot.Device.Mcp3428;
using Library;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;

namespace ShootingRoom.Services
{
    public class MainServices : IHostedService, IDisposable
    {
        private GPIOController _controller;
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts, cts2;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private readonly ILogger<MainServices> _logger;
        bool thereAreBackgroundSoundPlays = false;
        bool thereAreInstructionSoundPlays = false;
        private MCP23Pin DoorPin = MasterOutputPin.OUTPUT7;

        public MainServices(ILogger<MainServices> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Shooting);
            MCP23Controller.Init(Room.Shooting);
            AudioPlayer.Init(Room.Shooting);
            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            DoorStatus(DoorPin, false);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => CheckIFRoomIsEmpty(cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {

            RGBLight.SetColor(RGBColor.Red);
            while (!cancellationToken.IsCancellationRequested)
            {

                PIR1 = _controller.Read(MasterDI.PIRPin1);
                PIR2 = _controller.Read(MasterDI.PIRPin2);
                PIR3 = _controller.Read(MasterDI.PIRPin3);
                PIR4 = _controller.Read(MasterDI.PIRPin4);
                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
                ControlRoomAudio();
                if (VariableControlService.IsTheGameStarted)
                {

                }
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    _logger.LogDebug("Open The Door");
                    DoorStatus(DoorPin, true);
                    while (PIR1 || PIR2 || PIR3 || PIR4)
                    {
                        PIR1 = _controller.Read(MasterDI.PIRPin1);
                        PIR2 = _controller.Read(MasterDI.PIRPin2);
                        PIR3 = _controller.Read(MasterDI.PIRPin3);
                        PIR4 = _controller.Read(MasterDI.PIRPin4);
                    }
                    Thread.Sleep(30000);
                    DoorStatus(DoorPin, false);
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    VariableControlService.IsTheirAnyOneInTheRoom = false;
                    VariableControlService.IsTheGameStarted = false;
                    //RGBLight.SetColor(RGBColor.Off);
                    _logger.LogDebug("No One In The Room , All Gone To The Next Room");
                }



                Thread.Sleep(10);
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
        public void DoorStatus(MCP23Pin doorPin, bool status)
        {
            if (!status)
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Output);
                MCP23Controller.Write(doorPin, PinState.High);
            }
            else
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Input);
                MCP23Controller.Write(doorPin, PinState.Low);
            }
        }

    }
}
