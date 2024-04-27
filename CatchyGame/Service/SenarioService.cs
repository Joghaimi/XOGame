using Library.GPIOLib;
using Library;
using Library.RGBLib;
using Library.Model;
using Library.PinMapping;
using System.Diagnostics;
using System.IO;

namespace CatchyGame.Service
{
    public class SenarioService : IHostedService, IDisposable
    {


        List<Strip> StripList = new List<Strip>();
        List<RGBButton> RGBButtonList = new List<RGBButton>();

        Strip strip1, strip2, strip3, strip4;
        //RGBButton rgb1;
        private CancellationTokenSource _cts, _cts2;
        private Level CurrentLevel = Level.Level1;
        private int NumberOfStripToStart = 0;
        Stopwatch GameTiming = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        Random random = new Random();

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

            RGBButtonList.Add(rgb1);
            RGBButtonList.Add(rgb2);
            RGBButtonList.Add(rgb3);
            RGBButtonList.Add(rgb4);
            RGBButtonList.Add(rgb5);
            RGBButtonList.Add(rgb6);
            RGBButtonList.Add(rgb7);
            RGBButtonList.Add(rgb8);

            //while (true)
            //{
            //    if(!rgb1.CurrentStatus())
            //        Console.WriteLine("Button #1");
            //    if (!rgb2.CurrentStatus())
            //        Console.WriteLine("Button #2");
            //    if (!rgb3.CurrentStatus())
            //        Console.WriteLine("Button #3");
            //    if (!rgb4.CurrentStatus())
            //        Console.WriteLine("Button #4");
            //    if (!rgb5.CurrentStatus())
            //        Console.WriteLine("Button #5");
            //    if (!rgb6.CurrentStatus())
            //        Console.WriteLine("Button #6");
            //    if (!rgb7.CurrentStatus())
            //        Console.WriteLine("Button #7");
            //    if (!rgb8.CurrentStatus())
            //        Console.WriteLine("Button #8");
            //Console.WriteLine("Red");
            //rgb1.TurnColorOn(RGBColor.Red);
            //rgb2.TurnColorOn(RGBColor.Red);
            //rgb3.TurnColorOn(RGBColor.Red);
            //rgb4.TurnColorOn(RGBColor.Red);
            //rgb5.TurnColorOn(RGBColor.Red);
            //rgb6.TurnColorOn(RGBColor.Red);
            //rgb7.TurnColorOn(RGBColor.Red);
            //rgb8.TurnColorOn(RGBColor.Red);
            //Thread.Sleep(2000);

            //Console.WriteLine("Green");
            //rgb1.TurnColorOn(RGBColor.Green);
            //rgb2.TurnColorOn(RGBColor.Green);
            //rgb3.TurnColorOn(RGBColor.Green);
            //rgb4.TurnColorOn(RGBColor.Green);
            //rgb5.TurnColorOn(RGBColor.Green);
            //rgb6.TurnColorOn(RGBColor.Green);
            //rgb7.TurnColorOn(RGBColor.Green);
            //rgb8.TurnColorOn(RGBColor.Green);
            //Thread.Sleep(2000);

            //Console.WriteLine("Blue");
            //rgb1.TurnColorOn(RGBColor.Blue);
            //rgb2.TurnColorOn(RGBColor.Blue);
            //rgb3.TurnColorOn(RGBColor.Blue);
            //rgb4.TurnColorOn(RGBColor.Blue);
            //rgb5.TurnColorOn(RGBColor.Blue);
            //rgb6.TurnColorOn(RGBColor.Blue);
            //rgb7.TurnColorOn(RGBColor.Blue);
            //rgb8.TurnColorOn(RGBColor.Blue);
            //Thread.Sleep(2000);


            //}
            RGBButtonPixel StripOneButton1 = new RGBButtonPixel(20, rgb1);
            RGBButtonPixel StripOneButton2 = new RGBButtonPixel(60, rgb4);
            RGBButtonPixel StripOneButton3 = new RGBButtonPixel(82, rgb5);
            RGBButtonPixel StripOneButton4 = new RGBButtonPixel(123, rgb8);

