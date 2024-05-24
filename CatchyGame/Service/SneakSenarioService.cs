using Library.GPIOLib;
using Library.Media;
using Library.Model;
using Library.PinMapping;
using Library.RGBLib;
using Library;
using System.Diagnostics;
using Microsoft.Extensions.Logging;

namespace CatchyGame.Service
{
    public class SneakSenarioService : IHostedService, IDisposable
    {


        List<SneakStrip> StripList = new List<SneakStrip>();
        List<RGBButtonSneak> RGBButtonList = new List<RGBButtonSneak>();

        private CancellationTokenSource _cts, _cts2, _cts3;
        Stopwatch GameTiming = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        bool backgroundSoundStarted = false;


        private readonly ILogger<SneakSenarioService> _logger;

        public SneakSenarioService(ILogger<SneakSenarioService> logger)
        {
            _logger = logger;
        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogTrace("Start Thread ... Init");
            MCP23Controller.Init(Room.Fort);

            // RGB Pixel Button 
            var rgb1 = new RGBButtonSneak(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1);
            var rgb2 = new RGBButtonSneak(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2);
            var rgb3 = new RGBButtonSneak(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3);
            var rgb4 = new RGBButtonSneak(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16);
            var rgb5 = new RGBButtonSneak(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5);
            var rgb6 = new RGBButtonSneak(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6);
            var rgb7 = new RGBButtonSneak(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7);
            var rgb8 = new RGBButtonSneak(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8);


            var startRGBButton1 = new RGBButtonSneak(RGBButtonPin.RGBB13, RGBButtonPin.RGBR13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13);
            var startRGBButton2 = new RGBButtonSneak(RGBButtonPin.RGBB14, RGBButtonPin.RGBR14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14);
            var startRGBButton3 = new RGBButtonSneak(RGBButtonPin.RGBB15, RGBButtonPin.RGBR15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15);
            var startRGBButton4 = new RGBButtonSneak(RGBButtonPin.RGBB9, RGBButtonPin.RGBR9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9);
            var startRGBButton5 = new RGBButtonSneak(RGBButtonPin.RGBB10, RGBButtonPin.RGBR10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10);
            var startRGBButton6 = new RGBButtonSneak(RGBButtonPin.RGBB11, RGBButtonPin.RGBB11, RGBButtonPin.RGBB11, RGBButtonPin.RGBB11);




            RGBButtonList.Add(rgb1);
            RGBButtonList.Add(rgb2);
            RGBButtonList.Add(rgb3);
            RGBButtonList.Add(rgb4);
            RGBButtonList.Add(rgb5);
            RGBButtonList.Add(rgb6);
            RGBButtonList.Add(rgb7);
            RGBButtonList.Add(rgb8);
            //RGBButtonList.Add(startRGBButton1);
            //RGBButtonList.Add(startRGBButton2);
            //RGBButtonList.Add(startRGBButton3);
            //RGBButtonList.Add(startRGBButton4);
            //RGBButtonList.Add(startRGBButton5);
            //RGBButtonList.Add(startRGBButton6);


            RGBButtonSneakPixel StripOneButton0 = new RGBButtonSneakPixel(0, startRGBButton1);
            RGBButtonSneakPixel StripOneButton1 = new RGBButtonSneakPixel(19, rgb1);
            RGBButtonSneakPixel StripOneButton2 = new RGBButtonSneakPixel(60, rgb4);
            RGBButtonSneakPixel StripOneButton3 = new RGBButtonSneakPixel(82, rgb5);
            RGBButtonSneakPixel StripOneButton4 = new RGBButtonSneakPixel(123, rgb8);

            RGBButtonSneakPixel StripTwoButton0 = new RGBButtonSneakPixel(213, startRGBButton2);
            RGBButtonSneakPixel StripTwoButton1 = new RGBButtonSneakPixel(242, rgb1);
            RGBButtonSneakPixel StripTwoButton2 = new RGBButtonSneakPixel(280, rgb3);
            RGBButtonSneakPixel StripTwoButton3 = new RGBButtonSneakPixel(320, rgb6);
            RGBButtonSneakPixel StripTwoButton4 = new RGBButtonSneakPixel(355, rgb7);

            RGBButtonSneakPixel StripThreeButton0 = new RGBButtonSneakPixel(440, startRGBButton3);
            RGBButtonSneakPixel StripThreeButton1 = new RGBButtonSneakPixel(475, rgb2);
            RGBButtonSneakPixel StripThreeButton2 = new RGBButtonSneakPixel(520, rgb4);
            RGBButtonSneakPixel StripThreeButton3 = new RGBButtonSneakPixel(535, rgb5);
            RGBButtonSneakPixel StripThreeButton4 = new RGBButtonSneakPixel(591, rgb8);

            RGBButtonSneakPixel StripFourButton0 = new RGBButtonSneakPixel(666, startRGBButton4);
            RGBButtonSneakPixel StripFourButton2 = new RGBButtonSneakPixel(748, rgb1);
            RGBButtonSneakPixel StripFourButton1 = new RGBButtonSneakPixel(784, rgb3);
            RGBButtonSneakPixel StripFourButton3 = new RGBButtonSneakPixel(824, rgb6);
            RGBButtonSneakPixel StripFourButton4 = new RGBButtonSneakPixel(841, rgb7);


            RGBButtonSneakPixel StripFiveButton0 = new RGBButtonSneakPixel(882, startRGBButton5);
            RGBButtonSneakPixel StripFiveButton1 = new RGBButtonSneakPixel(945, rgb2);
            RGBButtonSneakPixel StripFiveButton3 = new RGBButtonSneakPixel(976, rgb4);
            RGBButtonSneakPixel StripFiveButton2 = new RGBButtonSneakPixel(998, rgb5);
            RGBButtonSneakPixel StripFiveButton4 = new RGBButtonSneakPixel(1043, rgb8);

            RGBButtonSneakPixel StripSixButton0 = new RGBButtonSneakPixel(1076, startRGBButton6);
            RGBButtonSneakPixel StripSixButton1 = new RGBButtonSneakPixel(1094, rgb7);
            RGBButtonSneakPixel StripSixButton3 = new RGBButtonSneakPixel(1107, rgb6);
            RGBButtonSneakPixel StripSixButton2 = new RGBButtonSneakPixel(1137, rgb3);
            RGBButtonSneakPixel StripSixButton4 = new RGBButtonSneakPixel(1153, rgb2);




            // Random 5 Work For Each Line
            List<RGBButtonSneakPixel> RGBButtonPixel1 = new List<RGBButtonSneakPixel>();
            List<RGBButtonSneakPixel> RGBButtonPixel2 = new List<RGBButtonSneakPixel>();
            List<RGBButtonSneakPixel> RGBButtonPixel3 = new List<RGBButtonSneakPixel>();
            List<RGBButtonSneakPixel> RGBButtonPixel4 = new List<RGBButtonSneakPixel>();
            List<RGBButtonSneakPixel> RGBButtonPixel5 = new List<RGBButtonSneakPixel>();
            List<RGBButtonSneakPixel> RGBButtonPixel6 = new List<RGBButtonSneakPixel>();

            RGBButtonPixel1.Add(StripOneButton0);
            RGBButtonPixel1.Add(StripOneButton1);
            RGBButtonPixel1.Add(StripOneButton2);
            RGBButtonPixel1.Add(StripOneButton3);
            RGBButtonPixel1.Add(StripOneButton4);

            RGBButtonPixel2.Add(StripTwoButton0);
            RGBButtonPixel2.Add(StripTwoButton1);
            RGBButtonPixel2.Add(StripTwoButton2);
            RGBButtonPixel2.Add(StripTwoButton3);
            RGBButtonPixel2.Add(StripTwoButton4);

            RGBButtonPixel3.Add(StripThreeButton0);
            RGBButtonPixel3.Add(StripThreeButton1);
            RGBButtonPixel3.Add(StripThreeButton2);
            RGBButtonPixel3.Add(StripThreeButton3);
            RGBButtonPixel3.Add(StripThreeButton4);

            RGBButtonPixel4.Add(StripFourButton0);
            RGBButtonPixel4.Add(StripFourButton1);
            RGBButtonPixel4.Add(StripFourButton2);
            RGBButtonPixel4.Add(StripFourButton4);
            RGBButtonPixel4.Add(StripFourButton3);

            RGBButtonPixel5.Add(StripFiveButton0);
            RGBButtonPixel5.Add(StripFiveButton1);
            RGBButtonPixel5.Add(StripFiveButton2);
            RGBButtonPixel5.Add(StripFiveButton4);
            RGBButtonPixel5.Add(StripFiveButton3);

            RGBButtonPixel6.Add(StripSixButton0);
            RGBButtonPixel6.Add(StripSixButton1);
            RGBButtonPixel6.Add(StripSixButton2);
            RGBButtonPixel6.Add(StripSixButton4);
            RGBButtonPixel6.Add(StripSixButton3);

            StripList.Add(new SneakStrip(
                VariableControlService.PlayerOneWarmColor, VariableControlService.PlayerOneStripDefaultColor,
                VariableControlService.StripOneStartIndex, VariableControlService.StripOneEndIndex,
                RGBButtonPixel1, 0, 0, VariableControlService.DefaultWarmLength));


            StripList.Add(new SneakStrip(
                VariableControlService.PlayerTwoWarmColor, VariableControlService.PlayerTwoStripDefaultColor,
                VariableControlService.StripThreeStartIndex, VariableControlService.StripThreeEndIndex,
                RGBButtonPixel3, 1, 1, VariableControlService.DefaultWarmLength));
            StripList.Add(new SneakStrip(
                VariableControlService.PlayerThreeWarmColor, VariableControlService.PlayerThreeStripDefaultColor,
                VariableControlService.StripFourStartIndex, VariableControlService.StripFourEndIndex,
                RGBButtonPixel4, 2, 2, VariableControlService.DefaultWarmLength));
            StripList.Add(new SneakStrip(
            VariableControlService.PlayerFourWarmColor, VariableControlService.PlayerFourStripDefaultColor,
            VariableControlService.StripSixStartIndex, VariableControlService.StripSixEndIndex,
            RGBButtonPixel6, 3, 3, VariableControlService.DefaultWarmLength)); // TO DO


            StripList.Add(new SneakStrip(
                VariableControlService.StripFiveWarmColor, VariableControlService.StripFiveStripDefaultColor,
                VariableControlService.StripFiveStartIndex, VariableControlService.StripFiveEndIndex,
                RGBButtonPixel5, 4, 4, VariableControlService.DefaultWarmLength)); // TO Do
            //StripList.Add(new SneakStrip(
            //    VariableControlService.StripSixWarmColor, VariableControlService.StripSixStripDefaultColor,
            //    VariableControlService.StripSixStartIndex, VariableControlService.StripSixEndIndex,
            //    RGBButtonPixel6, 5, 5, VariableControlService.DefaultWarmLength)); // TO DO

            StripList.Add(new SneakStrip(
              VariableControlService.StripSixWarmColor, VariableControlService.StripSixStripDefaultColor,
              VariableControlService.StripTwoStartIndex, VariableControlService.StripTwoEndIndex,
              RGBButtonPixel2, 5, 5, VariableControlService.DefaultWarmLength));


            RGBWS2811.Init();
            LevelTime.Start();
            AudioPlayer.Init(Room.Catchy);
            RGBWS2811.SetColorByRange(
                VariableControlService.StripSevenStartIndex, VariableControlService.StripSevenEndIndex,
                VariableControlService.StripSevenDefaultColor);
            RGBWS2811.Commit();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlRGBLight(_cts.Token));
            Task.Run(() => ControlRGBButton(_cts2.Token));
            Task.Run(() => ControlGameTiming(_cts3.Token));
            return Task.CompletedTask;
        }

