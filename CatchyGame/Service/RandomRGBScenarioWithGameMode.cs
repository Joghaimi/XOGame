using Library.GPIOLib;
using Library.LocalStorage;
using Library.Media;
using Library.Model;
using Library.PinMapping;
using Library;
using Library.RGBLib;
using System.Diagnostics;
using Library.APIIntegration;

namespace CatchyGame.Service
{
    public class RandomRGBScenarioWithGameMode : IHostedService, IDisposable
    {
        Random random = new Random();
        Stopwatch GameTime = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        private CancellationTokenSource _cts, _cts2, _cts3, _cts4;
        bool gameFinishedButScoreNotSend = false;

        List<SparkRGBButton> TeamRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerOneRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerTwoRGBButtonList = new List<SparkRGBButton>();
        int delayTime = 800;

        bool toggleSuccessSound = false;

        RGBColor inActiveGameRGBColor = RGBColor.Red;

        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);
            AudioPlayer.Init(Room.Catchy);

            var loadedData = LocalStorage.LoadData<TopScore>("data.json");
            Console.WriteLine(loadedData + $"loadedData {loadedData.name} - {loadedData.Score}");
            if (loadedData != null)
                VariableControlService.topScore = loadedData;


            // Player One 
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR4Extra, RGBButtonPin.RGBG4Extra, RGBButtonPin.RGBB4Extra, RGBButtonPin.RGBPB4Extra), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15), 5, Library.RGBColor.Blue));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16), 5, Library.RGBColor.Green));

            // Player Two
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5), 5, Library.RGBColor.Green));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6), 5, Library.RGBColor.Green));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7), 5, Library.RGBColor.Green));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8), 5, Library.RGBColor.Green));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR12Extra, RGBButtonPin.RGBG12Extra, RGBButtonPin.RGBB12Extra, RGBButtonPin.RGBPB12Extra), 5, Library.RGBColor.Green));

            // Team Mode
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR4Extra, RGBButtonPin.RGBG4Extra, RGBButtonPin.RGBB4Extra, RGBButtonPin.RGBPB4Extra), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR12Extra, RGBButtonPin.RGBG12Extra, RGBButtonPin.RGBB12Extra, RGBButtonPin.RGBPB12Extra), 5, Library.RGBColor.Green));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15), 5, Library.RGBColor.Blue));
            TeamRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16), 5, Library.RGBColor.Green));

            RGBWS2811.Init();
            RGBWS2811.SetColorByRange(
               VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
               RGBColor.purple);
            RGBWS2811.Commit();
            GameTime.Start();
            LevelTime.Start();

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task.Run(() => ControlGame(_cts.Token));
            Task.Run(() => PlayerCatchingGame(_cts2.Token));
            Task.Run(() => ControlGameTiming(_cts3.Token));
            Task.Run(() => ControlRGBLightWhenGameIsOff(_cts4.Token));

            return Task.CompletedTask;
        }
        private async Task ControlRGBLightWhenGameIsOff(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus != GameStatus.Started)
                {
                    RGBWS2811.SetColorByRange(
                           VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
                           RGBColor.purple);
                    RGBWS2811.Commit();
                    inActiveGameRGBColor = NextColor(inActiveGameRGBColor);
                    Thread.Sleep(10000);
                }
            }

        }

        private RGBColor NextColor(RGBColor currentColor)
        {
            if ((int)currentColor < (int)RGBColor.Off)
                return (RGBColor)((int)currentColor + 1);
            else
                return (RGBColor)(0);
        }


        private async Task PlayerCatchingGame(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus == GameStatus.Started)
                {

                    if (VariableControlService.GameMode == GameMode.inTeam)
                    {
                        // @ToDo: Change the list name 
                        int index = 0;
                        foreach (var button in TeamRGBButtonList)
                        {
                            if (button.isPressed() == 1)
                            {

                                SuccessSound();
                                VariableControlService.Team.player[0].score += 10;
                                Console.WriteLine($"Player One {VariableControlService.Team.player[0].score} ============ index {index}");
                                Console.WriteLine($"pressed index {index}");
                            }
                            index++;
                        }
                    }
                    else if (VariableControlService.GameMode == GameMode.inWar)
                    {
                        foreach (var button in PlayerOneRGBButtonList)
                        {
                            if (button.isPressed() == 1)
                            {
                                SuccessSound();
                                VariableControlService.Team.player[0].score += 10;
                                Console.WriteLine($"Player One Press the Button,new Sore {VariableControlService.Team.player[0].score}");
                            }
                        }
                        foreach (var button in PlayerTwoRGBButtonList)
                        {
                            if (button.isPressed() == 1)
                            {
                                SuccessSound();
                                VariableControlService.Team.player[1].score += 10;
                                Console.WriteLine($"Player Two Press the Button,new Sore {VariableControlService.Team.player[1].score}");
                            }
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
                    gameFinishedButScoreNotSend = true;
                    if (VariableControlService.GameMode == GameMode.inTeam)
                    {
                        RGBWS2811.SetColorByRange(
                           VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
                           RGBColor.purple);
                        RGBWS2811.Commit();
                        GameTime.Restart();
                        while (GameTime.ElapsedMilliseconds < VariableControlService.GameTiming)
                        {
                            SelectRandomButton();
                            Thread.Sleep(2000);
                            ResetAllButton();
                        }
                        ResetAllButton();
                        if (VariableControlService.Team.player[0].score > VariableControlService.topScore.Score)
                        {
                            VariableControlService.topScore.Score = VariableControlService.Team.player[0].score;
                            VariableControlService.topScore.name = VariableControlService.Team.teamName;
                            LocalStorage.SaveData(VariableControlService.topScore, "data.json");
                        }
                        VariableControlService.GameStatus = GameStatus.Empty;
                    }
                    else if (VariableControlService.GameMode == GameMode.inWar)
                    {
                        VariableControlService.GameRound = Round.Round1;
                        GameTime.Restart();
                        while (VariableControlService.GameRound < Round.Round4)
                        {
                            PlayRoundSound(VariableControlService.GameRound);
                            LevelTime.Restart();
                            RGBWS2811.SetColorByRange(
                                VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
                                RGBColor.purple);
                            RGBWS2811.Commit();
                            while (LevelTime.ElapsedMilliseconds < VariableControlService.LevelTimeInSec * 1000)
                            {
                                SelectRandomButton();
                                Thread.Sleep(2000);
                                ResetAllButton();
                            }
                            // Check Who Ones 
                            if (VariableControlService.Team.player[0].score > VariableControlService.Team.player[1].score)
                            {
                                Console.WriteLine($"Round Number {VariableControlService.GameRound.ToString()} player One Win");
                                VariableControlService.Team.player[0].score = 0;
                                VariableControlService.Team.player[1].score = 0;
                                VariableControlService.Team.player[0].winNumber++;
                            }
                            else if (VariableControlService.Team.player[0].score < VariableControlService.Team.player[1].score)
                            {
                                Console.WriteLine($"Round Number {VariableControlService.GameRound.ToString()} player Two Win");
                                VariableControlService.Team.player[0].score = 0;
                                VariableControlService.Team.player[1].score = 0;
                                VariableControlService.Team.player[1].winNumber++;
                            }
                            else
                            {
                                Console.WriteLine($"Round Number {VariableControlService.GameRound.ToString()} Draw");
                                VariableControlService.Team.player[0].score = 0;
                                VariableControlService.Team.player[1].score = 0;
                                VariableControlService.Team.player[0].winNumber++;
                                VariableControlService.Team.player[1].winNumber++;
                            }
                            // Next Round
                            VariableControlService.GameRound = NextRound(VariableControlService.GameRound);
                        }
                        VariableControlService.GameStatus = GameStatus.Empty;
                    }

                }
                if (gameFinishedButScoreNotSend)
                {
                    gameFinishedButScoreNotSend = false;
                    bool teamNotAssigned = VariableControlService.Team.teamName == "" || VariableControlService.Team.teamName == null;

                    if (!teamNotAssigned && VariableControlService.GameMode == GameMode.inTeam)
                    {
                        SendScore();
                    }
                }
            }
        }

        private async Task ControlGameTiming(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                VariableControlService.CurrentTime = (int)GameTime.ElapsedMilliseconds;
            }
        }
        private void SelectRandomButton()
        {
            if (VariableControlService.GameMode == GameMode.inTeam)
            {
                int selectedButton = random.Next(0, TeamRGBButtonList.Count());
                TeamRGBButtonList[selectedButton].Activate(true);
            }
            else if (VariableControlService.GameMode == GameMode.inWar)
            {

                int numberOfBtnToSelect = random.Next(0, PlayerOneRGBButtonList.Count());
                for (int i = 0; i < numberOfBtnToSelect; i++)
                {
                    int selectedButton = random.Next(0, PlayerOneRGBButtonList.Count());
                    PlayerOneRGBButtonList[selectedButton].Activate(true);
                    selectedButton = random.Next(0, PlayerTwoRGBButtonList.Count());
                    PlayerTwoRGBButtonList[selectedButton].Activate(true);
                }
            }
        }

        private void ResetAllButton()
        {
            if (VariableControlService.GameMode == GameMode.inTeam)
                foreach (var button in TeamRGBButtonList)
                    button.Activate(false);
            else if (VariableControlService.GameMode == GameMode.inWar)
            {
                foreach (var button in PlayerOneRGBButtonList)
                    button.Activate(false);
                foreach (var button in PlayerTwoRGBButtonList)
                    button.Activate(false);
            }
        }
        private void PlayRoundSound(Round currentRound)
        {
            switch (currentRound)
            {
                case Round.Round1:
                    AudioPlayer.PIStartAudio(SoundType.RoundOne);
                    break;
                case Round.Round2:
                    AudioPlayer.PIStartAudio(SoundType.RoundTwo);
                    break;
                case Round.Round3:
                    AudioPlayer.PIStartAudio(SoundType.RoundThree); break;
                default:
                    break;

            }
        }
        private Round NextRound(Round currentRound)
        {
            return (Round)((int)currentRound + 1);
        }


        // === SEND SCORE 
        private async void SendScore()
        {
            Team team = new Team();
            team.Name = VariableControlService.Team.teamName;
            team.player = new List<Player>();
            team.player.Add(new Player { Id = "", FirstName = VariableControlService.Team.player[0].firstname, LastName = VariableControlService.Team.player[0].lastname });
            team.Total = VariableControlService.Team.player[0].score;
            var result = await APIIntegration.GetSignature("https://admin.frenziworld.com/api/make-signature", GameType.Catchy, team);
            await APIIntegration.SendScore("https://admin.frenziworld.com/api/game-score", result.Item1, result.Item2);
        }
        private void SuccessSound()
        {
            if (toggleSuccessSound)
                AudioPlayer.PIStartAudio(SoundType.Success);
            else
                AudioPlayer.PIStartAudio(SoundType.Wow);
            toggleSuccessSound = !toggleSuccessSound;
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
