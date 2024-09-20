using Iot.Device.Mcp3428;
using Library;
using Library.APIIntegration;
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
        private CancellationTokenSource _cts, _cts2, _cts3, _cts4;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private readonly ILogger<MainService> _logger;
        bool thereAreBackgroundSoundPlays = false;
        bool thereAreInstructionSoundPlays = false;
        private MCP23Pin DoorPin = MasterOutputPin.OUTPUT7;
        Stopwatch GameTiming = new Stopwatch();

        bool NextRoomRGBButtonStatus = false;
        private MCP23Pin NextRoomPBLight = MasterOutputPin.OUTPUT8;
        private MCP23Pin NextRoomPB = MasterDI.IN3;

        bool EnterRGBButtonStatus = false;
        private MCP23Pin EnterRGBButton = MasterOutputPin.OUTPUT8;
        private MCP23Pin EnterRoomPB = MasterDI.IN3;


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

            RGBLight.SetColor(VariableControlService.DefaultColor);

            MCP23Controller.PinModeSetup(EnterRoomPB, PinMode.Input);
            MCP23Controller.PinModeSetup(NextRoomPB, PinMode.Input);

            //MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => CheckIFRoomIsEmpty(_cts2.Token));
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => GameTimingService(_cts3.Token));
            Task.Run(() => DoorLockControl(_cts4.Token));


            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {


            while (!cancellationToken.IsCancellationRequested)
            {
                RoomAudio();
                ControlEnteringRGBButton();
                await CheckNextRoomStatus();
                await ControlExitingRGBButton();
            }
            Thread.Sleep(10);
        }







        private void RoomAudio()
        {
            //if (VariableControlService.GameStatus == GameStatus.NotStarted && !thereAreInstructionSoundPlays)
            //{
            //    Thread.Sleep(VariableControlService.DelayTimeBeforeInstructionInMs);
            //    _logger.LogTrace("Start Instruction Audio");
            //    //AudioPlayer.PIBackgroundSound(SoundType.instruction);
            //    thereAreInstructionSoundPlays = true;
            //}
            //else if (thereAreInstructionSoundPlays && VariableControlService.GameStatus != GameStatus.NotStarted)
            //{
            //    _logger.LogTrace("Stop Instruction Audio");
            //    thereAreInstructionSoundPlays = false;
            //    //AudioPlayer.PIStopAudio();
            //    Thread.Sleep(500);
            //}

            if (VariableControlService.GameStatus == GameStatus.Started && !thereAreBackgroundSoundPlays)
            {
                _logger.LogTrace("Start Background Audio");
                thereAreBackgroundSoundPlays = true;
                AudioPlayer.PIBackgroundSound(SoundType.Background);
            }
            else if (VariableControlService.GameStatus != GameStatus.Started && thereAreBackgroundSoundPlays)
            {
                _logger.LogTrace("Stop Background Audio");
                thereAreBackgroundSoundPlays = false;
                AudioPlayer.PIStopAudio();
            }
        }

        private void ControlEnteringRGBButton()
        {
            //if (!EnterRGBButtonStatus && VariableControlService.GameStatus == GameStatus.NotStarted)
            //{
            //    Thread.Sleep(VariableControlService.DelayTimeBeforeTurnPBOnInMs);
            //    _logger.LogTrace("Ready To Start The Game .. Turn RGB Button On");
            //    EnterRGBButtonStatus = true;
            //    RelayController.Status(EnterRGBButton, true);
            //}
            //else if (EnterRGBButtonStatus && VariableControlService.GameStatus != GameStatus.NotStarted)
            //{
            //    EnterRGBButtonStatus = false;
            //    RelayController.Status(EnterRGBButton, false);
            //}
            //bool RGBButtonIsOnAndGameNotStarted = EnterRGBButtonStatus && VariableControlService.GameStatus == GameStatus.NotStarted;
            //if (RGBButtonIsOnAndGameNotStarted)
            //{
            //    bool PBPressed = !MCP23Controller.Read(EnterRoomPB,true);
            //    if (PBPressed)
            //    {
            //        _logger.LogTrace("Start The Game Pressed");
            //        Console.WriteLine(PBPressed);
            //        EnterRGBButtonStatus = false;
            //        RelayController.Status(NextRoomPBLight, false);
            //        VariableControlService.GameStatus = GameStatus.Started;
            //        VariableControlService.IsGameTimerStarted = false;
            //    }
            //}
            if (VariableControlService.GameStatus == GameStatus.InstructionAudioEnded)
            {
                _logger.LogTrace("Ready To Start The Game .. Turn RGB Button On");
                EnterRGBButtonStatus = true;
                RelayController.Status(EnterRGBButton, true);
            }
            bool RGBButtonIsOnAndGameNotStarted = EnterRGBButtonStatus && VariableControlService.GameStatus == GameStatus.InstructionAudioEnded;
            if (RGBButtonIsOnAndGameNotStarted)
            {
                bool PBPressed = !MCP23Controller.Read(EnterRoomPB);
                if (PBPressed)
                {
                    EnterRGBButtonStatus = false;
                    RelayController.Status(NextRoomPBLight, false);
                    VariableControlService.GameStatus = GameStatus.Started;
                    VariableControlService.IsGameTimerStarted = false;
                    _logger.LogTrace("Start The Game Btn Pressed, Game Status {0}", VariableControlService.GameStatus);

                }
            }
        }
        private async Task CheckNextRoomStatus()
        {
            if (VariableControlService.GameStatus == GameStatus.FinishedNotEmpty)
            {
                var status = await APIIntegration.NextRoomStatus(VariableControlService.NextRoomURL);
                if (status == "Empty")
                {
                    VariableControlService.GameStatus = GameStatus.ReadyToLeave;
                    return;
                }
                Thread.Sleep(5000);
            }
        }
        private async Task ControlExitingRGBButton()
        {
            if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && !NextRoomRGBButtonStatus)
            {
                Console.WriteLine("Ready To Leave .. Turn RGB Button On");
                NextRoomRGBButtonStatus = true;
                RelayController.Status(NextRoomPBLight, true);
            }
            else if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && NextRoomRGBButtonStatus)
            {

                bool PBPressed = !MCP23Controller.Read(NextRoomPB,true);
                if (PBPressed)
                {
                    NextRoomRGBButtonStatus = false;
                    while (true)
                    {
                        var result = await APIIntegration.SendScoreToTheNextRoom(VariableControlService.SendScoreToTheNextRoom, VariableControlService.TeamScore);
                        _logger.LogTrace($"Score Send {result}");
                        if (result)
                        {
                            VariableControlService.GameStatus = GameStatus.Leaving;
                            RelayController.Status(NextRoomPBLight, false);
                            DoorControl.Control(DoorPin, DoorStatus.Open);
                            _logger.LogTrace($"Player Should be out From the room");
                            Thread.Sleep(30000);
                            DoorControl.Control(DoorPin, DoorStatus.Close);
                            VariableControlService.GameStatus = GameStatus.Empty;
                            _logger.LogTrace($"Room Should be Empty now");
                            break;
                        }
                        Thread.Sleep(3000);
                    }


                }

            }
        }






        // Control Starting All the Threads
        private async Task GameTimingService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                VariableControlService.CurrentTime = (int)GameTiming.ElapsedMilliseconds;

                if ((VariableControlService.GameStatus == GameStatus.Started && !VariableControlService.IsGameTimerStarted))
                {
                    Console.WriteLine("Restart The Timer");
                    GameTiming.Restart();
                    Thread.Sleep(1000);
                    VariableControlService.IsGameTimerStarted = true;
                }
                bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.RoomTiming;
                bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.GameStatus == GameStatus.Started && VariableControlService.IsGameTimerStarted;
                if (GameFinishedByTimer)// || VariableControlService.GameStatus == GameStatus.FinishedNotEmpty)
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
            Console.WriteLine("Stoped By Time For Test");
            VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsTheGameFinished = true;
            VariableControlService.IsGameTimerStarted = false;

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
            DoorControl.Status(DoorPin, true);
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
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
