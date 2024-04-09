using Iot.Device.Mcp3428;
using Library;
using Library.APIIntegration;
using Library.DoorControl;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using Microsoft.VisualBasic;
using System.Device.Gpio;
using System.Diagnostics;

namespace FloorIsLava.Services
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
        MCP23Pin MagnetRelay = MasterOutputPin.OUTPUT4;


        bool NextRoomRGBButtonStatus = false;
        private MCP23Pin NextRoomPBLight = MasterOutputPin.OUTPUT6;
        private MCP23Pin NextRoomPB = MasterDI.IN8;

        bool EnterRGBButtonStatus = false;
        private MCP23Pin EnterRGBButton = MasterOutputPin.OUTPUT6;
        private MCP23Pin EnterRoomPB = MasterDI.IN8;


        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.FloorIsLava);
            AudioPlayer.Init(Room.FloorIsLava);
            MCP23Controller.Init(Room.FloorIsLava);


            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            //DoorControl.Status(DoorPin, false);


            MCP23Controller.PinModeSetup(EnterRoomPB, PinMode.Input);
            MCP23Controller.PinModeSetup(NextRoomPB, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);


            _logger.LogInformation("Start Main Service");
            Task.Run(() => CheckIFRoomIsEmpty(_cts2.Token));
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => GameTimingService(_cts3.Token));
            Task.Run(() => DoorLockControl(_cts4.Token));

            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (true)
            {
                RelayController.Status(EnterRGBButton, true);
                Thread.Sleep(1000);
                RelayController.Status(EnterRGBButton, false);
                Thread.Sleep(1000);

            }

            while (!cancellationToken.IsCancellationRequested)
            {
                RoomAudio();
                ControlEnteringRGBButton();
                await CheckNextRoomStatus();
                await ControlExitingRGBButton();
                //if (VariableControlService.GameStatus == GameStatus.Empty)
                //    DoorControl.Control(DoorPin, DoorStatus.Close);

            }
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    PIR1 = _controller.Read(MasterDI.PIRPin1);
            //    PIR2 = _controller.Read(MasterDI.PIRPin2);
            //    PIR3 = _controller.Read(MasterDI.PIRPin3);
            //    PIR4 = _controller.Read(MasterDI.PIRPin4);
            //    //Console.WriteLine($"PIR Status PIR1:{PIR1} PIR2:{PIR2} PIR3:{PIR3} PIR4:{PIR4}");
            //    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
            //    ControlRoomAudio();
            //    if (VariableControlService.EnableGoingToTheNextRoom)
            //    {
            //        _logger.LogDebug("Open The Door");
            //        DoorControl.Status(DoorPin, true);
            //        while (PIR1 || PIR2 || PIR3 || PIR4)
            //        {
            //            PIR1 = _controller.Read(MasterDI.PIRPin1);
            //            PIR2 = _controller.Read(MasterDI.PIRPin2);
            //            PIR3 = _controller.Read(MasterDI.PIRPin3);
            //            PIR4 = _controller.Read(MasterDI.PIRPin4);
            //        }
            //        Thread.Sleep(30000);
            //        DoorControl.Status(DoorPin, false);
            //        MCP23Controller.Write(MagnetRelay, PinState.Low); // Relese Magnet
            //        ResetTheGame();
            //        _logger.LogDebug("No One In The Room , All Gone To The Next Room");

            //    }

            //    Thread.Sleep(10);
            //}
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
        // Control Starting All the Threads
        private async Task GameTimingService(CancellationToken cancellationToken)
        {
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    if (VariableControlService.IsTheGameStarted && !VariableControlService.IsGameTimerStarted)
            //    {
            //        GameTiming.Restart();
            //        VariableControlService.IsGameTimerStarted = true;
            //    }
            //    bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.RoomTiming;
            //    bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.IsGameTimerStarted;

            //    if (GameFinishedByTimer || VariableControlService.IsTheGameFinished)
            //        StopTheGame();
            //}
            while (!cancellationToken.IsCancellationRequested)
            {
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
        private void StopTheGame()
        {
            //VariableControlService.IsTheGameStarted = false;
            //VariableControlService.IsTheGameFinished = true;
            //VariableControlService.EnableGoingToTheNextRoom = true;

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

        private void ControlEnteringRGBButton()
        {
            if (!EnterRGBButtonStatus && VariableControlService.GameStatus == GameStatus.NotStarted)
            {
                _logger.LogTrace("Ready To Start The Game .. Turn RGB Button On");
                EnterRGBButtonStatus = true;
                RelayController.Status(EnterRGBButton, true);
            }
            else if (EnterRGBButtonStatus && VariableControlService.GameStatus != GameStatus.NotStarted)
            {
                EnterRGBButtonStatus = false;
                RelayController.Status(EnterRGBButton, false);
            }
            bool RGBButtonIsOnAndGameNotStarted = EnterRGBButtonStatus && VariableControlService.GameStatus == GameStatus.NotStarted;
            if (RGBButtonIsOnAndGameNotStarted)
            {
                bool PBPressed = !MCP23Controller.Read(EnterRoomPB);
                if (PBPressed)
                {
                    _logger.LogTrace("Start The Game Pressed");
                    Console.WriteLine(PBPressed);
                    EnterRGBButtonStatus = false;
                    RelayController.Status(NextRoomPBLight, false);
                    VariableControlService.GameStatus = GameStatus.Started;
                    VariableControlService.IsGameTimerStarted = false;
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

                bool PBPressed = !MCP23Controller.Read(NextRoomPB);
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

        private void RoomAudio()
        {
            if (VariableControlService.GameStatus == GameStatus.NotStarted && !thereAreInstructionSoundPlays)
            {
                _logger.LogTrace("Start Instruction Audio");
                AudioPlayer.PIBackgroundSound(SoundType.instruction);
                thereAreInstructionSoundPlays = true;
            }
            else if (thereAreInstructionSoundPlays && VariableControlService.GameStatus != GameStatus.NotStarted)
            {
                _logger.LogTrace("Stop Instruction Audio");
                thereAreInstructionSoundPlays = false;
                AudioPlayer.PIStopAudio();
                Thread.Sleep(500);
            }

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
