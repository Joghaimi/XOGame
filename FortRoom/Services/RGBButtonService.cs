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
        Random random = new Random();

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
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
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
            while (!cancellationToken.IsCancellationRequested)
            {
                try
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

                        while (GameStopWatch.ElapsedMilliseconds < 70000)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;

                            int numberOfTurenedOnButton = 0;
                            for (int i = 0; i < ((int)gameLevel) + 1; i++)
                            {
                                if (!IsGameStartedOrInGoing())
                                    break;
                                TurnRandomRGBButtonWithColor(selectedColor);
                                numberOfTurenedOnButton++;
                            }
                            Console.WriteLine($"numberOfTurenedOnButton :numberOfTurenedOnButton");
                            TurnUnSelectedRGBButtonWithColor(VariableControlService.WrongColor);


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
                                            TurnUnSelectedRGBButtonWithColor(VariableControlService.WrongColor);
                                            _logger.LogTrace("Turn On All RGB Button");
                                        }

                                        bool itemSelected = !item.CurrentStatusWithCheckForDelay() && item.isSet();// item.CurrentColor() == selectedColor;
                                        bool itemOnButNotSelected = !item.CurrentStatusWithCheckForDelay() && !item.isSet() && item.CurrentColor()!=RGBColor.Off;
                                        if (itemSelected)
                                        {
                                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                                            RGBLight.SetColor(VariableControlService.CorrectColor);
                                            RGBLight.SetPriority(true);
                                            item.TurnColorOn(RGBColor.Off);
                                            item.Set(false);
                                            RGBLight.TurnRGBColorDelayedASecAndPriorityRemove(VariableControlService.DefaultColor);
                                            numberOfClickedButton++;
                                            item.BlockForASec();
                                            //VariableControlService.ActiveButtonPressed++;
                                            VariableControlService.TeamScore.FortRoomScore += 10;
                                            numberOfTurenedOnButton--;
                                            Console.WriteLine($"Selected {numberOfTurenedOnButton} score {VariableControlService.TeamScore.FortRoomScore}");
                                        }
                                        else if (itemOnButNotSelected)
                                        {
                                            RGBLight.SetColor(RGBColor.Red);
                                            RGBLight.SetPriority(true);
                                            RGBLight.TurnRGBColorDelayedASecAndPriorityRemove(VariableControlService.DefaultColor);
                                            item.TurnColorOn(RGBColor.Off);
                                            // item.BlockForASec();
                                            VariableControlService.TeamScore.FortRoomScore -= 5;
                                            Console.WriteLine($"UnSelected new score{VariableControlService.TeamScore.FortRoomScore}");
                                        }
                                    }
                                    else if (!isRGBButtonTurnedOffBecauseThePressureMate)
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
                        if (VariableControlService.GameRound < Round.Round5)
                        {
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
                catch (Exception ex)
                {
                    _logger.LogError($"{ex.Message}");
                }

            }
        }



        public void StartTheGameTask(RGBColor color, Round round)
        {
            int level = (int)round;
            if (level > RGBButtonList.Count)
                return;
            int button1Index = SelectRandomNumberInRange(0, 5);
            int button2Index = SelectRandomNumberInRange(5, 9);
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
                        RGBLight.SetColor(VariableControlService.CorrectColor);
                        RGBLight.SetPriority(true);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBButtonList[button1Index].Set(false);
                        RGBButtonList[button2Index].Set(false);
                        RGBButtonList[button1Index].TurnColorOn(RGBColor.Off);
                        RGBButtonList[button2Index].TurnColorOn(RGBColor.Off);

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



        private int SelectRandomNumberInRange(int start, int end, bool canBeNegative = false)
        {
            try
            {
                var value = random.Next(start, end);
                if (value < 0 || value > end)
                {
                    return SelectRandomNumberInRange(start, end, canBeNegative);
                }
                return value;

            }
            catch (Exception e)
            {

                _logger.LogError("SelectRandomNumberInRange Exception , Try Agan");
                return SelectRandomNumberInRange(start, end, canBeNegative);

            }

        }







        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.GameStatus == GameStatus.Started;
        }
        private void StopRGBButtonService(bool withoutFnishAudio = false)
        {
            StopRGBButton();
            _logger.LogTrace("RGB Button Service Out");
            RGBLight.SetPriority(false);
            RGBLight.SetColor(VariableControlService.DefaultColor);
            _logger.LogTrace("Set Rounds");

            VariableControlService.GameRound = Round.Round1;
            VariableControlService.IsRGBButtonServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;

            // Reset RGB Button List
            RGBColors = new List<RGBColor> {
            RGBColor.Blue,
            RGBColor.White,
            RGBColor.Turquoise,
            RGBColor.Yellow,
            RGBColor.purple};



            _logger.LogTrace("Audio Off");
            if (!withoutFnishAudio)
            {
                AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
                Thread.Sleep(1000);
                AudioPlayer.PIStopAudio();
            }
            AudioPlayer.PIForceStopAudio();
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
            try
            {
                Random random = new Random();
                int randomColorIndex = random.Next(0, RGBColors.Count());
                var selectedColor = RGBColors[randomColorIndex];
                RGBColors.RemoveAt(randomColorIndex);
                Console.WriteLine(selectedColor);
                return selectedColor;
            }
            catch (Exception ex)
            {
                _logger.LogError($"SelectRandomRGBColorForLevel Exception , Try Again {ex.Message}");
                throw;
            }
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
            _logger.LogTrace("Turn RGB Button Color Off");
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
            StopRGBButtonService(true);
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }
    }

}