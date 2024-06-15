using Iot.Device.BrickPi3.Sensors;
using Library.GPIOLib;
using Library;
using Library.PinMapping;
using Library.RGBLib;
using System.Diagnostics;

namespace CatchyGame.Service
{
    public class SparkRGBScenario : IHostedService, IDisposable
    {

        Random random = new Random();
        Stopwatch GameTiming = new Stopwatch();
        Stopwatch LevelTime = new Stopwatch();
        private CancellationTokenSource _cts, _cts2, _cts3;
        List<SparkRGBButton> PlayerOneRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerTwoRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerThreeRGBButtonList = new List<SparkRGBButton>();
        List<SparkRGBButton> PlayerFourRGBButtonList = new List<SparkRGBButton>();

        List<Spike> SpikeButtonOne = new List<Spike>();
        List<Spike> SpikeButtonTwo = new List<Spike>();
        List<Spike> SpikeButtonThree = new List<Spike>();

        int delayTime = 800;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);


            // Rgbx
            SpikeButtonOne.Add(new Spike(VariableControlService.StripOneStartIndex, VariableControlService.StripOneStartIndex + 5, Library.RGBColor.Blue));
            SpikeButtonOne.Add(new Spike(VariableControlService.StripOneEndIndex - 4, VariableControlService.StripOneEndIndex, Library.RGBColor.Blue));


            // rgb1 
            SpikeButtonTwo.Add(new Spike(19, 24, Library.RGBColor.Blue));
            SpikeButtonTwo.Add(new Spike(242, 242 + 5, Library.RGBColor.Blue));
            SpikeButtonTwo.Add(new Spike(748, 748 + 5, Library.RGBColor.Blue));
            SpikeButtonTwo.Add(new Spike(748 - 5, 748, Library.RGBColor.Blue)); // Line End
            SpikeButtonTwo.Add(new Spike(242 - 5, 242, Library.RGBColor.Blue)); // Line End
            SpikeButtonTwo.Add(new Spike(19 - 5, 19, Library.RGBColor.Blue)); // Line End


            // rgb2
            SpikeButtonThree.Add(new Spike(475, 475 + 5, Library.RGBColor.Blue));
            SpikeButtonThree.Add(new Spike(945, 945 + 5, Library.RGBColor.Blue));
            SpikeButtonThree.Add(new Spike(1153, 1153 + 5, Library.RGBColor.Blue));
            SpikeButtonThree.Add(new Spike(475 - 5, 475, Library.RGBColor.Blue));
            SpikeButtonThree.Add(new Spike(945 - 5, 945, Library.RGBColor.Blue));
            SpikeButtonThree.Add(new Spike(1153 - 5, 1153, Library.RGBColor.Blue));



            // rgb3 
            //SpikeButtonOne.Add(new Spike(60, 60+ 5, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(520, 520 + 5, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(976, 976 + 5, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(60 - 5, 60, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(520 - 5, 520, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(976 - 5, 976, Library.RGBColor.Blue));



            //SpikeButtonOne.Add(new Spike(1099, 1099 + 5, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(784, 784 + 5, Library.RGBColor.Blue));
            //SpikeButtonOne.Add(new Spike(1137, 1137 + 5, Library.RGBColor.Blue));



