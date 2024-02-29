using System.Diagnostics;
using Library.RGBLib;
using Library;
using Library.Media;
using Library.PinMapping;
using Library.GPIOLib;
using System.Device.Gpio;
using Iot.Device.Mcp3428;

namespace FloorIsLava.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private CancellationTokenSource _cts, _cts2;
        bool IN4, IN2, IN3 = false;







        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            // Init RGB
            //RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);

            Console.WriteLine("Init Button#1");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1));
            MCP23Controller.PinModeSetup(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN4.Chip, MasterDI.IN4.port, MasterDI.IN4.PinNumber, PinMode.Input);

            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinMode.Output);


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (true)
            {


                if (!MCP23Controller.Read(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber) && !IN2)
                {
                    IN2 = true;
                    RGBLight.SetColor(RGBColor.Green);
                    RGBLight.TurnRGBOFFDelayed();
                    Console.WriteLine("====");
                }
                if (!MCP23Controller.Read(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber) && !IN3)
                {
                    IN3 = true;
                    RGBLight.SetColor(RGBColor.Green);
                    RGBLight.TurnRGBOFFDelayed();
                    Console.WriteLine("====");

                }
                if (!MCP23Controller.Read(MasterDI.IN4.Chip, MasterDI.IN4.port, MasterDI.IN4.PinNumber) && !IN4)
                {
                    IN4 = true;
                    RGBLight.SetColor(RGBColor.Green);
                    RGBLight.TurnRGBOFFDelayed();
                    Console.WriteLine("====");

                }
                if (IN2 && IN3 && IN4)
                {
                    Console.WriteLine("Pressed all 3");

                    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber,PinState.High);
                    //while (!RGBButtonList[0].CurrentStatus())
                    //{

                    //}
                    //Console.WriteLine("Button Pressed");

                    //MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.Low);
                    //RGBButtonList[0].TurnColorOn(RGBColor.Blue);
                    //RGBLight.SetColor(RGBColor.Blue);
                    IN2 = false;
                    IN3 = false;
                    IN4 = false;
                }


            }
            //bool activeButton = false;
            //Stopwatch timer = new Stopwatch();
            //Stopwatch timerToStart = new Stopwatch();
            //int activeButtonIndox = -1;
            //Random random = new Random();
            //int randomTime = random.Next(1000, 5000);
            //timerToStart.Start();
            //timer.Start();
            //Console.WriteLine("Started .... ");
            //while (!cancellationToken.IsCancellationRequested)
            //{

            //    if (VariableControlService.IsTheGameStarted)
            //    {
            //        if (!activeButton && timerToStart.ElapsedMilliseconds > randomTime)
            //        {

            //            activeButton = true;
            //            activeButtonIndox = random.Next(0, 12);
            //            Console.WriteLine($"Button {activeButtonIndox} Activated");
            //            RGBLight.SetColor(RGBColor.Off);
            //            JQ8400AudioModule.PlayAudio((int)SoundType.Button);
            //            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
            //            timer.Restart();
            //            timerToStart.Restart();
            //            randomTime = random.Next(1000, 5000);
            //        }
            //        if (activeButton & timer.ElapsedMilliseconds >= 5000)//changingSpeed)
            //        {
            //            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
            //            Console.WriteLine($"Button {activeButtonIndox} Deactivated");
            //            activeButtonIndox = -1;
            //            activeButton = false;
            //            timer.Restart();
            //            timerToStart.Restart();
            //        }
            //        if (activeButton && activeButtonIndox > -1)
            //        {
            //            bool isPressed = !RGBButtonList[activeButtonIndox].CurrentStatus();
            //            if (isPressed)
            //            {
            //                JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
            //                activeButton = false;
            //                RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
            //                RGBLight.SetColor(RGBColor.Green);
            //                VariableControlService.ActiveButtonPressed++;
            //                Console.WriteLine($"Button {activeButtonIndox} Pressed");
            //                Console.WriteLine($"Current Score {VariableControlService.ActiveButtonPressed}");
            //                activeButtonIndox = -1;
            //                timerToStart.Restart();
            //                timer.Restart();
            //            }
            //        }
            //    }
            // Sleep for a short duration to avoid excessive checking
            //    Thread.Sleep(10);
            //}
        }
        private async Task TimingService(CancellationToken cancellationToken)
        {
            //if (VariableControlService.IsTheGameStarted)
            //{
            //    if (!IsTimerStarted)
            //    {
            //        GameStopWatch.Start();
            //        IsTimerStarted = true;
            //    }
            //}
            //while (true)
            //{
            //    if (GameStopWatch.ElapsedMilliseconds > SlowPeriod && GameStopWatch.ElapsedMilliseconds < MediumPeriod)
            //    {
            //        changingSpeed = mediumChangeTime;
            //    }
            //    else if (GameStopWatch.ElapsedMilliseconds > MediumPeriod)
            //    {
            //        changingSpeed = highChangeTime;
            //    }
            //    Thread.Sleep(10);
            //}
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
