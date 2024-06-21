﻿using Library.GPIOLib;
using Library.LocalStorage;
using Library.Media;
using Library.Model;
using Library.PinMapping;
using Library;
using Library.RGBLib;
using System.Diagnostics;

namespace CatchyGame.Service
{
    public class RandomRGBScenarioWithGameMode : IHostedService, IDisposable
    {
        Random random = new Random();
        Stopwatch LevelTime = new Stopwatch();
        private CancellationTokenSource _cts, _cts2, _cts3;


        List<SparkRGBButton> TeamRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerOneRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerTwoRGBButtonList = new List<SparkRGBButton>();
        int delayTime = 800;


        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);
            AudioPlayer.Init(Room.Catchy);

            var loadedData = LocalStorage.LoadData<TopScore>("data.json");
            Console.WriteLine(loadedData + $"loadedData {loadedData.name} - {loadedData.Score}");
            if (loadedData != null)
                VariableControlService.topScore = loadedData;




            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR4Extra, RGBButtonPin.RGBG4Extra, RGBButtonPin.RGBB4Extra, RGBButtonPin.RGBPB4Extra), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR12Extra, RGBButtonPin.RGBG12Extra, RGBButtonPin.RGBB12Extra, RGBButtonPin.RGBPB12Extra), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16), 5, Library.RGBColor.Green));
            RGBWS2811.Init();
            RGBWS2811.SetColorByRange(
               VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
               RGBColor.purple);
            RGBWS2811.Commit();


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //Task.Run(() => ControlGame(_cts.Token));
            Task.Run(() => PlayerCatchingGame(_cts.Token));
            //Task.Run(() => ControlGameTiming(_cts.Token));

            return Task.CompletedTask;
        }
        private async Task PlayerCatchingGame(CancellationToken cancellationToken)
        {
            foreach (var item in PlayerOneRGBButtonList)
            {
                item.Activate(true);
                //Console.WriteLine($"index {index}");
                Thread.Sleep(1000);
                //index++;
            }

            while (true)
            {
                //var index = 0; 
                //foreach (var item in PlayerOneRGBButtonList)
                //{
                //    item.Activate(true);
                //    Console.WriteLine($"index {index}");
                //    Thread.Sleep(1000);
                //    index++;
                //}  

                if (VariableControlService.GameMode == GameMode.inTeam)
                {
                    //    // @ToDo: Change the list name 
                    int index = 0;
                    foreach (var button in PlayerOneRGBButtonList)
                    {
                        if (button.isPressed() == 1)
                        {

                            AudioPlayer.PIStartAudio(SoundType.Success);
                            //VariableControlService.Team.player[0].score += 10;
                            Console.WriteLine($"Player One {VariableControlService.Team.player[0].score} ============ index {index}");
                            index++;
                        }
                    }
                }
                else if (VariableControlService.GameMode == GameMode.inWar)
                {
                    foreach (var button in PlayerOneRGBButtonList)
                    {
                        if (button.isPressed() == 1)
                        {
                            AudioPlayer.PIStartAudio(SoundType.Success);
                            VariableControlService.Team.player[0].score += 10;
                            Console.WriteLine($"Player One Press the Button,new Sore {VariableControlService.Team.player[0].score}");
                        }
                    }
                    foreach (var button in PlayerTwoRGBButtonList)
                    {
                        if (button.isPressed() == 1)
                        {
                            AudioPlayer.PIStartAudio(SoundType.Success);
                            VariableControlService.Team.player[1].score += 10;
                            Console.WriteLine($"Player Two Press the Button,new Sore {VariableControlService.Team.player[1].score}");
                        }
                    }
                }



            }
        }

        private async Task ControlGame(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                // Number Of Button 
                while (VariableControlService.GameStatus == GameStatus.Started)
                {
                    LevelTime.Restart();
                    while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                    {

                        SelectRandomButton();
                        Thread.Sleep(2000);
                        ResetAllButton();
                    }
                    if (VariableControlService.Team.player[0].score > VariableControlService.topScore.Score)
                    {
                        VariableControlService.topScore.Score = VariableControlService.Team.player[0].score;
                        VariableControlService.topScore.name = VariableControlService.Team.teamName;
                        LocalStorage.SaveData(VariableControlService.topScore, "data.json");
                    }
                    VariableControlService.GameStatus = GameStatus.Empty;
                }
            }
        }

        private async Task ControlGameTiming(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                VariableControlService.CurrentTime = (int)LevelTime.ElapsedMilliseconds;
            }
        }
        private void SelectRandomButton()
        {
            if (VariableControlService.GameMode == GameMode.inTeam)
            {
                // To Do , Edit it so the list for the team 
                int selectedButton = random.Next(0, PlayerOneRGBButtonList.Count());
                PlayerOneRGBButtonList[selectedButton].Activate(true);
            }
            else if (VariableControlService.GameMode == GameMode.inWar)
            {
                int selectedButton = random.Next(0, PlayerOneRGBButtonList.Count());
                PlayerOneRGBButtonList[selectedButton].Activate(true);
                selectedButton = random.Next(0, PlayerTwoRGBButtonList.Count());
                PlayerTwoRGBButtonList[selectedButton].Activate(true);
            }
        }

        private void ResetAllButton()
        {
            if (VariableControlService.GameMode == GameMode.inTeam)
                foreach (var button in PlayerOneRGBButtonList)
                    button.Activate(false);
            else if (VariableControlService.GameMode == GameMode.inWar)
            {
                foreach (var button in PlayerOneRGBButtonList)
                    button.Activate(false);
                foreach (var button in PlayerTwoRGBButtonList)
                    button.Activate(false);
            }
        }

        private void StopTheGame() { }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }
        public void Dispose()
        {
        }
    }
}
