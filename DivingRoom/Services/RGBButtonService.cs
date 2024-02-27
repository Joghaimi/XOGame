﻿using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.PinMapping;
using Library.GPIOLib;
using Iot.Device.Mcp3428;
using Library.Enum;

namespace DivingRoom.Services
{
    public class RGBButtonServices : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private readonly ILogger<RGBButtonServices> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        Stopwatch GameStopWatch = new Stopwatch();
        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Random random = new Random();




        int Score = 0;



        int currentPeriod = 30000;

        int CurrentColor = 4;


        public RGBButtonServices(ILogger<RGBButtonServices> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
            //JQ8400AudioModule.init(SerialPort.Serial2);
            MCP23Controller.Init(true);

            _appLifetime.ApplicationStopping.Register(Stopped);
            _logger.LogWarning("Start RGBButtonService");
            //AudioPlayer.PIBackgroundSound(SoundType.Background);
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10));
            GameStopWatch.Start();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //_cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            //Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (true)
            {
              
                RGBColor selectedColor = (RGBColor)CurrentColor;
                RGBLight.SetColor(selectedColor);
                var PrimaryColor = RGBColorMapping.GetRGBColors(selectedColor);
                AudioPlayer.PIStartAudio(SoundType.Button);
                Console.WriteLine($"{PrimaryColor.Length} {selectedColor.ToString()}");
                while (GameStopWatch.ElapsedMilliseconds < 30000)
                {
                    foreach (var item in RGBButtonList)
                    {
                        bool randomBoolean = random.Next(0, 2) == 0;
                        if (randomBoolean)
                        {
                            int index = random.Next(0, PrimaryColor.Length);
                            item.TurnColorOn(PrimaryColor[index]);
                            Console.WriteLine($"{PrimaryColor[index]}");
                        }
                    }
                }
                Thread.Sleep(10000);

                GameStopWatch.Restart();

                //AudioPlayer.PIStartAudio(SoundType.Button);
                //TurnRGBButtonWithColor(selectedColor);
                //byte numberOfClieckedButton = 0;
                //GameStopWatch.Restart();
                //Console.WriteLine("New Round ================");
                //while (GameStopWatch.ElapsedMilliseconds < 30000)
                //{
                //    foreach (var item in RGBButtonList)
                //    {
                //        bool itemSelected = !item.CurrentStatus() && item.CurrentColor() == selectedColor;
                //        if (itemSelected)
                //        {
                //            MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.High);
                //            RGBLight.SetColor(RGBColor.Green);
                //            AudioPlayer.PIStartAudio(SoundType.Bonus);
                //            item.TurnColorOn(RGBColor.Off);
                //            RGBLight.TurnRGBOffAfter1Sec();
                //            numberOfClieckedButton++;
                //            VariableControlService.ActiveButtonPressed++;
                //            Console.WriteLine($"Score {VariableControlService.ActiveButtonPressed} numberOfPressed now {numberOfClieckedButton}");
                //        }
                //    }
                //    if (numberOfClieckedButton == RGBButtonList.Count())
                //        break;
                //    Thread.Sleep(10);
                //}
                //TurnRGBButtonWithColor(RGBColor.Off);


                if (CurrentColor < 9)
                    CurrentColor++;
                else
                    CurrentColor = 4;
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
            MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.Low);
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
    }
}
