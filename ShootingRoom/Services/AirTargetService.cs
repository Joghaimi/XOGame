using Library.AirTarget;
using Library.RGBLib;
using System.Diagnostics;

namespace ShootingRoom.Services
{
    public class AirTargetService : IHostedService, IDisposable
    {
        List<AirTargetController> AirTargetList = new List<AirTargetController>();
        private CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            //AirTargetList.Add(new RGBButton());

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool activeButton = false;
            Stopwatch timer = new Stopwatch();
            Stopwatch timerToStart = new Stopwatch();
            int activeButtonIndox = -1;
            Random random = new Random();
            int randomTime = random.Next(5000, 15000);
            timerToStart.Start();
            timer.Start();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    if (!activeButton && timerToStart.ElapsedMilliseconds > randomTime)
                    {
                        activeButton = true;
                        activeButtonIndox = random.Next(0, 8);
                        AirTargetList[activeButtonIndox].Select(true);
                        timer.Reset();
                        timerToStart.Reset();
                        randomTime = random.Next(5000, 15000);
                    }
                    if (activeButton & timer.ElapsedMilliseconds >= 10000)
                    {
                        AirTargetList[activeButtonIndox].Select(false);
                        activeButtonIndox = -1;
                        activeButton = false;
                        timer.Reset();
                        timerToStart.Reset();
                    }
                    if (activeButton && activeButtonIndox > -1)
                    {
                        bool isPreesed = AirTargetList[activeButtonIndox].Status();
                        if (isPreesed)
                        {
                            activeButton = false;
                            AirTargetList[activeButtonIndox].Select(false);
                            VariableControlService.ActiveButtonPressed++;
                            activeButtonIndox = -1;
                            timerToStart.Reset();
                            timer.Reset();
                        }
                    }





                }
                // Sleep for a short duration to avoid excessive checking
                Thread.Sleep(10);
            }
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
