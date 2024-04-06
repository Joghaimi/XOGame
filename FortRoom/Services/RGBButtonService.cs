using Library.Media;
using Library.RGBLib;
using Library.Enum;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.PinMapping;
using Library.GPIOLib;
using Iot.Device.Mcp3428;
using Iot.Device.BrickPi3.Sensors;
using System.Xml.Linq;
using RGBColor = Library.RGBColor;

namespace FortRoom.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private readonly ILogger<RGBButtonService> _logger;
        Stopwatch GameStopWatch = new Stopwatch();
        private CancellationTokenSource _cts;
        //List<(int, int)> ButtonTaskList = new List<(int, int)>
        //{
        //    (2,3),
        //    (8,6),
        //    (5,0),
        //    (7,4),
        //    (2,6),
        //};
        List<int> LevelNumbers = new List<int> { 1, 2, 2, 3, 4 };
        //List<int> buttonGroupTwo = new List<int> { 0, 6, 7, 8, 9 };

        int CurrentColor = 0;

        List<RGBColor> RGBColors = new List<RGBColor> {
            RGBColor.Blue,
            RGBColor.White,
            RGBColor.Turquoise,
            RGBColor.Yellow,
            RGBColor.purple
        };



        public RGBButtonService(ILogger<RGBButtonService> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("Start RGBButtonService");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR12, RGBButtonPin.RGBG12, RGBButtonPin.RGBB12, RGBButtonPin.RGBPB12));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15));
            GameStopWatch.Start();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            int level = 0;
            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    if (!VariableControlService.IsRGBButtonServiceStarted)
                        VariableControlService.IsRGBButtonServiceStarted = true;
                    // Select Color 
                    var selectedColor = SelectRandomRGBColorForLevel();
                    ControlRoundSound(VariableControlService.GameRound);
                    TurnRGBButtonWithColor(RGBColor.Off, false);
                    StartTheGameTask(selectedColor, VariableControlService.GameRound);
                    TurnRGBButtonWithColor(RGBColor.Off, false);




                    byte numberOfClickedButton = 0;
                    GameStopWatch.Restart();
                    bool isRGBButtonTurnedOffBecauseThePressureMate = false;
                    Level gameLevel = Level.Level1;
                    while (GameStopWatch.ElapsedMilliseconds < 90000)
                    {

                        int numberOfTurenedOnButton = 0;
                        for (int i = 0; i < ((int)gameLevel) + 1; i++)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            TurnRandomRGBButtonWithColor(selectedColor);
                            numberOfTurenedOnButton++;
                        }
                        Console.WriteLine($"numberOfTurenedOnButton :numberOfTurenedOnButton");
                        TurnUnSelectedRGBButtonWithColor(RGBColor.Red);


                        Console.WriteLine($"gameLevel: {gameLevel}");
                        while (true)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            foreach (var item in RGBButtonList)
                            {

                                if (!VariableControlService.IsPressureMateActive)
                                {
                                    if (isRGBButtonTurnedOffBecauseThePressureMate)
                                    {
                                        isRGBButtonTurnedOffBecauseThePressureMate = false;
                                        ControlTheColorOfAllSetRGBButton(selectedColor);
                                        TurnUnSelectedRGBButtonWithColor(RGBColor.Red);
                                        _logger.LogTrace("Turn On All RGB Button");
                                    }

                                    bool itemSelected = !item.CurrentStatusWithCheckForDelay() && item.isSet();// item.CurrentColor() == selectedColor;
                                    bool itemOnButNotSelected = !item.CurrentStatusWithCheckForDelay() && !item.isSet();
                                    if (itemSelected)
                                    {
                                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                                        RGBLight.SetColor(RGBColor.Yellow);
                                        RGBLight.SetPriority(true);
                                        item.TurnColorOn(RGBColor.Off);
                                        item.Set(false);
                                        RGBLight.TurnRGBColorDelayedASecAndPriorityRemove(VariableControlService.DefaultColor);
                                        numberOfClickedButton++;
                                        item.BlockForASec();
                                        //VariableControlService.ActiveButtonPressed++;
                                        VariableControlService.TeamScore.FortRoomScore += 10;
                                        numberOfTurenedOnButton--;
                                        Console.WriteLine($"Selected {numberOfTurenedOnButton} ");
                                    }
                                    else if (itemOnButNotSelected)
                                    {
                                        RGBLight.SetColor(RGBColor.Red);
                                        RGBLight.SetPriority(true);
                                        RGBLight.TurnRGBColorDelayedASecAndPriorityRemove(VariableControlService.DefaultColor);
                                        item.TurnColorOn(RGBColor.Off);
                                        item.BlockForASec();
                                        VariableControlService.TeamScore.FortRoomScore -= 5;
                                        Console.WriteLine("UnSelected ");
                                    }
                                }
                                else if(!isRGBButtonTurnedOffBecauseThePressureMate)
                                {
                                    isRGBButtonTurnedOffBecauseThePressureMate = true;
                                    ControlTheColorOfAllSetRGBButton(RGBColor.Off);
                                    TurnUnSelectedRGBButtonWithColor(RGBColor.Off);
                                    _logger.LogTrace("TurnOff All RGB Button");

                                }
                            }

                            if (numberOfTurenedOnButton == 0)
                                break;
                        }
                        Console.WriteLine("Out From For While 1");
                        Thread.Sleep(1000);
                        if (gameLevel != Level.Level5)
                            gameLevel = NextLevel(gameLevel);
                        Console.WriteLine("Level Selected");
                        Thread.Sleep(10);
                    }
                    Console.WriteLine("Out From While 2");
                    if (CurrentColor < 2)
                        CurrentColor++;
                    else
                        CurrentColor = 0;
                    if (level < 4)
                    {
                        level++;
                        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                        ApplyChangesForTheNextRound();
                    }
                    else
                        StopRGBButtonService();
                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsRGBButtonServiceStarted)
                {
                    _logger.LogInformation("RGB Service Stopped");
                    StopRGBButtonService();
                }
            }
        }



        public void StartTheGameTask(RGBColor color, Round round)
        {
            int level = (int)round;
            if (level > RGBButtonList.Count)
                return;
            Random random = new Random();
            int button1Index = random.Next(0, 5);
            int button2Index = random.Next(5, 9);
            bool Button1 = false; bool Button2 = false;
            RGBButtonList[button1Index].TurnColorOn(color);
            RGBButtonList[button2Index].TurnColorOn(color);
            RGBButtonList[button1Index].Set(true);
            RGBButtonList[button2Index].Set(true);
            bool isRGBButtonTurnedOffBecauseThePressureMate = false;

            while (!Button1 || !Button2)
            {

                if (!IsGameStartedOrInGoing())
                    break;



                if (!VariableControlService.IsPressureMateActive)
                {
                    if (isRGBButtonTurnedOffBecauseThePressureMate)
                    {
                        isRGBButtonTurnedOffBecauseThePressureMate = false;
                        ControlTheColorOfAllSetRGBButton(color);
                    }
                    bool button1Status = !RGBButtonList[button1Index].CurrentStatus() && RGBButtonList[button1Index].isSet();
                    bool button2Status = !RGBButtonList[button2Index].CurrentStatus() && RGBButtonList[button2Index].isSet();
                    if (button1Status && button2Status)
                    {
                        RGBLight.SetColor(RGBColor.Yellow);
                        RGBLight.SetPriority(true);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBButtonList[button1Index].Set(false);
                        RGBButtonList[button2Index].Set(false);
                        RGBButtonList[button1Index].TurnColorOn(RGBColor.Off);

                        RGBLight.TurnRGBColorDelayedASecAndPriorityRemove(VariableControlService.DefaultColor);
                        Button1 = true;
                        Button2 = true;
                    }
                }
                else
                {
                    isRGBButtonTurnedOffBecauseThePressureMate = true;
                    ControlTheColorOfAllSetRGBButton(RGBColor.Off);
                }

                Thread.Sleep(10);
            }
            if (!IsGameStartedOrInGoing())
                return;
            Thread.Sleep(400);
        }











        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
        private void StopRGBButtonService()
        {
            StopRGBButton();
            VariableControlService.GameRound = Round.Round1;
            VariableControlService.IsRGBButtonServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            Thread.Sleep(1000);
            AudioPlayer.PIStopAudio();
        }
        private Round NextRound(Round round)
        {
            return (Round)((int)round + 1);
        }
        private Level NextLevel(Level level)
        {
            switch (level)
            {
                case Level.Level1:
                    return Level.Level2;
                case Level.Level2:
                    return Level.Level3;
                case Level.Level3:
                    return Level.Level4;
                case Level.Level4:
                    return Level.Level5;
                case Level.Level5:
                    return Level.Finished;
                default:
                    return Level.Level1;
            }
        }
        private void ApplyChangesForTheNextRound()
        {
            VariableControlService.IsThingsChangedForTheNewRound = false;
        }

        public void TurnRGBButtonWithColor(RGBColor color)
        {
            foreach (var item in RGBButtonList)
            {
                item.TurnColorOn(color);
                item.Set(true); ;
            }
        }
        public void TurnRGBButtonWithColor(RGBColor color, bool isSet)
        {
            foreach (var item in RGBButtonList)
            {
                item.TurnColorOn(color);
                item.Set(isSet); ;
            }
        }
        public RGBColor SelectRandomRGBColorForLevel()
        {
            Random random = new Random();
            int randomColorIndex = random.Next(0, RGBColors.Count());
            var selectedColor = RGBColors[randomColorIndex];
            RGBColors.RemoveAt(randomColorIndex);
            Console.WriteLine(selectedColor);
            return selectedColor;
        }

        public void TurnRandomRGBButtonWithColor(RGBColor color)
        {
            Random random = new Random();
            Console.WriteLine("Select Color");
            while (true)
            {
                int button1Index = random.Next(0, 9);
                if (!RGBButtonList[button1Index].isSet())
                {
                    RGBButtonList[button1Index].TurnColorOn(color);
                    RGBButtonList[button1Index].Set(true);
                    break;
                }
            }
            Console.WriteLine("Select Color =========");

        }
        public void TurnUnSelectedRGBButtonWithColor(RGBColor color)
        {

            foreach (var item in RGBButtonList)
            {
                if (!item.isSet())
                    item.TurnColorOn(color);
            }
        }

        private void StopRGBButton()
        {
            foreach (var item in RGBButtonList)
            {
                item.TurnColorOn(RGBColor.Off);
                item.Set(false);
            }
        }
        public void ControlTheColorOfAllSetRGBButton(RGBColor color)
        {
            foreach (var item in RGBButtonList)
            {
                if (item.isSet())
                    item.TurnColorOn(color);
            }
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
            StopRGBButtonService();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }
    }

}



