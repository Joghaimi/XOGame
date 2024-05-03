﻿using Library.GPIOLib;
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
        private int NumberOfStripToStart = 0;
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



            StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 0, 212, StripOneButton0, StripTwoButton1, StripTwoButton2, StripTwoButton3, StripTwoButton4));
            //StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 213, 439, StripTwoButton0, StripTwoButton1, StripTwoButton2, StripTwoButton3, StripTwoButton4));
            //StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 440, 665, StripThreeButton0, StripThreeButton1, StripThreeButton2, StripThreeButton3, StripThreeButton4));
            //StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 666, 881, StripFourButton0, StripFourButton1, StripFourButton2, StripFourButton3, StripFourButton4));
            //StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 882, 1075, StripFiveButton0, StripFiveButton1, StripFiveButton2, StripFiveButton3, StripFiveButton4));
            //StripList.Add(new Strip(RGBColor.Red, RGBColor.Off, 1076, 1236, StripSixButton0, StripSixButton1, StripSixButton2, StripSixButton3, StripSixButton4));

            // Init the RGBStrip 
            RGBWS2811.Init();
            // Init RGB Button 
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
            //RGBWS2811.SetColor(this.isActive, worm1.startPixel, this.rgbColor);
            //int startPixel = 0;
            //int endPixel = -5;
            Restart();
            while (!cancellationToken.IsCancellationRequested)
            {

                //RGBWS2811.SetColor(startPixel, RGBColor.Red);
                //startPixel++;
                //if (endPixel >= 0)
                //    RGBWS2811.SetColor(endPixel, RGBColor.Off);
                //endPixel++;

                //StripList[0].Move();
                //RGBWS2811.Commit();
                //Thread.Sleep(100);
                foreach (var strip in StripList)
                {

                    strip.Move();
                    strip.Move();
                }
                //StripList[0].Move();
                //StripList[0].Move();
                //RGBWS2811.Commit();
                RGBWS2811.Commit();
                Thread.Sleep(100);











                //if (VariableControlService.GameStatus == GameStatus.Started)
                //{
                //    Console.WriteLine("Start Game ...");
                //    if (!backgroundSoundStarted)
                //        AudioPlayer.PIBackgroundSound(SoundType.Background);
                //    // Restart The Game Parameter 
                //    Restart();
                //    while (VariableControlService.GameStatus == GameStatus.Started)
                //    {
                //        ResetAllLine();
                //        UnSelectAllStrap();
                //        NumberOfStripToStart = NumberOfStrapsInLevel(VariableControlService.GameRound);
                //        RandomSelectStrip(NumberOfStripToStart);
                //        Console.WriteLine(VariableControlService.GameRound.ToString());
                //        Console.WriteLine($"Number Of Selected Strip {NumberOfStripToStart}");
                //        // Start The Timer 
                //        LevelTime.Restart();
                //        while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                //        {
                //            int delayMs = 70 - (int)(LevelTime.ElapsedMilliseconds / 1000);
                //            if (delayMs < 0) { delayMs = 1; }

                //            foreach (var strip in StripList)
                //            {
                //                bool nextOneIsTargetButton =
                //                    strip.rGBButton1.Pixel == strip.currentLed ||
                //                    strip.rGBButton2.Pixel == strip.currentLed ||
                //                    strip.rGBButton3.Pixel == strip.currentLed ||
                //                    strip.rGBButton4.Pixel == strip.currentLed;
                //                if (nextOneIsTargetButton || LevelTime.ElapsedMilliseconds < 20)
                //                {
                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                }
                //                else if (LevelTime.ElapsedMilliseconds > 20 || LevelTime.ElapsedMilliseconds < 40)
                //                {
                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                }
                //                else
                //                {

                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                    //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                //                    strip.NextLed();
                //                }
                //                if (strip.isActive && strip.resetLine)
                //                {
                //                    ResetLine(strip.startRGBLed, strip.endRGBLed);
                //                    strip.LineReseted();

                //                    // Random Select New Strips
                //                    strip.isActive = false;
                //                    RandomSelectStrip(1);
                //                }

                //            }
                //            RGBWS2811.Commit();
                //        }
                //        if (VariableControlService.GameRound == Round.Round5)
                //            VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
                //        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                //    }
                //    if (backgroundSoundStarted)
                //        AudioPlayer.PIStopAudio();
                //}




            }
        }
        // ===== 
        private async Task ControlRGBButton(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                byte buttonIndex = 0;
                foreach (var button in RGBButtonList)
                {
                    if (!button.CurrentStatusWithCheckForDelay())
                    {
                        button.BlockForATimeInMs(200);
                        if (button.isSet())
                        {
                            AddPoint(ButtonNumberToPlayerIndex(buttonIndex));
                            button.BlockForATimeInMs(200);
                        }
                        else
                            SubstractPoint(ButtonNumberToPlayerIndex(buttonIndex));
                    }
                    buttonIndex++;
                }
                Thread.Sleep(50);
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

        // ======== Private To The Next Room
        private Round NextRound(Round currentRound)
        {
            if (currentRound == Round.Round5)
                return Round.Round5;
            return (Round)((int)currentRound + 1);
        }
        private void RandomSelectStrip(int numberOfStripToSelect)
        {
            int selected = 0;
            while (numberOfStripToSelect > selected)
            {
                var selectedIndex = random.Next(0, StripList.Count);
                if (!StripList[selectedIndex].isActive)
                {
                    StripList[selectedIndex].isActive = true;
                    selected++;
                    Console.WriteLine($"Selected Strip #{selectedIndex + 1}");
                }
            }
        }
        private void UnSelectAllStrap()
        {
            Console.Write("UnSelectAllStrap ..");

            foreach (var item in StripList)
                item.isActive = false;
            Console.WriteLine(" Done .");
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
                RGBWS2811.SetColor(true, i, RGBColor.Blue);
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
        }





    }
}