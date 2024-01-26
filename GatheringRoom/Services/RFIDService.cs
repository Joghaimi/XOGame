using Library.RFIDLib;

namespace GatheringRoom.Services
{
    public class RFIDService : IHostedService, IDisposable
    {
        private CancellationTokenSource _cts;
        private RFID _rfidController = new();
        int pinReset = 6;
        bool stop = false;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _rfidController.Init(pinReset);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }

        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    //Console.WriteLine("Game Started");
                    // Your service logic goes here
                    // Check The RFID 
                    if (_rfidController.CheckCardExisting() && Teams.player.Count < 5 && !stop)
                    {
                        // Read Card Info .. 
                        Console.WriteLine("Card Found .. ");
                        string newPlayer = _rfidController.ReadCardInfo();
                        if (!string.IsNullOrEmpty(newPlayer) && !GatheringRoom.Teams.player.Any(item => item == newPlayer))
                        {
                            GatheringRoom.Teams.player.Add(newPlayer);
                        }
                    }
                    else
                    {
                        //TestTest++;
                        //if (TestTest > 20)
                        //{
                        //    if(Teams.player.Count < 5)
                        //    Teams.player.Add($"Ahmad {TestTest}");
                        //    TestTest = 0;
                        //}
                    }
                }
                else
                {

                    Console.WriteLine("Waitting .. ");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
        }

    }
}