//if (gameLevel > Level.Level5)



//if (!IsGameStartedOrInGoing())
//    break;
//TurnRGBButtonWithColor(RGBColor.Off);
//// Level Number
//int numberOfTurenedOnButton = 0;
//for (int i = 0; i < (int)gameLevel; i++)
//{
//    if (!IsGameStartedOrInGoing())
//        break;
//    TurnRandomRGBButtonWithColor(selectedColor);
//    numberOfTurenedOnButton++;
//}

//TurnUnSelectedRGBButtonWithColor(RGBColor.Red);
//while (true)
//{
//    if (!IsGameStartedOrInGoing())
//        break;
//    foreach (var item in RGBButtonList)
//    {

//        if (!VariableControlService.IsPressureMateActive)
//        {
//            if (isRGBButtonTurnedOffBecauseThePressureMate)
//            {
//                isRGBButtonTurnedOffBecauseThePressureMate = false;
//                ControlTheColorOfAllSetRGBButton(selectedColor);
//            }

//            bool itemSelected = !item.CurrentStatus() && item.isSet();// item.CurrentColor() == selectedColor;
//            bool itemOnButNotSelected = !item.CurrentStatus();
//            if (itemSelected)
//            {
//                AudioPlayer.PIStartAudio(SoundType.Bonus);
//                RGBLight.SetColor(RGBColor.Yellow);
//                item.TurnColorOn(RGBColor.Off);
//                item.Set(false);
//                RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
//                numberOfClickedButton++;
//                //VariableControlService.ActiveButtonPressed++;
//                VariableControlService.TeamScore.FortRoomScore += 10;
//                numberOfTurenedOnButton--;
//            }
//            else if (itemOnButNotSelected)
//            {
//                RGBLight.SetColor(RGBColor.Red);
//                RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
//                item.TurnColorOn(RGBColor.Off);
//                VariableControlService.TeamScore.FortRoomScore -= 5;
//            }
//        }
//        else if (!isRGBButtonTurnedOffBecauseThePressureMate)
//        {
//            isRGBButtonTurnedOffBecauseThePressureMate = true;
//            ControlTheColorOfAllSetRGBButton(RGBColor.Off);
//        }
//        if (numberOfTurenedOnButton == 0)
//            break;


