﻿using Library.Media;
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
        private readonly ILogger<RGBButtonService> _logger;
        Stopwatch GameStopWatch = new Stopwatch();
        private CancellationTokenSource _cts;
        List<(int, int)> ButtonTaskList = new List<(int, int)>
        {
            (2,3),
            (8,6),
            (5,0),
            (7,4),
            (2,6),
        };
        int CurrentColor = 0;


        public RGBButtonService(ILogger<RGBButtonService> logger, IHostApplicationLifetime appLifetime)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
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
            GameStopWatch.Start();
            // Default Color For RGB IS White
            RGBLight.SetColor(RGBColor.White);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            int level = 0;
            //bool started = false;
            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    if (!VariableControlService.IsRGBButtonServiceStarted)
                        VariableControlService.IsRGBButtonServiceStarted = true;
                    RGBColor selectedColor = (RGBColor)CurrentColor;

                    ControlRoundSound(VariableControlService.GameRound);
                    StartTheGameTask(selectedColor, VariableControlService.GameRound);
                    TurnRGBButtonWithColor(selectedColor);

                    byte numberOfClickedButton = 0;
                    GameStopWatch.Restart();
                    while (GameStopWatch.ElapsedMilliseconds < 90000)
                    {
                        foreach (var item in RGBButtonList)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            bool itemSelected = !item.CurrentStatus() && item.CurrentColor() == selectedColor;
                            if (itemSelected)
                            {
                                AudioPlayer.PIStartAudio(SoundType.Bonus);
                                RGBLight.SetColor(RGBColor.Blue);
                                item.TurnColorOn(RGBColor.Off);
                                RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                                numberOfClickedButton++;
                                VariableControlService.ActiveButtonPressed++;
                                VariableControlService.TeamScore.FortRoomScore += 10;
                            }

                        }
                        if (numberOfClickedButton == RGBButtonList.Count())
                            break;
                        if (!IsGameStartedOrInGoing())
                            break;
                        Thread.Sleep(10);
                    }
                    TurnRGBButtonWithColor(RGBColor.Off);
                    if (CurrentColor < 2)
                        CurrentColor++;
                    else
                        CurrentColor = 0;
                    if (level < 4)
                    {
                        level++;
                        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                        ApplyChangesForTheNextRound();
                    }
                    else
                        StopRGBButtonService();
                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsRGBButtonServiceStarted)
                {
                    _logger.LogInformation("RGB Service Stopeed");
                    StopRGBButtonService();
                }
            }
        }



        public void StartTheGameTask(RGBColor color, Round round)
        {
            int level = (int)round;
            if (level > RGBButtonList.Count)
                return;
            bool Button1 = false; bool Button2 = false;
            (int button1Index, int button2Index) = ButtonTaskList[level];
            RGBButtonList[button1Index].TurnColorOn(color);
            RGBButtonList[button2Index].TurnColorOn(color);
            while (!Button1 || !Button2)
            {
                if (!IsGameStartedOrInGoing())
                    break;
                if (!Button1)
                {
                    if (!RGBButtonList[button1Index].CurrentStatus() && RGBButtonList[button1Index].CurrentColor() == color)
                    {
                        Button1 = true;
                        Console.WriteLine("Button #1 Pressed");
                        RGBLight.SetColor(RGBColor.Blue);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBButtonList[button1Index].TurnColorOn(RGBColor.Off);
                        RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
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
                        RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                    }
                }

                Thread.Sleep(10);
            }
            if (!IsGameStartedOrInGoing())
                return;
            Thread.Sleep(400);
        }

        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
        private void StopRGBButtonService()
        {
            StopRGBButton();
            VariableControlService.GameRound = Round.Round1;
            VariableControlService.IsRGBButtonServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            Thread.Sleep(1000);
            AudioPlayer.PIStopAudio();
        }
        private Round NextRound(Round round)
        {
            return (Round)((int)round + 1);
        }
        private void ApplyChangesForTheNextRound()
        {
            VariableControlService.IsThingsChangedForTheNewRound = false;
        }

        public void TurnRGBButtonWithColor(RGBColor color)
        {
            foreach (var item in RGBButtonList)
                item.TurnColorOn(color);
        }







        private void ControlRoundSound(Round round)
        {
            switch (round)
            {
                case Round.Round1:
                    AudioPlayer.PIStartAudio(SoundType.RoundOne);
                    break;
                case Round.Round2:
                    AudioPlayer.PIStartAudio(SoundType.RoundTwo);
                    break;
                case Round.Round3:
                    AudioPlayer.PIStartAudio(SoundType.RoundThree);
                    break;
                case Round.Round4:
                    AudioPlayer.PIStartAudio(SoundType.RoundFour);
                    break;
                case Round.Round5:
                    AudioPlayer.PIStartAudio(SoundType.RoundFive);
                    break;
                default:
                    break;
            }
        }

        private void StopRGBButton()
        {
            foreach (var item in RGBButtonList)
                item.TurnColorOn(RGBColor.Off);
        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            StopRGBButtonService();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }
    }

}
