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
        Stopwatch GameStopWatch = new Stopwatch();
        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        List<(int, int)> ButtonTaskList = new List<(int, int)>
        {
            (2,3),
            (8,6),
            (5,0),
            (7,4),
            (2,6),
        };


        int Score = 0;



        int currentPeriod = 30000;

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
            GameStopWatch.Start();
            AudioPlayer.PIBackgroundSound(SoundType.Background);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            int level = 0;
            while (true)
            {
                RGBColor selectedColor = (RGBColor)CurrentColor;
                AudioPlayer.PIStartAudio(SoundType.Button);
                // Add Extra Task
                Console.WriteLine("Enter Task ================");
                StartGameTask(selectedColor, level);
                Console.WriteLine("Out From Task ================");
                TurnRGBButtonWithColor(selectedColor);
                byte numberOfClieckedButton = 0;
                GameStopWatch.Restart();
                Console.WriteLine("New Round ================");
                while (GameStopWatch.ElapsedMilliseconds < 30000)
                {
                    foreach (var item in RGBButtonList)
                    {
                        bool itemSelected = !item.CurrentStatus() && item.CurrentColor() == selectedColor;
                        if (itemSelected)
                        {
                            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.High);
                            RGBLight.SetColor(RGBColor.Blue);
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            item.TurnColorOn(RGBColor.Off);
                            RGBLight.TurnRGBOffAfter1Sec();
                            numberOfClieckedButton++;
                            VariableControlService.ActiveButtonPressed++;
                            Console.WriteLine($"Score {VariableControlService.ActiveButtonPressed} numberOfPressed now {numberOfClieckedButton}");
                        }
                    }
                    if (numberOfClieckedButton == RGBButtonList.Count())
                        break;
                    Thread.Sleep(10);
                }
                TurnRGBButtonWithColor(RGBColor.Off);
                if (CurrentColor < 2)
                {
                    CurrentColor++;
                   
                }
                else
                {
                    CurrentColor = 0;
                }
                if (level < 4)
                {
                    level++;
                }
                else {
                    level = 0;
                }
            }
        }
        private async Task TimingService(CancellationToken cancellationToken)
        {
            if (!IsTimerStarted)
            {
                GameStopWatch.Start();
                IsTimerStarted = true;
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
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
            _logger.LogInformation("Stop RGB Button Service");
            foreach (var item in RGBButtonList)
            {
                item.TurnColorOn(RGBColor.Off);
            }
            _logger.LogInformation("Stop Background Audio");
            AudioPlayer.PIStopAudio();
        }


        public void TurnRGBButtonWithColor(RGBColor color)
        {
            foreach (var item in RGBButtonList)
                item.TurnColorOn(color);
        }
        public void StartGameTask(RGBColor color, int level)
        {
            if (level > RGBButtonList.Count)
                return;
            bool Button1 = false; bool Button2 = false;
            (int button1Index, int button2Index) = ButtonTaskList[level];
            RGBButtonList[button1Index].TurnColorOn(color);
            RGBButtonList[button2Index].TurnColorOn(color);
            while (!Button1 || !Button2)
            {
                if (!Button1)
                {
                    if (!RGBButtonList[button1Index].CurrentStatus() && RGBButtonList[button1Index].CurrentColor() == color)
                    {
                        Button1 = true;
                        Console.WriteLine("Button #1 Pressed");
                        RGBLight.SetColor(RGBColor.Blue);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBButtonList[button1Index].TurnColorOn(RGBColor.Off);
                        RGBLight.TurnRGBOffAfter1Sec();
                    }
                }
                if (!Button2)
                {
                    if (!RGBButtonList[button2Index].CurrentStatus() && RGBButtonList[button2Index].CurrentColor() == color)
                    {
                        Button2 = true;
                        Console.WriteLine("Button #2 Pressed");
                        RGBLight.SetColor(RGBColor.Blue);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBButtonList[button2Index].TurnColorOn(RGBColor.Off);
                        RGBLight.TurnRGBOffAfter1Sec();

                    }
                }
                Thread.Sleep(10);
            }
            Thread.Sleep(400);
        }
    }
}
