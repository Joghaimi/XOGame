using Iot.Device.Mcp3428;
using Library;
using Library.Display;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;
using System.Diagnostics;

namespace DarkRoom.Services
{
    public class LedMatrixService : IHostedService, IDisposable
    {


        private DisplayController display;
        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        int FlashingMinumum = 10000;
        int FlashingMax = 15000;

        int slowChangeTime = 5000;
        int mediumChangeTime = 3000;
        int highChangeTime = 1000;

        int changingSpeed = 5000;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT1.Chip, MasterOutputPin.OUTPUT1.port,
                MasterOutputPin.OUTPUT1.PinNumber, PinMode.Output);


            display = new DisplayController(SerialPort.Serial);


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


                        //    activeButton = true;
                        //    activeButtonIndox = random.Next(0, 12);
                        //    Console.WriteLine($"Button {activeButtonIndox} Activated");
                        //    RGBLight.SetColor(RGBColor.Off);
                        //    JQ8400AudioModule.PlayAudio((int)SoundType.Button);
                        //    //RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
                        //    timer.Restart();
                        //    timerToStart.Restart();
                        //    randomTime = random.Next(1000, 5000);
                        //}
                        //if (activeButton & timer.ElapsedMilliseconds >= 5000)//changingSpeed)
                        //{
                        //    //RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                        //    Console.WriteLine($"Button {activeButtonIndox} Deactivated");
                        //    activeButtonIndox = -1;
                        //    activeButton = false;
                        //    timer.Restart();
                        //    timerToStart.Restart();
                        //}
                        //if (activeButton && activeButtonIndox > -1)
                        //{
                        //    bool isPressed = !RGBButtonList[activeButtonIndox].CurrentStatus();
                        //    if (isPressed)
                        //    {
                        //        JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
                        //        activeButton = false;
                        //        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                        //        RGBLight.SetColor(RGBColor.Green);
                        //        VariableControlService.ActiveButtonPressed++;
                        //        Console.WriteLine($"Button {activeButtonIndox} Pressed");
                        //        Console.WriteLine($"Current Score {VariableControlService.ActiveButtonPressed}");
                        //        activeButtonIndox = -1;
                        //        timerToStart.Restart();
                        //        timer.Restart();
                        //    }
                        //}
                    }
                    // Sleep for a short duration to avoid excessive checking
                }
                Thread.Sleep(10);
            }
        }
        private async Task TimingService(CancellationToken cancellationToken)
        {
            GameStopWatch.Start();
            bool IsTimerSet = false;
            int timePeriod = 0;
            Random random = new Random();

            while (true)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    if (!IsTimerSet)
                    {
                        timePeriod = random.Next(FlashingMinumum, FlashingMax);
                        IsTimerSet = true;
                        GameStopWatch.Restart();

                    }

                    if (IsTimerSet && GameStopWatch.ElapsedMilliseconds > timePeriod)
                    {
                        IsTimerSet = false;
                        MCP23Controller.Write(MasterOutputPin.OUTPUT1.Chip, MasterOutputPin.OUTPUT1.port,MasterOutputPin.OUTPUT1.PinNumber, PinState.High);
                        Thread.Sleep(1000);
                        MCP23Controller.Write(MasterOutputPin.OUTPUT1.Chip, MasterOutputPin.OUTPUT1.port, MasterOutputPin.OUTPUT1.PinNumber, PinState.Low);
                    }
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

