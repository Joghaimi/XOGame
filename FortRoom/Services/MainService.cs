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
        private MCP23Pin LaserPin = MasterOutputPin.OUTPUT2;

        bool EnterRGBButtonStatus = false;
        private MCP23Pin EnterRGBButton = MasterOutputPin.OUTPUT8;
        private MCP23Pin EnterRoomPB = MasterDI.IN8;

        bool NextRoomRGBButtonStatus = false;
        private MCP23Pin NextRoomPBLight = MasterOutputPin.OUTPUT8;
        private MCP23Pin NextRoomPB = MasterDI.IN8;


        private CancellationTokenSource _cts, _cts2, _cts3, _cts4, _cts5;
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

            MCP23Controller.PinModeSetup(EnterRoomPB, PinMode.InputPullUp);
            MCP23Controller.PinModeSetup(NextRoomPB, PinMode.InputPullUp);



            //_controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            //_controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            //_controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            //_controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            // In Main Service Run All Default and common things 

            RGBLight.SetColor(VariableControlService.DefaultColor);

            //DoorControl.Status(DoorPin, false);


            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Output);


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts5 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => CheckIFRoomIsEmpty(_cts2.Token));
            Task.Run(() => GameTimingService(_cts3.Token));
            Task.Run(() => DoorLockControl(_cts4.Token));
            Task.Run(() => CheckNextRoomStatus(_cts5.Token));


            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {

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

            while (!cancellationToken.IsCancellationRequested)
            {

                RoomAudio();
                ControlEnteringRGBButton();
                ControlExitingRGBButton();
                //CheckNextRoomStatus();
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
                Console.WriteLine(PBPressed);
                Thread.Sleep(1000);
                //if (PBPressed)
                //{
                //    _logger.LogTrace("Start The Game Pressed");
                //    Console.WriteLine(PBPressed);
                //    EnterRGBButtonStatus = false;
                //    RelayController.Status(NextRoomPBLight, false);
                //    VariableControlService.GameStatus = GameStatus.Started;
                //}

            }


            //if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && !RGBButtonStatus)
            //{
            //    Console.WriteLine("Ready To Leave .. Turn RGB Button On");
            //    RGBButtonStatus = true;
            //    RelayController.Status(NextRoomPBLight, true);
            //}
            //else if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && RGBButtonStatus)
            //{
            //    Console.WriteLine(MCP23Controller.Read(NextRoomPB));
            //    Thread.Sleep(5000);
            //    bool PBPressed = !MCP23Controller.Read(NextRoomPB);
            //    if (PBPressed)
            //    {
            //        RGBButtonStatus = false;
            //        VariableControlService.GameStatus = GameStatus.Leaving;
            //        RelayController.Status(NextRoomPBLight, false);
            //    }
            //}
        }

        private async void ControlExitingRGBButton()
        {
            if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && !NextRoomRGBButtonStatus)
            {
                Console.WriteLine("Ready To Leave .. Turn RGB Button On");
                NextRoomRGBButtonStatus = true;
                RelayController.Status(NextRoomPBLight, true);
            }
            else if (VariableControlService.GameStatus == GameStatus.ReadyToLeave && NextRoomRGBButtonStatus)
            {
                //Console.WriteLine(MCP23Controller.Read(NextRoomPB));
                //Thread.Sleep(5000);
                //bool PBPressed = !MCP23Controller.Read(NextRoomPB);
                //if (PBPressed)
                //{
                //    NextRoomRGBButtonStatus = false;
                //    VariableControlService.GameStatus = GameStatus.Leaving;
                //    RelayController.Status(NextRoomPBLight, false);
                //}
                // This Will be Moved to run after PB Is Preessed 
                while (true)
                {

                    var result = await APIIntegration.SendScoreToTheNextRoom(VariableControlService.SendScoreToTheNextRoom, VariableControlService.TeamScore);
                    _logger.LogTrace($"Score Send {result}");
                    if (result)
                    {
                        VariableControlService.GameStatus = GameStatus.Leaving;
                        _logger.LogTrace($"Player Should be out From the room");
                        Thread.Sleep(30000);
                        VariableControlService.GameStatus = GameStatus.Empty;
                        _logger.LogTrace($"Room Should be Empty now");
                        break;
                    }


                }
            }
        }

        private async Task CheckNextRoomStatus(CancellationToken cancellationToken)
        {
            if (VariableControlService.GameStatus == GameStatus.FinishedNotEmpty)
            {
                var status = await APIIntegration.NextRoomStatus(VariableControlService.NextRoomURL);
                if (status == "Empty")
                {
                    VariableControlService.GameStatus = GameStatus.ReadyToLeave;
                    return;
                }
                Console.WriteLine("*");
                Thread.Sleep(30000);
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
        //private async Task CheckNextRoomSatus(CancellationToken cancellationToken)
        //{
        //    while (!cancellationToken.IsCancellationRequested)
        //    {
        //        if (VariableControlService.IsTheGameStarted && !VariableControlService.IsGameTimerStarted)
        //        {
        //            GameTiming.Restart();
        //            VariableControlService.IsGameTimerStarted = true;
        //        }
        //        bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.RoomTiming;
        //        bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.IsGameTimerStarted;

        //        if (GameFinishedByTimer || VariableControlService.IsTheGameFinished)
        //            StopTheGame();
        //    }
        //}

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
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsTheGameFinished = true;
            //VariableControlService.EnableGoingToTheNextRoom = true;

        }
        private void ResetTheGame()
        {
            VariableControlService.EnableGoingToTheNextRoom = false;
            VariableControlService.IsTheirAnyOneInTheRoom = false;
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.IsGameTimerStarted = false;
        }

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
