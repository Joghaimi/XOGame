﻿using Iot.Device.Mcp3428;
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


        private CancellationTokenSource _cts;
        Stopwatch GameStopWatch = new Stopwatch();
        private GPIOController _controller;
        int bigTargetHitScore = 0;

        int Score = 0;
        int numberOfAchivedInRow = 0;
        List<int> scoreList = new List<int>();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            GameStopWatch.Start();
            //var airTarget1 = new AirTargetController(ShelfLight, Target1, Target2, Target3, Target4, Target5);
            //var airTarget2 = new AirTargetController(ShelfLight2, Target6, Target7, Target8, Target9, Target10);
            //var airTarget3 = new AirTargetController(ShelfLight3, Target11, Target12, Target13, Target14, Target15);
            //var airTarget4 = new AirTargetController(ShelfLight4, Target16, Target17, Target18, Target19, Target20);
            //airTarget1.Init();
            //airTarget2.Init();
            //airTarget3.Init();
            //airTarget4.Init();
            AirTargetList.Add(new AirTargetController(ShelfLight, Target1, Target2, Target3, Target4, Target5));
            AirTargetList.Add(new AirTargetController(ShelfLight2, Target6, Target7, Target8, Target9, Target10));
            AirTargetList.Add(new AirTargetController(ShelfLight3, Target11, Target12, Target13, Target14, Target15));
            AirTargetList.Add(new AirTargetController(ShelfLight4, Target16, Target17, Target18, Target19, Target20));
            MCP23Controller.PinModeSetup(TargetMotorControl, PinMode.Output);
            MCP23Controller.PinModeSetup(BigTargetIRSensor, PinMode.Input);
            _controller.Setup(BigTargetRelay, PinMode.Output);
            _controller.Setup(GunShootRelay, PinMode.Output);
            _controller.Setup(UVLight, PinMode.Output);
            ControlPin(UVLight, false);
            RGBLight.SetColor(RGBColor.White);
            // Big Target Score === 100 > 1 Min
            scoreList.Add(75); // 2 Min
            scoreList.Add(100); // 2 Min
            scoreList.Add(125); // 2 Min
            scoreList.Add(150); // 2 Min
            scoreList.Add(300); // 2 Min

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            Console.WriteLine($"Game Started");

            Stopwatch Shelftimer = new Stopwatch();
            Stopwatch LevelTimer = new Stopwatch();
            

            while (true)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    ControlPin(GunShootRelay, true);
                    BigTargetTask();
                    ReturnAllTargets();

                    while (!cancellationToken.IsCancellationRequested)
                    {
                        if (!VariableControlService.IsTheGameStarted)
                            break;
                        int level = 0;
                        foreach (var LevelScore in scoreList)
                        {
                            if (VariableControlService.IsTheGameStarted)
                            {
                                level++;
                                int ActualLevelScore = 0;
                                int numberOfRightHits = 0;
                                int numberOfWrongHits = 0;
                                LevelTimer.Restart();
                                while (LevelScore > ActualLevelScore && LevelTimer.ElapsedMilliseconds < 96000)
                                {
                                    if (!VariableControlService.IsTheGameStarted)
                                        break;
                                    int i = 1;
                                    int numberOfHit = 0;
                                    foreach (var item in AirTargetList)
                                    {
                                        if (item.isAllDown())
                                            continue;
                                        Shelftimer.Restart();
                                        item.Select();
                                        Console.WriteLine($" Shelf #{i}");
                                        i++;
                                        while (Shelftimer.ElapsedMilliseconds <= (10000 - 2000 * (level - 1)))
                                        {

                                            int inShelf = 1;
                                            foreach (var element in AirTargetList)
                                            {
                                                (bool state, int itemScore, numberOfHit, int targetNumber) = element.TargetStatus();
                                                ActualLevelScore += itemScore;
                                                if (itemScore > 0 && state)
                                                {
                                                    Console.WriteLine($"Target {targetNumber} Right in {inShelf}");
                                                    Scored();
                                                    numberOfRightHits++;
                                                }
                                                else if (itemScore < 0 && state)
                                                {
                                                    Console.WriteLine($"Target {targetNumber} Wrong {inShelf}");
                                                    WrongScored();
                                                    numberOfWrongHits++;
                                                }
                                                if (numberOfHit == 20)
                                                {
                                                    break;
                                                }
                                                inShelf++;
                                                Thread.Sleep(10);
                                            }


                                        }
                                        if (numberOfHit == 20)
                                        {
                                            Console.WriteLine("All Target Down Go Next Level");
                                            break;
                                        }
                                        Console.WriteLine($"ActualLevelScore {ActualLevelScore}");
                                        Console.WriteLine($"numberOfRightHits {numberOfRightHits}");
                                        Console.WriteLine($"numberOfWrongHits {numberOfWrongHits}");
                                        item.UnSelectTarget(false);
                                        if (numberOfHit == 20 || ActualLevelScore >= LevelScore)
                                        {
                                            Console.WriteLine("All Target Down Go Next Level");
                                            break;
                                        }
                                    }
                                    if (numberOfHit == 20 || ActualLevelScore >= LevelScore)
                                    {
                                        Console.WriteLine("All Target Down Go Next Level");
                                        break;
                                    }

                                }
                                Console.WriteLine("================= Next Level");
                                Console.WriteLine($"ActualLevelScore {ActualLevelScore}");
                                Console.WriteLine($"numberOfRightHits {numberOfRightHits}");
                                Console.WriteLine($"numberOfWrongHits {numberOfWrongHits}");


                                // Calculate The Score 

                                // actualScore != recuired -> numberOfRightHits*5 - numberOfWrongHits*3 
                                // If mession1 and 2 turn uvlight -> if done *2 
                                if (level == 5)
                                {
                                    Score += (numberOfRightHits * 10 - numberOfWrongHits * 10);
                                    numberOfAchivedInRow = 0;
                                    Console.WriteLine($"================= Final Level {Score}");
                                    ControlPin(UVLight, false);

                                }
                                else if (ActualLevelScore == LevelScore && numberOfAchivedInRow == 2)
                                {
                                    Score += (LevelScore * 2);
                                    numberOfAchivedInRow = 0;
                                    ControlPin(UVLight, true);
                                    Console.WriteLine($"================= Double Score {Score}");
                                }
                                else if (ActualLevelScore == LevelScore)
                                {
                                    Score += (LevelScore);
                                    numberOfAchivedInRow++;
                                    Console.WriteLine($"================= Achived Score {Score}");
                                    ControlPin(UVLight, false);
                                }
                                else
                                {
                                    numberOfAchivedInRow = 0;
                                    Score += (numberOfRightHits * 5 - numberOfWrongHits * 3);
                                    Console.WriteLine($"================= Less Score {Score}");
                                    ControlPin(UVLight, false);
                                }

                                ReturnAllTargets();
                                foreach (var item in AirTargetList)
                                {
                                    item.UnSelectTarget(true);
                                }
                            }

                        }
                        ControlPin(GunShootRelay, false);

                        Console.WriteLine($"All Game Finished");
                        break;
                    }
                }
                Thread.Sleep(1);
            }



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
        private void ReturnAllTargets()
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
            Thread.Sleep(3000);
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
        }
        private void Scored()
        {
            AudioPlayer.PIStartAudio(SoundType.Bonus);
            RGBLight.SetColor(RGBColor.Blue);
            RGBLight.TurnRGBColorDelayed(RGBColor.White);
        }

        private void WrongScored()
        {
            AudioPlayer.PIStartAudio(SoundType.Descend);
            RGBLight.SetColor(RGBColor.Red);
            RGBLight.TurnRGBColorDelayed(RGBColor.White);
        }

        // ====== Big Target Task

        private void BigTargetTask()
        {
            Stopwatch BigTargetTimer = new Stopwatch();
            ControlPin(BigTargetRelay, true);
            BigTargetTimer.Start();
            BigTargetTimer.Restart();
            while (true && BigTargetTimer.ElapsedMilliseconds < 60000)
            {
                if (!VariableControlService.IsTheGameStarted)
                    break;
                if (MCP23Controller.Read(MasterDI.IN1))
                {
                    bigTargetHitScore++;
                    Console.WriteLine($"Target Hit # {bigTargetHitScore}");
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBColorDelayed(RGBColor.White);
                    Thread.Sleep(500);
                }
                if (bigTargetHitScore == 5)
                {
                    Score += 100;
                    Console.WriteLine($"Remove Big Target and start the game");
                    break;
                }
                Thread.Sleep(10);
            }
            ControlPin(BigTargetRelay, false);
            Console.WriteLine($"Big Target Finished");
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
