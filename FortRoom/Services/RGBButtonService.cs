using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.PinMapping;
using Library.GPIOLib;
using Iot.Device.Mcp3428;

namespace FortRoom.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private readonly ILogger<ObstructionControlService> _logger;
        private readonly IHostApplicationLifetime _appLifetime;

        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        int SlowPeriod = 10000;
        int MediumPeriod = 15000;

        int slowChangeTime = 5000;
        int mediumChangeTime = 3000;
        int highChangeTime = 1000;

        int changingSpeed = 5000;



        int currentPeriod = 30000;
        bool colorSelected = false;

        int CurrentColor = 0;


        public RGBButtonService(ILogger<ObstructionControlService> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStopping.Register(Stopped);
            _logger.LogWarning("Start RGBButtonService");
            AudioPlayer.PIBackgroundSound(SoundType.Background);
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR12, RGBButtonPin.RGBG12, RGBButtonPin.RGBB12, RGBButtonPin.RGBPB12));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15));
            //RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16));


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            Random random = new Random();
            while (true)
            {
               
                Console.WriteLine($"Green {GameStopWatch.ElapsedMilliseconds}");
                RGBLight.SetColor(RGBColor.Green);
                Console.WriteLine($"Green End {GameStopWatch.ElapsedMilliseconds}");
                Thread.Sleep(3000);
                
                Console.WriteLine($"Blue {GameStopWatch.ElapsedMilliseconds}");
                RGBLight.SetColor(RGBColor.Blue);
                Console.WriteLine($"Blue End {GameStopWatch.ElapsedMilliseconds}");
                Thread.Sleep(3000);
                
                Console.WriteLine($"Off {GameStopWatch.ElapsedMilliseconds}");
                RGBLight.SetColor(RGBColor.Off);
                Console.WriteLine($"Off End {GameStopWatch.ElapsedMilliseconds}");
                Thread.Sleep(3000);



                //MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.High);
                //Thread.Sleep(2000);
                //RGBLight.SetColor(RGBColor.Off);
                //MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.Low);
                //Thread.Sleep(2000);




                //RGBColor selectedColor = (RGBColor)CurrentColor;
                //AudioPlayer.PIStartAudio(SoundType.Button);
                //byte numberOfClieckedButton = 0;
                //foreach (var item in RGBButtonList)
                //{
                //    item.TurnColorOn(selectedColor);
                //}
                //while (GameStopWatch.ElapsedMilliseconds < currentPeriod)
                //{
                //    foreach (var item in RGBButtonList)
                //    {
                //        if (!item.CurrentStatus() && item.CurrentColor() == selectedColor)
                //        {

                //            MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.High);
                //            RGBLight.SetColor(RGBColor.Green);
                //            AudioPlayer.PIStartAudio(SoundType.Bonus);
                //            item.TurnColorOn(RGBColor.Off);
                //            Thread.Sleep(2000);
                //            MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.Low);
                //            RGBLight.SetColor(RGBColor.Off);
                //            //RGBLight.TurnRGBOffAfter1Sec();


                //            numberOfClieckedButton++;
                //            VariableControlService.ActiveButtonPressed++;
                //            Console.WriteLine($"Score {VariableControlService.ActiveButtonPressed} numberOfPressed now {numberOfClieckedButton}");
                //        }
                //    }
                //    if (numberOfClieckedButton == RGBButtonList.Count())
                //    {
                //        break;
                //    }
                //    Thread.Sleep(10);
                //}
                //if (currentPeriod > 10)
                //    currentPeriod -= 5;
                //foreach (var item in RGBButtonList)
                //{
                //    item.TurnColorOn(RGBColor.Off);
                //}
                //if (CurrentColor < 5)
                //    CurrentColor++;
                //GameStopWatch.Restart();
            }













            //bool activeButton = false;
            //Stopwatch timer = new Stopwatch();
            //Stopwatch timerToStart = new Stopwatch();
            //int activeButtonIndox = -1;
            //int randomTime = random.Next(1000, 5000);
            //timerToStart.Start();
            //timer.Start();
            //_logger.LogWarning("Started .... ");
            //foreach (var item in RGBButtonList)
            //{
            //    item.TurnColorOn(RGBColor.Green);
            //}
            //while (!cancellationToken.IsCancellationRequested)
            //{
            //    foreach (var item in RGBButtonList)
            //    {
            //        if (!item.CurrentStatus())
            //        {
            //            item.TurnColorOn(RGBColor.Off);
            //        }
            //    }

            //    //if (VariableControlService.IsTheGameStarted)
            //    //{
            //    //    if (!activeButton && timerToStart.ElapsedMilliseconds > randomTime)
            //    //    {

            //    //        activeButton = true;
            //    //        activeButtonIndox = random.Next(0, RGBButtonList.Count -1);
            //    //        _logger.LogWarning($"Button {activeButtonIndox} Activated");
            //    //        RGBLight.SetColor(RGBColor.Off);
            //    //        AudioPlayer.PIStartAudio(SoundType.Button);
            //    //        //JQ8400AudioModule.PlayAudio((int)SoundType.Button);
            //    //        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
            //    //        timer.Restart();
            //    //        timerToStart.Restart();
            //    //        randomTime = random.Next(1000, 5000);
            //    //    }
            //    //    if (activeButton & timer.ElapsedMilliseconds >= 5000)//changingSpeed)
            //    //    {
            //    //        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
            //    //        _logger.LogWarning($"Button {activeButtonIndox} Deactivated");
            //    //        activeButtonIndox = -1;
            //    //        activeButton = false;
            //    //        timer.Restart();
            //    //        timerToStart.Restart();
            //    //    }
            //    //    if (activeButton && activeButtonIndox > -1)
            //    //    {
            //    //        bool isPressed = !RGBButtonList[activeButtonIndox].CurrentStatus();
            //    //        if (isPressed)
            //    //        {
            //    //            AudioPlayer.PIStartAudio(SoundType.Bonus);
            //    //            //JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
            //    //            activeButton = false;
            //    //            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
            //    //            RGBLight.SetColor(RGBColor.Green);
            //    //            VariableControlService.ActiveButtonPressed++;
            //    //            _logger.LogWarning($"Button {activeButtonIndox} Pressed");
            //    //            _logger.LogWarning($"Current Score {VariableControlService.ActiveButtonPressed}");
            //    //            activeButtonIndox = -1;
            //    //            timerToStart.Restart();
            //    //            timer.Restart();
            //    //        }
            //    //    }
            //    //}
            //    //// Sleep for a short duration to avoid excessive checking
            //    Thread.Sleep(10);
            //}

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
        public void Stopped()
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.Low);
            _logger.LogInformation("Stop RGB Button Service");
            foreach (var item in RGBButtonList)
            {
                item.TurnColorOn(RGBColor.Off);
            }
            _logger.LogInformation("Stop Background Audio");
            AudioPlayer.PIStopAudio();
        }
    }
}
