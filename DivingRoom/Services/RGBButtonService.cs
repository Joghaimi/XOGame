using Library.Media;
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
        private CancellationTokenSource _cts;
        bool IsTimerStarted = false;
        Random random = new Random();
        int numberOfSelectedButton = 0;
        int numberOfPressedButton = 0;
        int Score = 0;
        int CurrentColor = 5;
        int difficulty = 6;
        List<int> unSelectedPushButton = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9 };
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

            GameStopWatch.Start();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (true)
            {

                Reset();
                if (IsGameStartedOrInGoing())
                {
                    if (!VariableControlService.IsRGBButtonServiceStarted)
                        VariableControlService.IsRGBButtonServiceStarted = true;
                    while (difficulty >= 2)
                    {
                        GameStopWatch.Restart();
                        bool isSelected = false;
                        if (IsGameStartedOrInGoing())
                            break;
                        while (GameStopWatch.ElapsedMilliseconds < 60000)
                        {
                            if (IsGameStartedOrInGoing())
                                break;
                            if (!isSelected)
                            {
                                numberOfPressedButton = 0;
                                numberOfSelectedButton = 0;
                                UnselectAllPB();

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

                            }

                            foreach (var item in RGBButtonList)
                            {
                                if (!item.CurrentStatus() && item.isSet())
                                {
                                    numberOfPressedButton++;
                                    VariableControlService.TeamScore.DivingRoomScore++;
                                    //Score++;
                                    item.TurnColorOn(RGBColor.Off);
                                    item.Set(false);
                                    AudioPlayer.PIStartAudio(SoundType.Bonus);
                                    Console.WriteLine($"score {Score}");
                                }
                                else if (!item.CurrentStatus() && !item.isSet())
                                {
                                    Score--;
                                    VariableControlService.TeamScore.DivingRoomScore--;
                                    AudioPlayer.PIStartAudio(SoundType.Descend);
                                }
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
                        Console.WriteLine("Game Ended");
                        if (CurrentColor < 7)
                            CurrentColor++;
                        else
                            CurrentColor = 5;
                        difficulty -= 2;
                    }
                    //RGBLight.SetColor(RGBColor.Off);
                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsRGBButtonServiceStarted)
                {
                    _logger.LogInformation("RGB Service Stopeed");
                    StopRGBButtonService();
                }



            }
        }

        private void StopRGBButtonService()
        {
            Reset();
            VariableControlService.IsRGBButtonServiceStarted = false;
            VariableControlService.IsTheGameFinished = true;
            TurnRGBButtonWithColor(RGBColor.Off);
            AudioPlayer.PIStartAudio(SoundType.MissionAccomplished);
            Thread.Sleep(1000);
            AudioPlayer.PIStopAudio();
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

        private void Reset()
        {
            Score = 0;
            CurrentColor = 5;
            difficulty = 6;
        }


        public void Stopped()
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
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