            RGBButtonPixel StripTwoButton1 = new RGBButtonPixel(242, rgb1);
            RGBButtonPixel StripTwoButton2 = new RGBButtonPixel(279, rgb3);
            RGBButtonPixel StripTwoButton3 = new RGBButtonPixel(319, rgb6);
            RGBButtonPixel StripTwoButton4 = new RGBButtonPixel(354, rgb7);

            RGBButtonPixel StripThreeButton1 = new RGBButtonPixel(474, rgb2);
            RGBButtonPixel StripThreeButton2 = new RGBButtonPixel(519, rgb4);
            RGBButtonPixel StripThreeButton3 = new RGBButtonPixel(534, rgb5);
            RGBButtonPixel StripThreeButton4 = new RGBButtonPixel(590, rgb8);


            RGBButtonPixel StripFourButton2 = new RGBButtonPixel(747, rgb1);
            RGBButtonPixel StripFourButton1 = new RGBButtonPixel(783, rgb3);
            RGBButtonPixel StripFourButton3 = new RGBButtonPixel(823, rgb6);
            RGBButtonPixel StripFourButton4 = new RGBButtonPixel(840, rgb7);

            RGBButtonPixel StripFiveButton1 = new RGBButtonPixel(945, rgb2);
            RGBButtonPixel StripFiveButton3 = new RGBButtonPixel(975, rgb4);
            RGBButtonPixel StripFiveButton2 = new RGBButtonPixel(996, rgb5);
            RGBButtonPixel StripFiveButton4 = new RGBButtonPixel(1041, rgb8);


            RGBButtonPixel StripSixButton1 = new RGBButtonPixel(1092, rgb7);
            RGBButtonPixel StripSixButton3 = new RGBButtonPixel(1106, rgb6);
            RGBButtonPixel StripSixButton2 = new RGBButtonPixel(1136, rgb3);
            RGBButtonPixel StripSixButton4 = new RGBButtonPixel(1152, rgb2);

            //var rgb2 = new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4);
            //var rgb3 = new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5);
            //var rgb4 = new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8);
            //while (true)
            //{
            //    Console.WriteLine("RED");
            //    rgb1.TurnColorOn(RGBColor.Red);
            //    rgb2.TurnColorOn(RGBColor.Red);
            //    rgb3.TurnColorOn(RGBColor.Red);
            //    rgb4.TurnColorOn(RGBColor.Red);
            //    Thread.Sleep(2000);
            //    Console.WriteLine("Green");

            //    rgb1.TurnColorOn(RGBColor.Green);
            //    rgb2.TurnColorOn(RGBColor.Green);
            //    rgb4.TurnColorOn(RGBColor.Green);
            //    rgb3.TurnColorOn(RGBColor.Green);
            //    Thread.Sleep(2000);
            //    Console.WriteLine("Blue");

            //    rgb1.TurnColorOn(RGBColor.Blue);
            //    rgb2.TurnColorOn(RGBColor.Blue);
            //    rgb4.TurnColorOn(RGBColor.Blue);
            //    rgb3.TurnColorOn(RGBColor.Blue);
            //    Thread.Sleep(2000);

            //}


            //Init Strip
            StripList.Add(new Strip(RGBColor.purple, 0, 212, StripOneButton1, StripOneButton2, StripOneButton3, StripOneButton4));
            StripList.Add(new Strip(RGBColor.purple, 213, 438, StripTwoButton1, StripTwoButton2, StripTwoButton3, StripTwoButton4));
            StripList.Add(new Strip(RGBColor.purple, 439, 664, StripThreeButton1, StripThreeButton2, StripThreeButton3, StripThreeButton4));
            StripList.Add(new Strip(RGBColor.purple, 665, 880, StripFourButton1, StripFourButton2, StripFourButton3, StripFourButton4));
            StripList.Add(new Strip(RGBColor.purple, 881, 1073, StripFiveButton1, StripFiveButton2, StripFiveButton3, StripFiveButton4));
            StripList.Add(new Strip(RGBColor.purple, 1074, 1236, StripSixButton1, StripSixButton2, StripSixButton3, StripSixButton4));


