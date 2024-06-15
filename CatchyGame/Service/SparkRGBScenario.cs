namespace CatchyGame.Service
{
    public class SparkRGBScenario : IHostedService, IDisposable
    {



        private CancellationTokenSource _cts;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlGame(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task ControlGame(CancellationToken cancellationToken)
        {
        }




        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
