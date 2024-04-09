using Iot.Device.Mcp3428;
using Library;
using Library.APIIntegration;
using Library.DoorControl;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;
using System.Diagnostics;

namespace FortRoom.Services
{
    public class MainService : IHostedService, IDisposable
    {
        private readonly ILogger<MainService> _logger;
        private GPIOController _controller;
        private MCP23Pin DoorPin = MasterOutputPin.OUTPUT7;
        //private MCP23Pin LaserPin = MasterOutputPin.OUTPUT2;

        bool EnterRGBButtonStatus = false;
        private MCP23Pin EnterRGBButton = MasterOutputPin.OUTPUT8;
        private MCP23Pin EnterRoomPB = MasterDI.IN3;

        bool NextRoomRGBButtonStatus = false;
        private MCP23Pin NextRoomPBLight = MasterOutputPin.OUTPUT8;
        private MCP23Pin NextRoomPB = MasterDI.IN3;


        private CancellationTokenSource _cts, _cts2, _cts3, _cts4;
        public bool isTheirAreSomeOneInTheRoom = false;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        bool thereAreBackgroundSoundPlays = false;
        bool thereAreInstructionSoundPlays = false;
        bool FinishAudioNotStarted = false;
        Stopwatch GameTiming = new Stopwatch();





        public MainService(ILogger<MainService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Fort);
            AudioPlayer.Init(Room.Fort);
            MCP23Controller.Init(Room.Fort);

            MCP23Controller.PinModeSetup(EnterRoomPB, PinMode.Input);
            MCP23Controller.PinModeSetup(NextRoomPB, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);
           

            RGBLight.SetColor(VariableControlService.DefaultColor);

            //DoorControl.Status(DoorPin, false);
            //MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Output);


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => CheckIFRoomIsEmpty(_cts2.Token));
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
                if (VariableControlService.GameStatus == GameStatus.Empty)
                    DoorControl.Control(DoorPin, DoorStatus.Close);

            }
        }
        private async Task CheckIFRoomIsEmpty(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheirAnyOneInTheRoom && !VariableControlService.IsTheGameStarted)
                {
                    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4;
                    Thread.Sleep(10000);
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
                // This Will be Moved to run after PB Is Preessed 

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






        private async Task GameTimingService(CancellationToken cancellationToken)
        {
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

        private async Task DoorLockControl(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
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


        private void StopTheGame()
        {
            Console.WriteLine("Stoped By Time For Test");
            VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsTheGameFinished = true;
            VariableControlService.IsGameTimerStarted = false;
            //VariableControlService.EnableGoingToTheNextRoom = true;
        }
        //private void ResetTheGame()
        //{
        //    VariableControlService.EnableGoingToTheNextRoom = false;
        //    VariableControlService.IsTheirAnyOneInTheRoom = false;
        //    VariableControlService.IsTheGameStarted = false;
        //    VariableControlService.IsGameTimerStarted = false;
        //}

        // Stop The Service
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            _cts2.Cancel();
            _cts3.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
            _cts2.Dispose();
            _cts3.Dispose();
        }


    }
}



// To Do Next Time
//[Obsolete("Old")]
//private void ControlRoomAudio()
//{
//    // Control Background Audio
//    if (VariableControlService.IsOccupied && !VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished && !thereAreInstructionSoundPlays)
//    {
//        _logger.LogTrace("Start Instruction Audio");
//        thereAreInstructionSoundPlays = true;
//        AudioPlayer.PIBackgroundSound(SoundType.instruction); // TO DO
//        RelayController.Status(LaserPin, true);
//        Thread.Sleep(60000);
//        VariableControlService.IsTheGameStarted = true;
//        VariableControlService.IsTheGameFinished = false;
//    }
//    else if (VariableControlService.IsOccupied && VariableControlService.IsTheGameStarted
//        && !VariableControlService.IsTheGameFinished
//        && thereAreInstructionSoundPlays && !thereAreBackgroundSoundPlays)
//    {
//        // Stop Background Audio 
//        _logger.LogTrace("Stop Instruction Audio");
//        thereAreInstructionSoundPlays = false;
//        AudioPlayer.PIStopAudio();
//        Thread.Sleep(500);
//        // Start Background Audio
//        _logger.LogTrace("Start Background Audio");
//        thereAreBackgroundSoundPlays = true;
//        AudioPlayer.PIBackgroundSound(SoundType.Background);
//    }
//    else if (VariableControlService.IsTheGameFinished && thereAreBackgroundSoundPlays)
//    {
//        // Game Finished .. 
//        _logger.LogTrace("Stop Background Audio");
//        thereAreBackgroundSoundPlays = false;
//        VariableControlService.EnableGoingToTheNextRoom = true;
//        AudioPlayer.PIStopAudio();
//    }
//}




//while (!cancellationToken.IsCancellationRequested)
//{
//    PIR1 = _controller.Read(MasterDI.PIRPin1);
//    PIR2 = _controller.Read(MasterDI.PIRPin2);
//    PIR3 = _controller.Read(MasterDI.PIRPin3);
//    PIR4 = _controller.Read(MasterDI.PIRPin4);
//    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
//    ControlRoomAudio();// Control Background Audio
//    // IF Enable Going To The Next room 
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
//        ResetTheGame();
//        _logger.LogDebug("No One In The Room , All Gone To The Next Room");
//    }
//    Thread.Sleep(10);


//}