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

        public RGBButtonService(ILogger<ObstructionControlService> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _appLifetime.ApplicationStopping.Register(Stopped);
            _logger.LogInformation("Start RGBButtonService");
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
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16));


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
            _logger.LogTrace("Started .... ");
            while (!cancellationToken.IsCancellationRequested)
            {

                if (VariableControlService.IsTheGameStarted)
                {
                    if (!activeButton && timerToStart.ElapsedMilliseconds > randomTime)
                    {

                        activeButton = true;
                        activeButtonIndox = random.Next(0, RGBButtonList.Count -1);
                        _logger.LogTrace($"Button {activeButtonIndox} Activated");
                        RGBLight.SetColor(RGBColor.Off);
                        AudioPlayer.PIStartAudio(SoundType.Button);
                        //JQ8400AudioModule.PlayAudio((int)SoundType.Button);
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
                        timer.Restart();
                        timerToStart.Restart();
                        randomTime = random.Next(1000, 5000);
                    }
                    if (activeButton & timer.ElapsedMilliseconds >= 5000)//changingSpeed)
                    {
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                        _logger.LogTrace($"Button {activeButtonIndox} Deactivated");
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
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            //JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
                            activeButton = false;
                            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                            RGBLight.SetColor(RGBColor.Green);
                            VariableControlService.ActiveButtonPressed++;
                            _logger.LogTrace($"Button {activeButtonIndox} Pressed");
                            _logger.LogTrace($"Current Score {VariableControlService.ActiveButtonPressed}");
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
        public void Stopped()
        {
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