            //SpikeButtonOne.Add(new Spike(VariableControlService.StripOneStartIndex, VariableControlService.StripOneStartIndex + 5, Library.RGBColor.Blue));

            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1), SpikeButtonThree, 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2), SpikeButtonOne, 5, Library.RGBColor.Green));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3), SpikeButtonTwo, 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR4Extra, RGBButtonPin.RGBG4Extra, RGBButtonPin.RGBB4Extra, RGBButtonPin.RGBPB4Extra), 5, Library.RGBColor.Green));
            PlayerThreeRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5), 5, Library.RGBColor.Green));
            PlayerThreeRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6), 5, Library.RGBColor.Green));
            PlayerFourRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7), 5, Library.RGBColor.Green));
            PlayerFourRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8), 5, Library.RGBColor.Green));
            PlayerThreeRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9), 5, Library.RGBColor.Blue));
            PlayerThreeRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10), 5, Library.RGBColor.Blue));
            PlayerFourRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11), 5, Library.RGBColor.Blue));
            PlayerFourRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR12Extra, RGBButtonPin.RGBG12Extra, RGBButtonPin.RGBB12Extra, RGBButtonPin.RGBPB12Extra), 5, Library.RGBColor.Green));
            PlayerOneRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15), 5, Library.RGBColor.Blue));
            PlayerTwoRGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16), 5, Library.RGBColor.Green));
            RGBWS2811.Init();

            RGBWS2811.SetColorByRange(
               VariableControlService.StripOneStartIndex, VariableControlService.StripSevenEndIndex,
               VariableControlService.StripSevenDefaultColor);
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
                        VariableControlService.Team.player[0].score += 10;
                        Console.WriteLine($"Player One {VariableControlService.Team.player[0].score}");
                    }


                }
                foreach (var button in PlayerTwoRGBButtonList)
                {
                    if (VariableControlService.Team.player.Count > 1)
                        if (button.isPressed() == 1)
                        {
                            VariableControlService.Team.player[1].score += 10;
                            Console.WriteLine($"Player One {VariableControlService.Team.player[1].score}");
                        }
                }
                foreach (var button in PlayerThreeRGBButtonList)
                {
                    if (VariableControlService.Team.player.Count > 2)

                        if (button.isPressed() == 1)
                        {
                            VariableControlService.Team.player[2].score += 10;
                            Console.WriteLine($"Player One {VariableControlService.Team.player[2].score}");
                        }
                }
                foreach (var button in PlayerFourRGBButtonList)
                {
                    if (VariableControlService.Team.player.Count > 3)
                        if (button.isPressed() == 1)
                        {
                            VariableControlService.Team.player[0].score += 10;
                            Console.WriteLine($"Player One {VariableControlService.Team.player[3].score}");
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
                        Thread.Sleep(delayTime);
                        ResetAllButton();
                    }
                    if (VariableControlService.GameRound == Round.Round3)
                    {
                        VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
                        delayTime = 800;
                    }
                    VariableControlService.GameRound = NextRound(VariableControlService.GameRound);

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
                VariableControlService.CurrentTime = (int)GameTiming.ElapsedMilliseconds;
                if (VariableControlService.GameStatus == GameStatus.Started)
                {
                    bool IsGameTimeFinished = GameTiming.ElapsedMilliseconds > VariableControlService.GameTiming;
                    bool GameFinishedByTimer = IsGameTimeFinished && VariableControlService.GameStatus == GameStatus.Started && VariableControlService.IsGameTimerStarted;
                    if (GameFinishedByTimer)
                        StopTheGame();
                }
            }
        }
        private void SelectRandomButton()
        {
            int numberOfButton = random.Next(1, 5);
            //Console.WriteLine($"numberOfButton {numberOfButton}");
            for (int i = 0; i < numberOfButton; i++)
            {
                int selectedButton = random.Next(0, 4);
                //Console.WriteLine($"selectedButton {selectedButton}");
                PlayerOneRGBButtonList[selectedButton].Activate(true);
                PlayerTwoRGBButtonList[selectedButton].Activate(true);
                PlayerThreeRGBButtonList[selectedButton].Activate(true);
                PlayerFourRGBButtonList[selectedButton].Activate(true);
            }

        }

        private void ResetAllButton()
        {
            foreach (var button in PlayerOneRGBButtonList)
                button.Activate(false);
            foreach (var button in PlayerTwoRGBButtonList)
                button.Activate(false);
            foreach (var button in PlayerThreeRGBButtonList)
                button.Activate(false);
            foreach (var button in PlayerFourRGBButtonList)
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
