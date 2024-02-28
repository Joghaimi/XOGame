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
        int numberOfSelectedButton = 0;
        int numberOfPressedButton = 0;
        int Score = 0;
        int currentPeriod = 30000;
        int CurrentColor = 5;
        int difficulty = 6;
        //int[] unSelectedPushButton = { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> unSelectedPushButton = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        public RGBButtonServices(ILogger<RGBButtonServices> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;
         
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {



            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
            MCP23Controller.Init(true);

            MCP23Controller.PinModeSetup(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinMode.Output);
            MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.Low);

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
            Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            //foreach (var item in RGBButtonList)
            //{
            //    item.TurnColorOn(RGBColor.Blue);
            //}
            //while (true)
            //{
            //    Console.Write($"Status");
            //    foreach (var item in RGBButtonList)
            //    {
            //        Console.Write(!item.CurrentStatus());
            //        if (!item.CurrentStatus())
            //        {
            //            item.TurnColorOn(RGBColor.Off);
            //            //AudioPlayer.PIStartAudio(SoundType.Bonus);
            //            Console.WriteLine($"score {Score}");
            //        }
            //    }
            //    Console.WriteLine();
            //    Thread.Sleep(1000);
            //}


            while (true)
            {
                bool isIntered =
                        MCP23Controller.Read(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber) ||
                        MCP23Controller.Read(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber) ||
                        MCP23Controller.Read(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber);
                MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port,
                    MasterOutputPin.OUTPUT6.PinNumber, PinState.High);

                while (isIntered && difficulty >= 2)
                {
                    Console.WriteLine($"difficulty {difficulty}");
                    GameStopWatch.Restart();
                    bool isSelected = false;
                    while (GameStopWatch.ElapsedMilliseconds < 60000)
                    {
                        if (!isSelected)
                        {
                            var index = 0;
                            unSelectedPushButton.Clear();
                            foreach (var item in RGBButtonList)
                            {
                                Console.WriteLine(index);
                                item.Set(false);
                                item.TurnColorOn(RGBColor.Off);
                                unSelectedPushButton.Add(index);
                                index++;
                            }
                            RGBColor selectedColor = (RGBColor)CurrentColor;
                            Console.WriteLine($"Game Started {selectedColor.ToString()}");

                            RGBLight.SetColor(selectedColor);
                            var PrimaryColor = RGBColorMapping.GetRGBColors(selectedColor);
                            Console.WriteLine(PrimaryColor[0]);
                            Console.WriteLine(PrimaryColor[1]);
                            AudioPlayer.PIStartAudio(SoundType.Button);
                            for (int i = 0; i < difficulty; i += 2)
                            {
                                int randomNumber = random.Next(0, unSelectedPushButton.Count);
                                int selectedButtonIndex = unSelectedPushButton[randomNumber];
                                RGBButtonList[selectedButtonIndex].TurnColorOn(PrimaryColor[0]);
                                RGBButtonList[selectedButtonIndex].Set(true);
                                Console.WriteLine($"Button #{selectedButtonIndex} color is {PrimaryColor[0].ToString()}");
                                numberOfSelectedButton++;
                                unSelectedPushButton.Remove(selectedButtonIndex);//= unSelectedPushButton.Where(val => val != selectedButtonIndex).ToArray();
                                randomNumber = random.Next(0, unSelectedPushButton.Count);
                                selectedButtonIndex = unSelectedPushButton[randomNumber];
                                RGBButtonList[selectedButtonIndex].TurnColorOn(PrimaryColor[1]);
                                RGBButtonList[selectedButtonIndex].Set(true);
                                Console.WriteLine($"Button #{selectedButtonIndex} color is {PrimaryColor[1].ToString()}");
                                numberOfSelectedButton++;
                                unSelectedPushButton.Remove(selectedButtonIndex);
                            }
                            Console.WriteLine($"Finished number of Selected button {numberOfSelectedButton} number of pressed {numberOfPressedButton} unSelectedPushButton {unSelectedPushButton.Count}");



                            RGBColor[] unSelectedColorArray = { RGBColor.Green, RGBColor.Red, RGBColor.Blue };
                            unSelectedColorArray = unSelectedColorArray.Where(val => val != PrimaryColor[0] && val != PrimaryColor[1]).ToArray();
                            if (unSelectedColorArray.Length > 0)
                            {
                                Console.WriteLine($"unselected {unSelectedColorArray[0].ToString()}");
                                foreach (var item in unSelectedPushButton)
                                {
                                    Console.WriteLine($"other color {item}");
                                    RGBButtonList[item].TurnColorOn(unSelectedColorArray[0]);
                                }
                            }
                            isSelected = true;


                            //foreach (var item in RGBButtonList)
                            //{
                            //    bool randomBoolean = random.Next(0, 2) == 0;
                            //    if (randomBoolean)
                            //    {
                            //        int index = random.Next(0, PrimaryColor.Length);
                            //        item.TurnColorOn(PrimaryColor[index]);
                            //        item.Set(true);
                            //        numberOfSelectedButton++;
                            //        Console.WriteLine($"{PrimaryColor[index]}");
                            //    }
                            //    else
                            //    {
                            //        int index = random.Next(0, 3);
                            //        item.TurnColorOn((RGBColor)index);
                            //    }
                            //}
                        }

                        foreach (var item in RGBButtonList)
                        {
                            if (!item.CurrentStatus() && item.isSet())
                            {
                                numberOfPressedButton++;
                                Score++;
                                item.TurnColorOn(RGBColor.Off);
                                item.Set(false);
                                AudioPlayer.PIStartAudio(SoundType.Bonus);
                                Console.WriteLine($"score {Score}");
                            }
                        }
                        if (numberOfPressedButton == numberOfSelectedButton)
                        {
                            numberOfPressedButton = 0;
                            numberOfSelectedButton = 0;
                            isSelected = false;
                            CurrentColor++;
                            await Task.Delay(1000); // Delay for 1 second
                        }
                    }
                    Console.WriteLine("Game Ended");
                    if (CurrentColor < 9)
                        CurrentColor++;
                    else
                        CurrentColor = 5;
                    difficulty -= 2;
                }
                MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.Low);
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
            RGBLight.SetColor(RGBColor.Off);
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