            // Init the RGBStrip 
            RGBWS2811.Init();
            // Init RGB Button 
            LevelTime.Start();



            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlRGBLight(_cts.Token));
            Task.Run(() => ControlRGBButton(_cts2.Token));
            return Task.CompletedTask;
        }

        private async Task ControlRGBLight(CancellationToken cancellationToken)
        {

            while (!cancellationToken.IsCancellationRequested)
            {



                ResetAllLine();
                UnSelectAllStrap();
                NumberOfStripToStart = NumberOfStrapsInLevel(CurrentLevel);
                RandomSelectStrip(NumberOfStripToStart);
                Console.WriteLine(CurrentLevel.ToString());
                Console.WriteLine($"Number Of Selected Strip {NumberOfStripToStart}");


                // Start The Timer 
                LevelTime.Restart();
                while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                {
                    int delayMs = 70 - (int)(LevelTime.ElapsedMilliseconds / 1000);
                    if (delayMs < 0) { delayMs = 1; }

                    foreach (var strip in StripList)
                    {
                        bool nextOneIsTargetButton =
                            strip.rGBButton1.Pixel == strip.currentLed ||
                            strip.rGBButton2.Pixel == strip.currentLed ||
                            strip.rGBButton3.Pixel == strip.currentLed ||
                            strip.rGBButton4.Pixel == strip.currentLed;
                        if (nextOneIsTargetButton)
                        {
                            RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                            strip.NextLed();
                        }
                        else
                        {
                            RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                            strip.NextLed();
                            RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                            strip.NextLed();
                            //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                            //strip.NextLed();
                        }
                        //RGBWS2811.SetColor(strip.isActive, strip.currentLed, strip.rgbColor);
                        //strip.NextLed();
                        if (strip.isActive && strip.resetLine)
                        {
                            ResetLine(strip.startRGBLed, strip.endRGBLed);
                            strip.LineReseted();
                        }

                    }
                    RGBWS2811.Commit();
                    //Thread.Sleep(delayMs);
                }
                CurrentLevel = NextLevel(CurrentLevel);


                //RGBWS2811.SetColor(strip1.isActive, strip1.currentLed, strip1.rgbColor);
                //RGBWS2811.Commit();
                //strip1.NextLed();
                //if (strip1.rGBButton1.Pixel == strip1.currentLed)
                //{
                //    strip1.rGBButton1.Button.TurnColorOn(RGBColor.Red);
                //}
                //else
                //{
                //    rgb1.TurnColorOn(RGBColor.Blue);
                //}
                //rgb1
                Thread.Sleep(1000);
                Console.Write(".");
            }
        }
        // ===== 
        private async Task ControlRGBButton(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                foreach (var button in RGBButtonList)
                {
                    if (!button.CurrentStatusWithCheckForDelay())
                    {
                        button.BlockForATimeInMs(200);
                        if (button.isSet())
                        {
                            AddPoint();
                            button.BlockForATimeInMs(200);
                        }
                        else
                            SubstractPoint();
                    }
                }
                Thread.Sleep(50);
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
            CurrentLevel = Level.Level1;
        }












        // ========== Number Of Straps
        private int NumberOfStrapsInLevel(Level currentLevel)
        {
            if (currentLevel == Level.Level1) return 2;
            else if (currentLevel == Level.Level2) return 3;
            else if (currentLevel == Level.Level3) return 4;
            else if (currentLevel == Level.Level4) return 5;
            else if (currentLevel == Level.Level5) return 6;
            return 0;
        }

        // ======== Private To The Next Room
        private Level NextLevel(Level currentLevel)
        {
            if (currentLevel == Level.Level5)
                return Level.Finished;
            return (Level)((int)currentLevel + 1);
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
            foreach (var item in StripList)
                item.isActive = false;
        }
        private void AddPoint()
        {
            Console.WriteLine("Add Point");

        }
        private void SubstractPoint()
        {
            Console.WriteLine("Substract Point");
        }

        private void ResetLine(int startLed, int endLed)
        {
            for (int i = startLed; i <= endLed; i++)
            {
                RGBWS2811.SetColor(true, i, RGBColor.Yellow);
            }
        }

        private void ResetAllLine()
        {

            foreach (var strip in StripList)
            {
                ResetLine(strip.startRGBLed, strip.endRGBLed);
                strip.LineReseted();
            }
            RGBWS2811.Commit();
        }





    }
}
