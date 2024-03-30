using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.PinMapping;
using Library.GPIOLib;
using Iot.Device.Mcp3428;
using Library.Enum;
using Microsoft.Extensions.Logging;

namespace DivingRoom.Services
{
    public class RGBButtonServices : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private readonly ILogger<RGBButtonServices> _logger;
        private readonly IHostApplicationLifetime _appLifetime;
        Stopwatch GameStopWatch = new Stopwatch();
        private CancellationTokenSource _cts;
        //bool IsTimerStarted = false;
        Random random = new Random();
        int numberOfSelectedButton = 0;
        int numberOfPressedButton = 0;
        int Score = 0;
        int CurrentColor = 5;
        int difficulty = 6;
        List<int> unSelectedPushButton = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };

        bool IsEnterdTheRoom = false;
        public RGBButtonServices(ILogger<RGBButtonServices> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {

            _appLifetime.ApplicationStopping.Register(Stopped);
            _logger.LogInformation("Start RGBButtonService");

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
            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN2, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3, PinMode.Input);
            GameStopWatch.Start();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (true)
            {

                if (IsGameStartedOrInGoing())
                {
                    while (!IsEnterdTheRoom)
                    {
                        IsEnterdTheRoom = MCP23Controller.Read(MasterDI.IN1) ||
                                          MCP23Controller.Read(MasterDI.IN2) ||
                                          MCP23Controller.Read(MasterDI.IN3);
                        if (!IsGameStartedOrInGoing())
                            break;
                    }
                    if (!VariableControlService.IsRGBButtonServiceStarted)
                    {
                        Reset();
                        VariableControlService.IsRGBButtonServiceStarted = true;
                        Console.WriteLine("****************** Game Started");
                    }
                    while (difficulty >= 2)
                    {
                        GameStopWatch.Restart();
                        bool isSelected = false;
                        if (!IsGameStartedOrInGoing())
                            break;
                        ControlRoundSound(VariableControlService.GameRound);
                        while (GameStopWatch.ElapsedMilliseconds < 150000)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            // Turn RGB Button On For This Round
                            if (!isSelected)
                            {
                                numberOfPressedButton = 0;
                                numberOfSelectedButton = 0;
                                UnselectAllPB();
                                var PrimaryColor = SelectColor((RGBColor)CurrentColor);
                                TurnSelectiveRGBButtonWithColorRandom(PrimaryColor);
                                TurnUnSelectedRGBButtonWithColorRandom(PrimaryColor);
                                AudioPlayer.PIStartAudio(SoundType.LightsChange);
                                isSelected = true;
                                _logger.LogTrace("Select RGB Color");
                                _logger.LogTrace($"Color#1 {PrimaryColor[0]}");
                                _logger.LogTrace($"Color#2 {PrimaryColor[1]}");
                                _logger.LogTrace($"Finished number of Selected button {numberOfSelectedButton} number of pressed {numberOfPressedButton} unSelectedPushButton {unSelectedPushButton.Count}");
                            }
                            // Loop Inside The Button To Allow 
                            int index = 0;
                            foreach (var item in RGBButtonList)
                            {
                                bool PressSelectedButton = !item.CurrentStatusWithCheckForDelay() && item.isSet();
                                AddToScore(PressSelectedButton, index);
                                bool PressUnselectedButton = !item.CurrentStatusWithCheckForDelay() && !item.isSet();
                                RemoveFromScore(PressUnselectedButton, index);
                                index++;
                            }

                            if (numberOfPressedButton == numberOfSelectedButton)
                            {
                                numberOfPressedButton = 0;
                                numberOfSelectedButton = 0;
                                isSelected = false;
                                if (CurrentColor < 7)
                                    CurrentColor++;
                                else
                                    CurrentColor = 5;
                                await Task.Delay(1000); // Delay for 1 second
                            }
                        }
                        _logger.LogTrace("Game Ended");
                        if (CurrentColor < 7)
                            CurrentColor++;
                        else
                            CurrentColor = 5;
                        difficulty -= 2;
                        VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                    }
                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsRGBButtonServiceStarted)
                {
                    _logger.LogInformation("RGB Service Stopped");
                    StopRGBButtonService();
                    Reset();
                    IsEnterdTheRoom = false;
                }
                Thread.Sleep(10);

            }
        }
        private void AddToScore(bool addScore, int index)
        {
            if (addScore)
            {
                RGBButtonList[index].TurnColorOn(RGBColor.Off);
                RGBButtonList[index].Set(false);
                RGBButtonList[index].BlockForASec();
                AudioPlayer.PIStartAudio(SoundType.Bonus);
                VariableControlService.TeamScore.DivingRoomScore++;
                numberOfPressedButton++;
                _logger.LogInformation($"+ score {VariableControlService.TeamScore.DivingRoomScore}");
            }
        }
        private void RemoveFromScore(bool removeScore, int index)
        {
            if (removeScore)
            {

                RGBButtonList[index].BlockForASec();
                VariableControlService.TeamScore.DivingRoomScore--;
                AudioPlayer.PIStartAudio(SoundType.Descend);
                Console.WriteLine($"- score {VariableControlService.TeamScore.DivingRoomScore}");
            }

        }

        private void StopRGBButtonService()
        {
            Reset();
            VariableControlService.IsRGBButtonServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            TurnRGBButtonWithColor(RGBColor.Off);
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            AudioPlayer.PIStopAudio();
            RGBLight.SetColor(RGBColor.White);
            Thread.Sleep(1000);
        }
        private void UnselectAllPB()
        {
            var index = 0;
            unSelectedPushButton.Clear();
            foreach (var item in RGBButtonList)
            {
                item.Set(false);
                item.TurnColorOn(RGBColor.Off);
                unSelectedPushButton.Add(index);
                index++;
            }
        }
        private RGBColor[] SelectColor(RGBColor selectedColor)
        {
            _logger.LogTrace($"Selected Color {selectedColor.ToString()}");
            RGBLight.SetColor(selectedColor);
            AudioPlayer.PIStartAudio(SoundType.LightsChange);
            return RGBColorMapping.GetRGBColors(selectedColor);
        }

        private void TurnUnSelectedRGBButtonWithColorRandom(RGBColor[] colorArray)
        {

            RGBColor[] unSelectedColorArray = { RGBColor.Green, RGBColor.Red, RGBColor.Blue };
            unSelectedColorArray = unSelectedColorArray.Where(val => val != colorArray[0] && val != colorArray[1]).ToArray();
            if (unSelectedColorArray.Length > 0)
            {
                _logger.LogTrace($"unselected {unSelectedColorArray[0].ToString()}");
                foreach (var item in unSelectedPushButton)
                {
                    _logger.LogTrace($"other color {item}");
                    RGBButtonList[item].TurnColorOn(unSelectedColorArray[0]);
                }
            }
        }


        private void TurnSelectiveRGBButtonWithColorRandom(RGBColor[] colorArray)
        {
            for (int i = 0; i < difficulty; i += 2)
            {
                int RGBButtonIndex = GetRandomRGBButtonIndexFormUnselectedList();
                SelectRGBButton(RGBButtonIndex, colorArray[0]);
                numberOfSelectedButton++;
                RGBButtonIndex = GetRandomRGBButtonIndexFormUnselectedList();
                SelectRGBButton(RGBButtonIndex, colorArray[1]);
                numberOfSelectedButton++;
            }
        }
        private int GetRandomRGBButtonIndexFormUnselectedList()
        {
            int randomNumber = random.Next(0, unSelectedPushButton.Count);
            int selectedButtonIndex = unSelectedPushButton[randomNumber];
            return selectedButtonIndex;
        }

        private void SelectRGBButton(int index, RGBColor rGBColor)
        {
            RGBButtonList[index].TurnColorOn(rGBColor);
            RGBButtonList[index].Set(true);
            unSelectedPushButton.Remove(index);
            _logger.LogTrace($"Button #{index} color is {rGBColor.ToString()}");
        }

        private Round NextRound(Round round)
        {
            return (Round)((int)round + 1);
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


        private void Reset()
        {
            VariableControlService.TeamScore.DivingRoomScore = 0;
            CurrentColor = 5;
            difficulty = 6;
        }


        public void Stopped()
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
            RGBLight.SetColor(RGBColor.White);
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
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
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
