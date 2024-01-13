using Library.RFIDLib;

namespace GatheringRoom.Services
{
    public class RFIDService : IHostedService, IDisposable
    {
        private CancellationTokenSource _cts;
        private RFID _rfidController =new();
        int pinReset = 6;
        int TestTest = 0;
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
                // Your service logic goes here
                // Check The RFID 
                if (_rfidController.CheckCardExisting() && Teams.player.Count<5)
                {
                    // Read Card Info .. 
                    Console.WriteLine("CArd Found .. ");
                }
                else {
                    //TestTest++;
                    //if (TestTest > 20)
                    //{
                    //    if(Teams.player.Count < 5)
                    //    Teams.player.Add($"Ahmad {TestTest}");
                    //    TestTest = 0;
                    //}
                }
                await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken); 
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
