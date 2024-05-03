using Library.GPIOLib;
using Library;
using Library.RGBLib;
using Library.Model;
using Library.PinMapping;
using System.Diagnostics;
using System.IO;
using Library.Media;

namespace CatchyGame.Service
{
    public class SenarioService : IHostedService, IDisposable
    {


        List<Strip> StripList = new List<Strip>();
        List<RGBButton> RGBButtonList = new List<RGBButton>();

        private CancellationTokenSource _cts, _cts2, _cts3;
        private int WormLengthInTheLevel = 0;
        Stopwatch GameTiming = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        Random random = new Random();
        bool backgroundSoundStarted = false;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);

            // RGB Pixel Button 
            var rgb1 = new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1);
            var rgb2 = new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2);
            var rgb3 = new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3);
            var rgb4 = new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4);
            var rgb5 = new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5);
            var rgb6 = new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6);
            var rgb7 = new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7);
            var rgb8 = new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8);


            var startRGBButton1 = new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13);
            var startRGBButton2 = new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14);
            var startRGBButton3 = new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15);
            var startRGBButton4 = new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9);
            var startRGBButton5 = new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10);
            var startRGBButton6 = new RGBButton(RGBButtonPin.RGBR12, RGBButtonPin.RGBG12, RGBButtonPin.RGBB12, RGBButtonPin.RGBPB12);





            RGBButtonList.Add(rgb1);
            RGBButtonList.Add(rgb2);
            RGBButtonList.Add(rgb3);
            RGBButtonList.Add(rgb4);
            RGBButtonList.Add(rgb5);
            RGBButtonList.Add(rgb6);
            RGBButtonList.Add(rgb7);
            RGBButtonList.Add(rgb8);


            RGBButtonPixel StripOneButton0 = new RGBButtonPixel(0, startRGBButton1);
            RGBButtonPixel StripOneButton1 = new RGBButtonPixel(19, rgb1);
            RGBButtonPixel StripOneButton2 = new RGBButtonPixel(60, rgb4);
            RGBButtonPixel StripOneButton3 = new RGBButtonPixel(82, rgb5);
            RGBButtonPixel StripOneButton4 = new RGBButtonPixel(123, rgb8);

            RGBButtonPixel StripTwoButton0 = new RGBButtonPixel(213, startRGBButton2);
            RGBButtonPixel StripTwoButton1 = new RGBButtonPixel(242, rgb1);
            RGBButtonPixel StripTwoButton2 = new RGBButtonPixel(280, rgb3);
            RGBButtonPixel StripTwoButton3 = new RGBButtonPixel(320, rgb6);
            RGBButtonPixel StripTwoButton4 = new RGBButtonPixel(355, rgb7);

            RGBButtonPixel StripThreeButton0 = new RGBButtonPixel(440, startRGBButton3);
            RGBButtonPixel StripThreeButton1 = new RGBButtonPixel(475, rgb2);
            RGBButtonPixel StripThreeButton2 = new RGBButtonPixel(520, rgb4);
            RGBButtonPixel StripThreeButton3 = new RGBButtonPixel(535, rgb5);
            RGBButtonPixel StripThreeButton4 = new RGBButtonPixel(591, rgb8);

            RGBButtonPixel StripFourButton0 = new RGBButtonPixel(666, startRGBButton4);
            RGBButtonPixel StripFourButton2 = new RGBButtonPixel(748, rgb1);
            RGBButtonPixel StripFourButton1 = new RGBButtonPixel(784, rgb3);
            RGBButtonPixel StripFourButton3 = new RGBButtonPixel(824, rgb6);
            RGBButtonPixel StripFourButton4 = new RGBButtonPixel(841, rgb7);


            RGBButtonPixel StripFiveButton0 = new RGBButtonPixel(882, startRGBButton5);
            RGBButtonPixel StripFiveButton1 = new RGBButtonPixel(945, rgb2);
            RGBButtonPixel StripFiveButton3 = new RGBButtonPixel(976, rgb4);
            RGBButtonPixel StripFiveButton2 = new RGBButtonPixel(998, rgb5);
            RGBButtonPixel StripFiveButton4 = new RGBButtonPixel(1043, rgb8);

            RGBButtonPixel StripSixButton0 = new RGBButtonPixel(1076, startRGBButton6);
            RGBButtonPixel StripSixButton1 = new RGBButtonPixel(1094, rgb7);
            RGBButtonPixel StripSixButton3 = new RGBButtonPixel(1107, rgb6);
            RGBButtonPixel StripSixButton2 = new RGBButtonPixel(1137, rgb3);
            RGBButtonPixel StripSixButton4 = new RGBButtonPixel(1153, rgb2);




            // Random 5 Work For Each Line
            List<RGBButtonPixel> RGBButtonPixel1 = new List<RGBButtonPixel>();
            List<RGBButtonPixel> RGBButtonPixel2 = new List<RGBButtonPixel>();
            List<RGBButtonPixel> RGBButtonPixel3 = new List<RGBButtonPixel>();
            List<RGBButtonPixel> RGBButtonPixel4 = new List<RGBButtonPixel>();
            List<RGBButtonPixel> RGBButtonPixel5 = new List<RGBButtonPixel>();
            List<RGBButtonPixel> RGBButtonPixel6 = new List<RGBButtonPixel>();

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



            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 0, 212, RGBButtonPixel1, 0));
            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 213, 439, RGBButtonPixel2, 1));
            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 440, 665, RGBButtonPixel3, 2));
            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 666, 881, RGBButtonPixel4, 3));
            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 882, 1075, RGBButtonPixel5, 4));
            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 1076, 1236, RGBButtonPixel6, 5));


            RGBWS2811.Init();
            LevelTime.Start();
            AudioPlayer.Init(Room.Catchy);



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

            Restart();
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus == GameStatus.Started)
                {
                    Console.WriteLine("Start Game ...");
                    if (!backgroundSoundStarted)
                        AudioPlayer.PIBackgroundSound(SoundType.Background);
                    Restart();
                    while (VariableControlService.GameStatus == GameStatus.Started)
                    {
                        ResetAllLine();
                        WormLengthInTheLevel = WormLengthInLevel(VariableControlService.GameRound);
                        foreach (var strip in StripList)
                        {
                            strip.UpdateLength(WormLengthInTheLevel);
                        }

                        Console.WriteLine(VariableControlService.GameRound.ToString());
                        Console.WriteLine($"WormLengthInTheLevel {WormLengthInTheLevel}");
                        // Start The Timer 
                        LevelTime.Restart();
                        while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                        {
                            int delayMs = 70 - (int)(LevelTime.ElapsedMilliseconds / 1000);
                            if (delayMs < 0) { delayMs = 1; }

                            foreach (var strip in StripList)
                            {
                                if (LevelTime.ElapsedMilliseconds < 20)
                                {
                                    strip.Move();
                                }
                                else if (LevelTime.ElapsedMilliseconds > 20 || LevelTime.ElapsedMilliseconds < 40)
                                {
                                    strip.Move();
                                    strip.Move();
                                }
                                else
                                {

                                    strip.Move();
                                    strip.Move();
                                    strip.Move();
                                }
                            }

                            RGBWS2811.Commit();
                        }
                        if (VariableControlService.GameRound == Round.Round5)
                            VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
                        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                    }
                    if (backgroundSoundStarted)
                        AudioPlayer.PIStopAudio();
                }
            }
        }
        private async Task ControlRGBButton(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                byte buttonIndex = 0;
                try
                {

                    foreach (var button in RGBButtonList)
                    {
                        if (!button.CurrentStatusWithCheckForDelay())
                        {
                            button.BlockForATimeInMs(200);
                            if (button.isSet())
                            {
                                Console.WriteLine("Is Set");
                                AddPoint(ButtonNumberToPlayerIndex(buttonIndex));
                                button.BlockForATimeInMs(200);
                                button.clickedForOnce = true;
                                button.Set(false);
                            }
                            else
                                SubstractPoint(ButtonNumberToPlayerIndex(buttonIndex));
                        }
                        buttonIndex++;
                    }
                    Thread.Sleep(50);

                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception {ex.Message}");
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
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        private void Restart()
        {
            VariableControlService.GameRound = Round.Round1;
            VariableControlService.PlayerScore = 0;
            GameTiming.Restart();

        }
        private void StopTheGame()
        {

        }












        // ========== Number Of Straps
        private int NumberOfStrapsInLevel(Round currentRound)
        {
            if (currentRound == Round.Round1) return 2;
            else if (currentRound == Round.Round2) return 3;
            else if (currentRound == Round.Round3) return 4;
            else if (currentRound == Round.Round4) return 5;
            else if (currentRound == Round.Round5) return 6;
            return 0;
        }


        private int WormLengthInLevel(Round currentRound)
        {
            if (currentRound == Round.Round1) return 5;
            else if (currentRound == Round.Round2) return 4;
            else if (currentRound == Round.Round3) return 3;
            else if (currentRound == Round.Round4) return 2;
            else if (currentRound == Round.Round5) return 1;
            return 5;
        }

        // ======== Private To The Next Room
        private Round NextRound(Round currentRound)
        {
            if (currentRound == Round.Round5)
                return Round.Round5;
            return (Round)((int)currentRound + 1);
        }
        private int ButtonNumberToPlayerIndex(int buttonIndex)
        {
            if (buttonIndex == 0 || buttonIndex == 1)
                return 0;
            else if (buttonIndex == 2 || buttonIndex == 3)
                return 1;
            else if (buttonIndex == 4 || buttonIndex == 5)
                return 2;
            else if (buttonIndex == 6 || buttonIndex == 7)
                return 3;
            return 0;
        }


        private void AddPoint(int playerIndex)
        {
            if (playerIndex > VariableControlService.Team.player.Count() - 1)
                return;

            VariableControlService.Team.player[playerIndex].score += 1;
            AudioPlayer.PIStartAudio(SoundType.Success);
            Console.WriteLine($"Add Point To {playerIndex} Total {VariableControlService.Team.player[playerIndex].score}");
        }
        private void SubstractPoint(int playerIndex)
        {
            Console.WriteLine("Substract Point");
            AudioPlayer.PIStartAudio(SoundType.Failure);
        }

        private void ResetLine(int startLed, int endLed)
        {
            for (int i = startLed; i <= endLed; i++)
            {
                RGBWS2811.SetColor(true, i, RGBColor.Off);
            }
        }

        private void ResetAllLine()
        {

            Console.Write("Restart All Lines ...");
            foreach (var strip in StripList)
            {
                ResetLine(strip.startRGBLed, strip.endRGBLed);
                strip.LineReseted();
            }
            RGBWS2811.Commit();
            Console.WriteLine(" Done .");

            foreach (var button in RGBButtonList)
            {
                button.Set(false);
                button.clickedForOnce = false;
            }
        }






    }
}