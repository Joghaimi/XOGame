using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.PinMapping;

namespace FortRoom.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        int SlowPeriod = 10000;
        int MediumPeriod = 15000;

        int slowChangeTime = 5000;
        int mediumChangeTime = 3000;
        int highChangeTime = 1000;

        int changingSpeed = 5000;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            Console.WriteLine("RGB 1");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1));
            Console.WriteLine("RGB 2");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2));
            Console.WriteLine("RGB 3");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3));
            Console.WriteLine("RGB 4");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4));
            Console.WriteLine("RGB 5");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            Console.WriteLine("RGB 6");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6));
            Console.WriteLine("RGB 7");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
            Console.WriteLine("RGB 8");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8));
            Console.WriteLine("RGB 9");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9));
            Console.WriteLine("RGB 10");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10));
            Console.WriteLine("RGB 11");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11));
            Console.WriteLine("RGB 12");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR12, RGBButtonPin.RGBG12, RGBButtonPin.RGBB12, RGBButtonPin.RGBPB12));
            Console.WriteLine("=======================");


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool activeButton = false;
            Stopwatch timer = new Stopwatch();
            Stopwatch timerToStart = new Stopwatch();
            int activeButtonIndox = -1;
            Random random = new Random();
            int randomTime = random.Next(1000, 5000);
            timerToStart.Start();
            timer.Start();
            Console.WriteLine("Started .... ");
            while (!cancellationToken.IsCancellationRequested)
            {

                if (VariableControlService.IsTheGameStarted)
                {
                    if (!activeButton && timerToStart.ElapsedMilliseconds > randomTime)
                    {

                        activeButton = true;
                        activeButtonIndox = random.Next(0, 11);
                        Console.WriteLine($"Button {activeButtonIndox} Activated");
                        RGBLight.SetColor(RGBColor.Off);
                        JQ8400AudioModule.PlayAudio((int)SoundType.Button);
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
                        timer.Restart();
                        timerToStart.Restart();
                        randomTime = random.Next(1000, 5000);
                    }
                    if (activeButton & timer.ElapsedMilliseconds >= 5000)//changingSpeed)
                    {
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                        Console.WriteLine($"Button {activeButtonIndox} Deactivated");
                        activeButtonIndox = -1;
                        activeButton = false;
                        timer.Restart();
                        timerToStart.Restart();
                    }
                    if (activeButton && activeButtonIndox > -1)
                    {
                        bool isPressed = !RGBButtonList[activeButtonIndox].CurrentStatus();
                        if (isPressed)
                        {
                            JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
                            activeButton = false;
                            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                            RGBLight.SetColor(RGBColor.Green);
                            VariableControlService.ActiveButtonPressed++;
                            Console.WriteLine($"Button {activeButtonIndox} Pressed");
                            Console.WriteLine($"Current Score {VariableControlService.ActiveButtonPressed}");
                            activeButtonIndox = -1;
                            timerToStart.Restart();
                            timer.Restart();
                        }
                    }
                }
                // Sleep for a short duration to avoid excessive checking
                Thread.Sleep(10);
            }
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
