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
        AirTargetModel Target20 = new AirTargetModel(HatInputPin.IR20, 5);
        MCP23Pin TargetMotorControl = MasterOutputPin.OUTPUT5;
        MCP23Pin BigTargetIRSensor = MasterDI.IN1;

        int BigTargetRelay = MasterOutputPin.GPIO23;
        int GunShootRelay = MasterOutputPin.GPIO24;


        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        private GPIOController _controller;
        int SlowPeriod = 3000;
        int MediumPeriod = 3000;

        int slowChangeTime = 1000;
        int mediumChangeTime = 700;
        int highChangeTime = 500;
        int changingSpeed = 1000;
        int bigTargetHitScore = 0;

        int Score = 0;

        List<int> scoreList = new List<int>();

        int stageOneScore = 0;
        int stageOneNeededScore = 0;

        int stageSecoundScore = 0;
        int stageSecoundNeededScore = 0;

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
            _controller.Setup(BigTargetRelay, PinMode.Output);
            _controller.Setup(GunShootRelay, PinMode.Output);
            RGBLight.SetColor(RGBColor.White);

            scoreList.Add(100);
            scoreList.Add(150);
            scoreList.Add(200);
            scoreList.Add(255);


            MCP23Controller.PinModeSetup(Target1.Pin, PinMode.Input);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {

            while (true) {

                Console.WriteLine(MCP23Controller.Read(Target1.Pin));
                Thread.Sleep(1000);
            }
            Stopwatch timer = new Stopwatch();
            Stopwatch timerToStart = new Stopwatch();
            int activeTargetIndox = -1;
            Random random = new Random();
            int randomTime = random.Next(5000, 15000);
            timerToStart.Restart();
            timer.Restart();

            // Start Both Selonoid @TODO Depend on the api start game
            //_controller.Setup(MasterOutputPin.GPIO23, PinMode.Output);
            //_controller.Setup(MasterOutputPin.GPIO24, PinMode.Output);
            //_controller.Write(MasterOutputPin.GPIO23, true);// GPIO24 -> gunsenoloid 
            //_controller.Write(MasterOutputPin.GPIO24, true);// GPIO24 -> gunsenoloid 

            //ControlPin(BigTargetRelay, true);
            //ControlPin(GunShootRelay, true);

            //while (true)
            //{
            //    if (MCP23Controller.Read(MasterDI.IN1))
            //    {
            //        bigTargetHitScore++;
            //        Console.WriteLine($"Target Hit # {bigTargetHitScore}");
            //        RGBLight.SetColor(RGBColor.Blue);
            //        RGBLight.TurnRGBColorDelayed(RGBColor.White);
            //        Thread.Sleep(500);
            //    }
            //    if (bigTargetHitScore == 5)
            //    {
            //        Console.WriteLine($"Remove Big Target and start the game");
            //        ControlPin(BigTargetRelay, false);
            //        break;
            //    }
            //    Thread.Sleep(10);
            //}

            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
            Thread.Sleep(5000);
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);

            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var LevelScore in scoreList)
                {
                    int ActualLevelScore = 0;
                    int numberOfRightHits = 0;
                    int numberOfWrongHits = 0;
                    while (LevelScore > ActualLevelScore)
                    {
                        foreach (var item in AirTargetList)
                        {
                            timer.Restart();
                            item.Select();
                            while (timer.ElapsedMilliseconds <= 5000)
                            {
                                foreach (var element in AirTargetList)
                                {
                                    while (true)
                                    {
                                        (bool statea, int itemScorea) = element.TargetOneStatus();
                                        ActualLevelScore += itemScorea;
                                        Console.WriteLine($"itemScore:{itemScorea} state:{statea} condition:{itemScorea > 0 && statea}");
                                        if (itemScorea > 0 && statea)
                                        {
                                            //Scored();
                                            numberOfRightHits++;

                                        }
                                        Thread.Sleep(300);

                                    }
                                    (bool state, int itemScore) = element.TargetOneStatus();
                                    ActualLevelScore += itemScore;
                                    if (itemScore > 0 && state)
                                    {
                                        Scored();
                                        numberOfRightHits++;
                                    }
                                    else if (itemScore < 0 && state)
                                    {
                                        numberOfWrongHits++;
                                    }
                                    (state, itemScore) = element.TargetTwoStatus();
                                    ActualLevelScore += itemScore;
                                    if (itemScore > 0 && state)
                                    {
                                        Scored();
                                        numberOfRightHits++;
                                    }
                                    else if (itemScore < 0 && state)
                                    {
                                        numberOfWrongHits++;
                                    }
                                    (state, itemScore) = element.TargetThreeStatus();
                                    ActualLevelScore += itemScore;
                                    if (itemScore > 0 && state)
                                    {
                                        Scored();
                                        numberOfRightHits++;
                                    }
                                    else if (itemScore < 0 && state)
                                    {
                                        numberOfWrongHits++;
                                    }
                                    (state, itemScore) = element.TargetFourStatus();
                                    ActualLevelScore += itemScore;
                                    if (itemScore > 0 && state)
                                    {
                                        Scored();
                                        numberOfRightHits++;
                                    }
                                    else if (itemScore < 0 && state)
                                    {
                                        numberOfWrongHits++;
                                    }
                                    (state, itemScore) = element.TargetFiveStatus();
                                    ActualLevelScore += itemScore;
                                    if (itemScore > 0 && state)
                                    {
                                        Scored();
                                        numberOfRightHits++;
                                    }
                                    else if (itemScore < 0 && state)
                                    {
                                        numberOfWrongHits++;
                                    }
                                }

                            }
                            item.UnSelectTarget();
                            Console.WriteLine(Score);
                        }
                    }

                    Console.WriteLine($"ActualLevelScore {ActualLevelScore}");
                    Console.WriteLine($"numberOfRightHits {numberOfRightHits}");
                    Console.WriteLine($"numberOfWrongHits {numberOfWrongHits}");



                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
                    Thread.Sleep(5000);
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);

                    foreach (var item in AirTargetList)
                    {
                        item.UnSelectTarget();
                    }
                }


                Thread.Sleep(10);
            }
        }

        private void ControlPin(int pinNumber, bool state)
        {
            if (state)
            {
                _controller.Setup(pinNumber, PinMode.Output);
                _controller.Write(pinNumber, true);// GPIO24 -> gunsenoloid
            }
            else
            {
                _controller.Setup(pinNumber, PinMode.Input);
                _controller.Write(pinNumber, true);// GPIO24 -> gunsenoloid
            }

        }
        private void Scored()
        {
            AudioPlayer.PIStartAudio(SoundType.Bonus);
            RGBLight.SetColor(RGBColor.Blue);
            RGBLight.TurnRGBColorDelayed(RGBColor.White);
        }



        private async Task TimingService(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {
                if (!IsTimerStarted)
                {
                    GameStopWatch.Start();
                    IsTimerStarted = true;
                }
            }
            while (true)
            {
                if (GameStopWatch.ElapsedMilliseconds > SlowPeriod && GameStopWatch.ElapsedMilliseconds < MediumPeriod)
                {
                    changingSpeed = mediumChangeTime;
                }
                else if (GameStopWatch.ElapsedMilliseconds > MediumPeriod)
                {
                    changingSpeed = highChangeTime;
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

    }
}

//if (item.TargetOneStatus())
//{
//    AudioPlayer.PIStartAudio(SoundType.Bonus);
//    RGBLight.SetColor(RGBColor.Blue);
//    RGBLight.TurnRGBColorDelayed(RGBColor.White);
//    Score++;
//}
//if (item.TargetTwoStatus())
//{
//    AudioPlayer.PIStartAudio(SoundType.Bonus);
//    RGBLight.SetColor(RGBColor.Blue);
//    RGBLight.TurnRGBColorDelayed(RGBColor.White);
//    Score++;
//}
//if (item.TargetThreeStatus())
//{
//    AudioPlayer.PIStartAudio(SoundType.Bonus);
//    RGBLight.SetColor(RGBColor.Blue);
//    RGBLight.TurnRGBColorDelayed(RGBColor.White);
//    Score++;
//}
//if (item.TargetFourStatus())
//{
//    AudioPlayer.PIStartAudio(SoundType.Bonus);
//    RGBLight.SetColor(RGBColor.Blue);
//    RGBLight.TurnRGBColorDelayed(RGBColor.White);
//    Score++;
//}
//if (item.TargetFiveStatus())
//{
//    AudioPlayer.PIStartAudio(SoundType.Bonus);
//    RGBLight.SetColor(RGBColor.Blue);
//    RGBLight.TurnRGBColorDelayed(RGBColor.White);
//    Score++;
//}

//if (VariableControlService.IsTheGameStarted && IsTimerStarted)
//{
//    if (!activeTarget && timerToStart.ElapsedMilliseconds > randomTime)
//    {
//        activeTarget = true;
//        activeTargetIndox = random.Next(0, 8);
//        AirTargetList[activeTargetIndox].Select(true);
//        timer.Restart();
//        timerToStart.Restart();
//        randomTime = random.Next(5000, 15000);
//    }
//    if (activeTarget & timer.ElapsedMilliseconds >= changingSpeed)
//    {
//        AirTargetList[activeTargetIndox].Select(false);
//        activeTargetIndox = -1;
//        activeTarget = false;
//        timer.Restart();
//        timerToStart.Restart();
//    }
//    if (activeTarget && activeTargetIndox > -1)
//    {
//        bool isPreesed = AirTargetList[activeTargetIndox].Status();
//        if (isPreesed)
//        {
//            activeTarget = false;
//            AirTargetList[activeTargetIndox].Select(false);
//            JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
//            VariableControlService.ActiveTargetPressed++;
//            Console.WriteLine($"Score {VariableControlService.ActiveTargetPressed}");
//            activeTargetIndox = -1;
//            timerToStart.Restart();
//            timer.Restart();
//        }
//    }
//}
// Sleep for a short duration to avoid excessive checking