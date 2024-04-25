using Iot.Device.Mcp3428;
using Library;
using Library.AirTarget;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShootingRoom.Services
{
    public class AirTargetService : IHostedService, IDisposable
    {

        List<AirTargetController> AirTargetList = new List<AirTargetController>();
        // Shelf One
        AirTargetModel ShelfLight = new AirTargetModel(MasterOutputPin.OUTPUT1, 0);
        AirTargetModel Target1 = new AirTargetModel(HatInputPin.IR1, 15);
        AirTargetModel Target2 = new AirTargetModel(HatInputPin.IR2, 5);
        AirTargetModel Target3 = new AirTargetModel(HatInputPin.IR3, 10);
        AirTargetModel Target4 = new AirTargetModel(HatInputPin.IR4, 5);
        AirTargetModel Target5 = new AirTargetModel(HatInputPin.IR5, 15);
        // Shelf Two
        AirTargetModel ShelfLight2 = new AirTargetModel(MasterOutputPin.OUTPUT2, 0);
        AirTargetModel Target6 = new AirTargetModel(HatInputPin.IR6, 5);
        AirTargetModel Target7 = new AirTargetModel(HatInputPin.IR7, 10);
        AirTargetModel Target8 = new AirTargetModel(HatInputPin.IR8, 5);
        AirTargetModel Target9 = new AirTargetModel(HatInputPin.IR9, 5);
        AirTargetModel Target10 = new AirTargetModel(HatInputPin.IR10, 15);
        // Shelf Three
        AirTargetModel ShelfLight3 = new AirTargetModel(MasterOutputPin.OUTPUT3, 0);
        AirTargetModel Target11 = new AirTargetModel(HatInputPin.IR11, 15);
        AirTargetModel Target12 = new AirTargetModel(HatInputPin.IR12, 5);
        AirTargetModel Target13 = new AirTargetModel(HatInputPin.IR13, 5);
        AirTargetModel Target14 = new AirTargetModel(HatInputPin.IR14, 10);
        AirTargetModel Target15 = new AirTargetModel(HatInputPin.IR15, 5);
        // Shelf Four
        AirTargetModel ShelfLight4 = new AirTargetModel(MasterOutputPin.OUTPUT4, 0);
        AirTargetModel Target16 = new AirTargetModel(HatInputPin.IR16, 5);
        AirTargetModel Target17 = new AirTargetModel(HatInputPin.IR17, 10);
        AirTargetModel Target18 = new AirTargetModel(HatInputPin.IR18, 5);
        AirTargetModel Target19 = new AirTargetModel(HatInputPin.IR19, 10);
        AirTargetModel Target20 = new AirTargetModel(HatInputPin.IR20, 15);
        MCP23Pin TargetMotorControl = MasterOutputPin.OUTPUT5;
        MCP23Pin BigTargetIRSensor = MasterDI.IN1;

        int BigTargetRelay = MasterOutputPin.GPIO23;
        int GunShootRelay = MasterOutputPin.GPIO24;
        int UVLight = MasterOutputPin.GPIO17;

        bool IsItUV = false;
        bool IsItDoubleScore = false;
        private CancellationTokenSource _cts;
        Stopwatch GameStopWatch = new Stopwatch();
        private GPIOController _controller;
        int numberOfAchivedInRow = 0;


        List<int> LevelsRequiredScoreList = new List<int>() { 100, 80, 60, 40, 150 };
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            GameStopWatch.Start();
            AirTargetList.Add(new AirTargetController(ShelfLight, Target1, Target2, Target3, Target4, Target5));
            AirTargetList.Add(new AirTargetController(ShelfLight2, Target6, Target7, Target8, Target9, Target10));
            AirTargetList.Add(new AirTargetController(ShelfLight3, Target11, Target12, Target13, Target14, Target15));
            AirTargetList.Add(new AirTargetController(ShelfLight4, Target16, Target17, Target18, Target19, Target20));
            MCP23Controller.PinModeSetup(TargetMotorControl, PinMode.Output);
            Thread.Sleep(1);
            MCP23Controller.PinModeSetup(BigTargetIRSensor, PinMode.Input);
            Thread.Sleep(1);
            ControlPin(BigTargetRelay, false);
            Thread.Sleep(1);
            ControlPin(BigTargetRelay, false);
            Thread.Sleep(1);
            ControlPin(UVLight, false);
            Thread.Sleep(1);

            RGBLight.SetColor(VariableControlService.DefaultColor);


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            Stopwatch ShelfTimer = new Stopwatch();
            Stopwatch LevelTimer = new Stopwatch();
            while (!cancellationToken.IsCancellationRequested)
            {
                while (true)
                {
                    try
                    {
                        if (IsGameStartedOrInGoing())
                        {

                            if (!VariableControlService.IsAirTargetServiceStarted)
                            {
                                VariableControlService.IsAirTargetServiceStarted = true;
                                VariableControlService.GameRound = Round.Round0;
                            }
                            ControlPin(GunShootRelay, true);
                            Thread.Sleep(1);

                            BigTargetTask();
                            ReturnAllTargets();
                            if (!IsGameStartedOrInGoing())
                                break;
                            int level = 0;
                            foreach (var LevelScore in LevelsRequiredScoreList)
                            {
                                ControlRoundSound(VariableControlService.GameRound);
                                Console.WriteLine(VariableControlService.GameRound.ToString());
                                if (IsGameStartedOrInGoing())
                                {
                                    level++;
                                    VariableControlService.LevelScore = 0;
                                    int numberOfRightHits = 0;
                                    int numberOfWrongHits = 0;
                                    LevelTimer.Restart();
                                    Console.WriteLine("Start The Level");
                                    while ((LevelScore > VariableControlService.LevelScore || VariableControlService.GameRound == Round.Round5) && LevelTimer.ElapsedMilliseconds < 60000)
                                    {
                                        if (!IsGameStartedOrInGoing())
                                            break;
                                        int numberOfHit = 0;
                                        foreach (var item in AirTargetList)
                                        {
                                            if (item.isAllDown())
                                                continue;
                                            ShelfTimer.Restart();
                                            item.Select();
                                            Console.WriteLine($"Start Shelf Loop number OF Hits{numberOfHit}");
                                            while (ShelfTimer.ElapsedMilliseconds <= (10000 - 2000 * (level - 1)))
                                            {
                                                if (!IsGameStartedOrInGoing())
                                                    break;
                                                if (VariableControlService.LevelScore >= LevelScore && VariableControlService.GameRound != Round.Round5)
                                                    break;
                                                if (LevelTimer.ElapsedMilliseconds > 60000)
                                                    break;
                                                foreach (var element in AirTargetList)
                                                {
                                                    if (!IsGameStartedOrInGoing())
                                                        break;
                                                    (bool state, int itemScore, numberOfHit, int targetNumber) = element.TargetStatus();
                                                    Thread.Sleep(1);
                                                    VariableControlService.LevelScore += itemScore;

                                                    if (itemScore > 0 && state)
                                                    {
                                                        Scored(true, IsItUV, 0);
                                                        numberOfRightHits++;
                                                        Console.WriteLine($"+ Score {VariableControlService.LevelScore} , #{numberOfRightHits}");
                                                    }
                                                    else if (itemScore < 0 && state)
                                                    {
                                                        Scored(false, IsItUV, numberOfWrongHits);
                                                        numberOfWrongHits++;
                                                        Console.WriteLine($"- Score {VariableControlService.LevelScore} , #{numberOfWrongHits}");
                                                    }
                                                    if (numberOfHit == 20)
                                                        break;
                                                    Thread.Sleep(10);
                                                }

                                            }
                                            item.UnSelectTarget(false);
                                            if (numberOfHit == 20 || VariableControlService.LevelScore >= LevelScore && VariableControlService.GameRound != Round.Round5)
                                                break;
                                        }
                                        if (numberOfHit == 20 || (VariableControlService.LevelScore >= LevelScore && VariableControlService.GameRound != Round.Round5))
                                            break;

                                    }
                                    Console.WriteLine("End The Level");

                                    bool roundAchieved =
                                        CalculateTheScore(IsItDoubleScore, VariableControlService.LevelScore >= LevelScore, LevelScore,
                                        numberOfRightHits, numberOfWrongHits, VariableControlService.GameRound);
                                    if (roundAchieved)
                                        numberOfAchivedInRow++;
                                    else
                                        numberOfAchivedInRow = 0;
                                    if (VariableControlService.GameRound == Round.Round4 || numberOfAchivedInRow == 2)
                                    {
                                        IsItDoubleScore = true;
                                        IsItUV = ControlUVLight(true);
                                        AudioPlayer.PIStartAudio(SoundType.DoubleScore);
                                        RGBLight.SetColor(RGBColor.Off);
                                    }
                                    else
                                    {
                                        RGBLight.SetColor(VariableControlService.DefaultColor);
                                        IsItUV = ControlUVLight(false);
                                    }


                                    ReturnAllTargets();
                                    ResetAllTarget();
                                }
                                VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                            }
                            ControlPin(GunShootRelay, false);
                            StopAirTargetService();
                        }
                        else if (!IsGameStartedOrInGoing() && VariableControlService.IsAirTargetServiceStarted)
                        {
                            StopAirTargetService();
                        }
                        Thread.Sleep(1);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
                Thread.Sleep(10);
            }
        }

        private void BigTargetTask()
        {
            Stopwatch BigTargetTimer = new Stopwatch();
            ControlPin(BigTargetRelay, true);
            Thread.Sleep(1);
            BigTargetTimer.Start();
            BigTargetTimer.Restart();
            while (true && BigTargetTimer.ElapsedMilliseconds < 60000)
            {
                if (!IsGameStartedOrInGoing())
                    break;
                if (MCP23Controller.Read(MasterDI.IN1))
                {

                    VariableControlService.LevelScore++;
                    //bigTargetHitScore++;
                    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    Console.WriteLine($"Target Hit # {VariableControlService.LevelScore}");
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
                    Thread.Sleep(500);
                }
                if (VariableControlService.LevelScore == 5)
                {
                    VariableControlService.TeamScore.ShootingRoomScore += 100;
                    Console.WriteLine($"Remove Big Target and start the game");
                    break;
                }
                Thread.Sleep(10);
            }
            VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
            VariableControlService.LevelScore = 0;
            ControlPin(BigTargetRelay, false);
            Thread.Sleep(1);
            Console.WriteLine($"Big Target Finished");
        }


        private bool CalculateTheScore(
            bool isItDoubleScore, bool achieveTargetScore, int levelScore,
            int numberOfRightHit, int numberOfWrongHit)
        {
            if (isItDoubleScore && achieveTargetScore)
            {
                VariableControlService.TeamScore.ShootingRoomScore += (levelScore * 2);
                return false;
            }
            else if (achieveTargetScore)
            {
                VariableControlService.TeamScore.ShootingRoomScore += (levelScore);
                return true;
            }
            else
            {
                Console.WriteLine($"Not All The Target shooted right{numberOfRightHit} , wrong {numberOfWrongHit}");
                VariableControlService.TeamScore.ShootingRoomScore += (numberOfRightHit * 5 - numberOfWrongHit * 5);
                return false;
            }
        }



        private bool CalculateTheScore(
            bool isItDoubleScore, bool achieveTargetScore, int levelScore,
            int numberOfRightHit, int numberOfWrongHit, Round round)
        {
            if ((isItDoubleScore && achieveTargetScore || round == Round.Round5))
            {
                VariableControlService.TeamScore.ShootingRoomScore += (levelScore * 2);
                return false;
            }
            else if (achieveTargetScore)
            {
                VariableControlService.TeamScore.ShootingRoomScore += (levelScore);
                return true;
            }
            else
            {
                Console.WriteLine($"Not All The Target shooted right{numberOfRightHit} , wrong {numberOfWrongHit}");
                VariableControlService.TeamScore.ShootingRoomScore += (numberOfRightHit * 5 - numberOfWrongHit * 5);
                return false;
            }
        }


        private void StopAirTargetService()
        {
            ControlPin(GunShootRelay, false);
            VariableControlService.IsAirTargetServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            VariableControlService.LevelScore = 0;
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            Thread.Sleep(1000);
            AudioPlayer.PIStopAudio();
            AudioPlayer.PIForceStopAudio();
        }

        private void ControlPin(int pinNumber, bool state)
        {

            if (state)
            {
                _controller.Setup(pinNumber, PinMode.Output);
                _controller.Write(pinNumber, true);
            }
            else
            {
                _controller.Setup(pinNumber, PinMode.Input);
                _controller.Write(pinNumber, true);
            }
            Thread.Sleep(1);
        }

        private bool ControlUVLight(bool state)
        {
            ControlPin(UVLight, state);
            return state;
        }

        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.GameStatus == GameStatus.Started;
            //return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
        private void ReturnAllTargets()
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
            Thread.Sleep(3000);
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
        }
        private void ResetAllTarget()
        {
            foreach (var item in AirTargetList)
                item.UnSelectTarget(true);
        }
        private int Scored(bool inTheRightTarget, bool inUVMode, int score)
        {
            if (inTheRightTarget)
            {
                AudioPlayer.PIStartAudio(SoundType.Bonus);
                RGBLight.SetColor(RGBColor.Blue);
                if (!inUVMode)
                    RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
                else
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Off);
                return score++;
            }
            else
            {
                AudioPlayer.PIStartAudio(SoundType.Descend);
                RGBLight.SetColor(RGBColor.Red);
                if (!inUVMode)
                    RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
                else
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Off);
                return score--;
            }
        }





        private Round NextRound(Round round)
        {
            return (Round)((int)round + 1);
        }

        private void ControlRoundSound(Round round)
        {
            switch (round)
            {
                case Round.Round1:
                    AudioPlayer.PIStartAudio(SoundType.RoundOne);
                    break;
                case Round.Round2:
                    AudioPlayer.PIStartAudio(SoundType.RoundTwo);
                    break;
                case Round.Round3:
                    AudioPlayer.PIStartAudio(SoundType.RoundThree);
                    break;
                case Round.Round4:
                    AudioPlayer.PIStartAudio(SoundType.RoundFour);
                    break;
                case Round.Round5:
                    AudioPlayer.PIStartAudio(SoundType.RoundFive);
                    break;
                default:
                    break;
            }
        }




        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
        }

    }
}