        private async Task ControlRGBLight(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus == GameStatus.Started)
                {
                    _logger.LogTrace("Start Game ,Number of players {0}", VariableControlService.Team.player.Count());
                    StartBackgroundAudio();
                    Restart();
                    UpdateStripState();
                    while (VariableControlService.GameStatus == GameStatus.Started)
                    {
                        //ResetAllLine();
                        _logger.LogTrace("New Round :- {0}", VariableControlService.GameRound.ToString());
                        LevelTime.Restart();
                        while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                        {
                            UpdateSneakSize();
                            foreach (var strip in StripList)
                            {
                                if (LevelTime.ElapsedMilliseconds < 20)
                                    strip.Move();
                                else if (LevelTime.ElapsedMilliseconds > 20 || LevelTime.ElapsedMilliseconds < 40)
                                    strip.MoveTwoPixel();
                                else
                                    strip.MoveThreePixel();
                            }
                            RGBWS2811.Commit();
                        }
                        _logger.LogTrace("Round {0} Finshed", VariableControlService.GameRound);
                        if (VariableControlService.GameRound == Round.Round5)
                            VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
                        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                    }
                    StopBackGroundAudio();
                    _logger.LogTrace("Game Finished ..");
                }
            }
        }
        private async Task ControlRGBButton(CancellationToken cancellationToken)
        {
            // TO DO Work when game Started only
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus == GameStatus.Started || true)
                {

                    byte buttonIndex = 0;
                    try
                    {

                        foreach (var button in RGBButtonList)
                        {
                            if (!button.CurrentStatusWithCheckForDelay())
                            {
                                Console.WriteLine($"Button Pressed index {buttonIndex} , Activated By Warm in strip index {button.AssignedFor()} , For Player {ButtonNumberToPlayerIndex(buttonIndex)}");
                                button.BlockForATimeInMs(200);
                                if (button.isSet())
                                {
                                    Console.WriteLine("RGB Button Pressed for set Button");
                                    AddPoint(ButtonNumberToPlayerIndex(buttonIndex), button.AssignedFor(), buttonIndex > 7);
                                    button.BlockForATimeInMs(200);
                                    button.clickedForOnce = true;
                                    button.Set(false);
                                }
                                else
                                {

                                    Console.WriteLine($"Subtract ----***// buttonIndex {buttonIndex}");
                                    SubstractPoint(ButtonNumberToPlayerIndex(buttonIndex), button.AssignedFor());
                                }
                            }
                            buttonIndex++;
                        }
                        //Thread.Sleep(2000);
                        Thread.Sleep(50);

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Exception {ex.Message}");
                    }


                }

            }
        }

        private async Task ControlGameTiming(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                VariableControlService.CurrentTime = (int)GameTiming.ElapsedMilliseconds;
                if (VariableControlService.GameStatus == GameStatus.Started)
                {

                    bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.GameTiming;
                    bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.GameStatus == GameStatus.Started && VariableControlService.IsGameTimerStarted;
                    if (GameFinishedByTimer)
                        StopTheGame();
                }
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        public void Dispose()
        {
        }

        private void Restart()
        {
            _logger.LogTrace("Restart The Game , and prepere it for the new player");
            VariableControlService.GameRound = Round.Round1;
            VariableControlService.PlayerScore = 0;
            GameTiming.Restart();

        }
        private void StopTheGame()
        {

        }

        private Round NextRound(Round currentRound)
        {
            if (currentRound == Round.Round5)
                return Round.Round5;
            return (Round)((int)currentRound + 1);
        }
        private int ButtonNumberToPlayerIndex(int buttonIndex)
        {
            bool playerOneButton = buttonIndex == 0 || buttonIndex == 1 || buttonIndex == 8;
            bool playerTwoButton = buttonIndex == 2 || buttonIndex == 3 || buttonIndex == 10;
            bool playerThreeButton = buttonIndex == 4 || buttonIndex == 5 || buttonIndex == 11;
            bool playerFourButton = buttonIndex == 6 || buttonIndex == 7 || buttonIndex == 13;
            if (playerOneButton)
                return 0;
            else if (playerTwoButton)
                return 1;
            else if (playerThreeButton)
                return 2;
            else if (playerFourButton)
                return 3;
            return 0;
        }





        public void UpdateSneakSize()
        {
            StripList[0].UpdateLength(VariableControlService.PlayerOneWarmLength);
            StripList[1].UpdateLength(VariableControlService.PlayerTwoWarmLength);
            StripList[2].UpdateLength(VariableControlService.PlayerThreeWarmLength);
            StripList[3].UpdateLength(VariableControlService.PlayerFourWarmLength);
        }

        public void UpdateStripState()
        {
            if (VariableControlService.Team.player.Count() <= 2)
            {
                StripList[0].Activate(true);
                StripList[1].Activate(true);
                StripList[2].Activate(false);
                StripList[3].Activate(false);
                StripList[4].Activate(true);
                StripList[5].Activate(true);
            }
            else if (VariableControlService.Team.player.Count() <= 3)
            {
                StripList[0].Activate(true);
                StripList[1].Activate(true);
                StripList[2].Activate(true);
                StripList[3].Activate(false);
                StripList[4].Activate(true);
                StripList[5].Activate(true);
            }
            else if (VariableControlService.Team.player.Count() <= 4)
            {
                StripList[0].Activate(true);
                StripList[1].Activate(true);
                StripList[2].Activate(true);
                StripList[3].Activate(true);
                StripList[4].Activate(true);
                StripList[5].Activate(true);
            }
        }




        private void ResetLine(int startLed, int endLed, RGBColor offColor)
        {
            RGBWS2811.SetColorByRange(startLed, endLed, offColor);
        }

        private void ResetAllLine()
        {
            _logger.LogTrace("Restart All Lines ...");
            foreach (var strip in StripList)
            {
                ResetLine(strip.startRGBLed, strip.endRGBLed, strip.rgbOffColor);
                strip.LineReset();
            }
            RGBWS2811.Commit();
            foreach (var button in RGBButtonList)
            {
                button.Set(false);
                button.clickedForOnce = false;
            }
            _logger.LogTrace("Done .");
        }




        #region Score System
        private void AddPoint(int playerIndex, int buttonAssignedFor, bool isDubleScore)
        {
            if (playerIndex > VariableControlService.Team.player.Count() - 1)
                return;
            if (isDubleScore)
                VariableControlService.Team.player[playerIndex].score += 2;
            else
                VariableControlService.Team.player[playerIndex].score += 1;
            if (playerIndex == buttonAssignedFor)
                ChangeSneakSize(playerIndex, 1);
            else
            {
                ChangeSneakSize(playerIndex, 1);
                ChangeSneakSize(buttonAssignedFor, -1);
            }
            _logger.LogTrace($"Add Point To {playerIndex} Total {VariableControlService.Team.player[playerIndex].score}");
            AudioPlayer.PIStartAudio(SoundType.Success);
        }
        private void SubstractPoint(int playerIndex, int buttonAssignedFor)
        {
            _logger.LogTrace($"Substract Point playerIndex {playerIndex}");
            //  AudioPlayer.PIStartAudio(SoundType.Failure);
            VariableControlService.Team.player[playerIndex].score -= 1;
            _logger.LogTrace($"Substract Point To {playerIndex} Total {VariableControlService.Team.player[playerIndex].score}");
            ChangeSneakSize(playerIndex, -1);
        }
        public void ChangeSneakSize(int playerIndex, int addedValue)
        {

            switch (playerIndex)
            {
                case 0:
                    VariableControlService.PlayerOneWarmLength += addedValue;
                    _logger.LogTrace("new Worm Size {1}", VariableControlService.PlayerOneWarmLength);
                    break;
                case 1:
                    VariableControlService.PlayerTwoWarmLength += addedValue;
                    _logger.LogTrace("new Worm Size {1}", VariableControlService.PlayerTwoWarmLength);
                    break;
                case 2:
                    VariableControlService.PlayerThreeWarmLength += addedValue;
                    _logger.LogTrace("new Worm Size {1}", VariableControlService.PlayerThreeWarmLength);
                    break;
                case 3:
                    VariableControlService.PlayerFourWarmLength += addedValue;
                    _logger.LogTrace("new Worm Size {1}", VariableControlService.PlayerFourWarmLength);
                    break;
                default:
                    break;
            }
        }

        #endregion

        #region Control Audio 
        private void StartBackgroundAudio()
        {
            if (!backgroundSoundStarted)
            {
                _logger.LogTrace("Play Background Sound");
                backgroundSoundStarted = true;
                AudioPlayer.PIBackgroundSound(SoundType.Background);
            }
        }
        private void StopBackGroundAudio()
        {
            if (backgroundSoundStarted)
            {
                backgroundSoundStarted = false;
                AudioPlayer.PIStopAudio();
            }
        }


        #endregion





    }

}
