using Library.GPIOLib;
using Library.PinMapping;
using Library.RGBLib;
using Library;
using System.Diagnostics;
using Library.LocalStorage;
using Library.Model;
using Library.Media;

namespace CatchyGame.Service
{
    public class RandomRGBScenario : IHostedService, IDisposable
    {

        Random random = new Random();
        Stopwatch GameTiming = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        private CancellationTokenSource _cts, _cts2, _cts3;
        List<SparkRGBButton> PlayerOneRGBButtonList = new List<SparkRGBButton>();


        int delayTime = 800;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);
            AudioPlayer.Init(Room.Catchy);

            var loadedData = LocalStorage.LoadData<string>("data.json");
            if (loadedData != null)
            {
                var topScore = loadedData.Split(" ");
                if (topScore.Length > 1)
                {
                    VariableControlService.TopScoreTeam = topScore[0];
                    VariableControlService.TopScore = int.Parse(topScore[1]);
                }
                else
                {
                    LocalStorage.SaveData($"{VariableControlService.TopScoreTeam} {VariableControlService.TopScore}", "data.json");
                }
                Console.WriteLine($"Load Top Score{VariableControlService.TopScore} TeamName {VariableControlService.TopScoreTeam} ");
            }



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
               RGBColor.Off);
            RGBWS2811.Commit();


            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlGame(_cts.Token));
            Task.Run(() => PlayerCatchingGame(_cts.Token));
            Task.Run(() => ControlGameTiming(_cts.Token));

            return Task.CompletedTask;
        }
        private async Task PlayerCatchingGame(CancellationToken cancellationToken)
        {


            while (true)
            {
                foreach (var button in PlayerOneRGBButtonList)
                {
                    if (button.isPressed() == 1)
                    {
                        AudioPlayer.PIStartAudio(SoundType.Success);
                        VariableControlService.Team.player[0].score += 10;
                        Console.WriteLine($"Player One {VariableControlService.Team.player[0].score}");
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
                        Thread.Sleep(3000);
                        ResetAllButton();
                    }
                    if (VariableControlService.Team.player[0].score > VariableControlService.TopScore)
                    {
                        VariableControlService.TopScore = VariableControlService.Team.player[0].score;
                        LocalStorage.SaveData($"{VariableControlService.TopScoreTeam} {VariableControlService.TopScore}", "data.json");
                    }
                    VariableControlService.GameStatus = GameStatus.Empty;
                    //VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                }
            }
        }
        private Round NextRound(Round currentRound)
        {
            if (currentRound == Round.Round3)
                return Round.Round3;
            else
                delayTime -= 100;
            return (Round)((int)currentRound + 1);
        }
        private async Task ControlGameTiming(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                VariableControlService.CurrentTime = (int)LevelTime.ElapsedMilliseconds;
                //if (VariableControlService.GameStatus == GameStatus.Started)
                //{
                //    bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.GameTiming;
                //    bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.GameStatus == GameStatus.Started && VariableControlService.IsGameTimerStarted;
                //    if (GameFinishedByTimer)
                //        StopTheGame();
                //}
            }
        }
        private void SelectRandomButton()
        {
            int selectedButton = random.Next(0, PlayerOneRGBButtonList.Count());
            PlayerOneRGBButtonList[selectedButton].Activate(true);
        }

        private void ResetAllButton()
        {
            foreach (var button in PlayerOneRGBButtonList)
                button.Activate(false);
        }

        private void StopTheGame() { }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
            //throw new NotImplementedException();
        }
        public void Dispose()
        {
            //throw new NotImplementedException();
        }
    }
}

