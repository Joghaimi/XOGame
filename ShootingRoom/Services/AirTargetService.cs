using Iot.Device.Mcp3428;
using IronPython.Compiler.Ast;
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
        private CancellationTokenSource _cts;
        Stopwatch GameStopWatch = new Stopwatch();
        private GPIOController _controller;
        int bigTargetHitScore = 0;

        //int Score = 0;
        int numberOfAchivedInRow = 0;
        List<int> LevelsRequiredScoreList = new List<int>() { 75, 100, 125, 150, 300 };
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            GameStopWatch.Start();
            AirTargetList.Add(new AirTargetController(ShelfLight, Target1, Target2, Target3, Target4, Target5));
            AirTargetList.Add(new AirTargetController(ShelfLight2, Target6, Target7, Target8, Target9, Target10));
            AirTargetList.Add(new AirTargetController(ShelfLight3, Target11, Target12, Target13, Target14, Target15));
            AirTargetList.Add(new AirTargetController(ShelfLight4, Target16, Target17, Target18, Target19, Target20));
            MCP23Controller.PinModeSetup(TargetMotorControl, PinMode.Output);
            MCP23Controller.PinModeSetup(BigTargetIRSensor, PinMode.Input);
            //_controller.Setup(BigTargetRelay, PinMode.Output);
            //_controller.Setup(GunShootRelay, PinMode.Output);
            ControlPin(BigTargetRelay, false);
            ControlPin(BigTargetRelay, false);
            ControlPin(UVLight, false);
            RGBLight.SetColor(RGBColor.White);


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            Stopwatch ShelfTimer = new Stopwatch();
            Stopwatch LevelTimer = new Stopwatch();


            while (true)
            {
                if (IsGameStartedOrInGoing())
                {

                    if (!VariableControlService.IsAirTargetServiceStarted)
                        VariableControlService.IsAirTargetServiceStarted = true;
                    ControlPin(GunShootRelay, true);
                    BigTargetTask();
                    ReturnAllTargets();
                    if (!IsGameStartedOrInGoing())
                        break;
                    int level = 0;
                    foreach (var LevelScore in LevelsRequiredScoreList)
                    {
                        ControlRoundSound(VariableControlService.GameRound);
                        if (IsGameStartedOrInGoing())
                        {

                            level++;
                            int ActualLevelScore = 0;
                            int numberOfRightHits = 0;
                            int numberOfWrongHits = 0;
                            LevelTimer.Restart();
                            while (LevelScore > ActualLevelScore && LevelTimer.ElapsedMilliseconds < 96000)
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
                                    while (ShelfTimer.ElapsedMilliseconds <= (10000 - 2000 * (level - 1)))
                                    {
                                        if (!IsGameStartedOrInGoing())
                                            break;
                                        foreach (var element in AirTargetList)
                                        {
                                            if (!IsGameStartedOrInGoing())
                                                break;
                                            (bool state, int itemScore, numberOfHit, int targetNumber) = element.TargetStatus();
                                            ActualLevelScore += itemScore;

                                            if (itemScore > 0 && state)
                                            {

                                                numberOfRightHits = Scored(true, IsItUV, numberOfRightHits);
                                                Console.WriteLine($"Score {ActualLevelScore}");
                                            }
                                            else if (itemScore < 0 && state)
                                            {
                                                numberOfWrongHits = Scored(false, IsItUV, numberOfWrongHits);
                                                Console.WriteLine($"Score {ActualLevelScore}");
                                            }
                                            if (numberOfHit == 20)
                                                break;
                                            Thread.Sleep(10);
                                        }

                                    }
                                    if (numberOfHit == 20)

                                        break;
                                    item.UnSelectTarget(false);
                                    if (numberOfHit == 20 || ActualLevelScore >= LevelScore)
                                        break;
                                }
                                if (numberOfHit == 20 || ActualLevelScore >= LevelScore)

                                    break;

                            }
                            //Console.WriteLine("================= Next Level");
                            //Console.WriteLine($"ActualLevelScore {ActualLevelScore}");
                            //Console.WriteLine($"numberOfRightHits {numberOfRightHits}");
                            //Console.WriteLine($"numberOfWrongHits {numberOfWrongHits}");


                            // Calculate The Score 


                            if (level == 5)
                            {
                                VariableControlService.TeamScore.ShootingRoomScore += (numberOfRightHits * 10 - numberOfWrongHits * 10);
                                numberOfAchivedInRow = 0;
                                ControlPin(UVLight, true);
                                RGBLight.SetColor(RGBColor.Off);
                                IsItUV = true;

                            }
                            else if (ActualLevelScore >= LevelScore && numberOfAchivedInRow == 1)
                            {
                                VariableControlService.TeamScore.ShootingRoomScore += (LevelScore * 2);
                                numberOfAchivedInRow = 0;
                                ControlPin(UVLight, true);
                                RGBLight.SetColor(RGBColor.Off);
                                IsItUV = true;
                            }
                            else if (ActualLevelScore >= LevelScore)
                            {
                                VariableControlService.TeamScore.ShootingRoomScore += (LevelScore);
                                numberOfAchivedInRow++;
                                ControlPin(UVLight, false);
                                RGBLight.SetColor(RGBColor.White);
                                IsItUV = false;
                            }
                            else
                            {
                                numberOfAchivedInRow = 0;
                                VariableControlService.TeamScore.ShootingRoomScore += (numberOfRightHits * 5 - numberOfWrongHits * 3);
                                ControlPin(UVLight, false);
                                RGBLight.SetColor(RGBColor.White);
                                IsItUV = false;
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
        }

        private void BigTargetTask()
        {
            Stopwatch BigTargetTimer = new Stopwatch();
            ControlPin(BigTargetRelay, true);
            BigTargetTimer.Start();
            BigTargetTimer.Restart();
            while (true && BigTargetTimer.ElapsedMilliseconds < 60000)
            {
                if (!IsGameStartedOrInGoing())
                    break;
                if (MCP23Controller.Read(MasterDI.IN1))
                {
                    bigTargetHitScore++;
                    Console.WriteLine($"Target Hit # {bigTargetHitScore}");
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                    Thread.Sleep(500);
                }
                if (bigTargetHitScore == 5)
                {
                    VariableControlService.TeamScore.ShootingRoomScore += 100;
                    Console.WriteLine($"Remove Big Target and start the game");
                    break;
                }
                Thread.Sleep(10);
            }
            ControlPin(BigTargetRelay, false);
            Console.WriteLine($"Big Target Finished");
        }


        private void StopAirTargetService()
        {
            ControlPin(GunShootRelay, false);
            VariableControlService.IsAirTargetServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            Thread.Sleep(1000);
            AudioPlayer.PIStopAudio();
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

        }
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
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
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                else
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Off);
                return score++;
            }
            else
            {
                AudioPlayer.PIStartAudio(SoundType.Descend);
                RGBLight.SetColor(RGBColor.Red);
                if (!inUVMode)
                    RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
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
            //_cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            //_cts.Dispose();
        }

    }
}
