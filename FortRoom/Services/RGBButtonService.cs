using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;

namespace FortRoom.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private CancellationTokenSource _cts;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            //RGBButtonList.Add(new RGBButton());

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
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Green);
                        timer.Reset();
                        timerToStart.Reset();
                        randomTime = random.Next(5000, 15000);
                    }
                    if (activeButton & timer.ElapsedMilliseconds >= 10000)
                    {
                        RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
                        activeButtonIndox = -1;
                        activeButton = false;
                        timer.Reset();
                        timerToStart.Reset();
                    }
                    if (activeButton && activeButtonIndox > -1)
                    {
                        bool isPreesed = RGBButtonList[activeButtonIndox].CurrentStatus();
                        if (isPreesed)
                        {
                            activeButton = false;
                            RGBButtonList[activeButtonIndox].TurnColorOn(RGBColor.Off);
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