//    }
//    if (numberOfTurenedOnButton == 0)
//        break;
//}

//gameLevel = NextLevel(gameLevel);
//if (gameLevel > Level.Level5)




//foreach (var item in RGBButtonList)
//{
//    if (!IsGameStartedOrInGoing())
//        break;
//    if (!VariableControlService.IsPressureMateActive)
//    {
//        if (isRGBButtonTurnedOffBecauseThePressureMate)
//        {
//            isRGBButtonTurnedOffBecauseThePressureMate = false;
//            ControlTheColorOfAllSetRGBButton(selectedColor);

//        }
//        bool itemSelected = !item.CurrentStatus() && item.isSet();// item.CurrentColor() == selectedColor;
//        if (itemSelected)
//        {
//            AudioPlayer.PIStartAudio(SoundType.Bonus);
//            RGBLight.SetColor(RGBColor.Yellow);
//            item.TurnColorOn(RGBColor.Off);
//            item.Set(false);
//            RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor);
//            numberOfClickedButton++;
//            VariableControlService.ActiveButtonPressed++;
//            VariableControlService.TeamScore.FortRoomScore += 10;
//        }
//    }
//    else if (!isRGBButtonTurnedOffBecauseThePressureMate)
//    {
//        isRGBButtonTurnedOffBecauseThePressureMate = true;
//        ControlTheColorOfAllSetRGBButton(RGBColor.Off);
//    }


//}
//if (numberOfClickedButton == RGBButtonList.Count())
//    break;
//if (!IsGameStartedOrInGoing())
//    break;