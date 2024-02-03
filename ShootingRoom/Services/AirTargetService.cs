using Library;
using Library.AirTarget;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Diagnostics;
using System.Threading.Tasks;

namespace ShootingRoom.Services
{
    public class AirTargetService : IHostedService, IDisposable
    {
        List<AirTargetController> AirTargetList = new List<AirTargetController>();
        private CancellationTokenSource _cts, _cts2;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        int SlowPeriod = 3000;
        int MediumPeriod = 3000;

        int slowChangeTime = 1000;
        int mediumChangeTime = 700;
        int highChangeTime = 500;

        int changingSpeed = 1000;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR1, RGBButtonPin.RGBPB1));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR2, RGBButtonPin.RGBPB2));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR3, RGBButtonPin.RGBPB3));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR4, RGBButtonPin.RGBPB4));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR5, RGBButtonPin.RGBPB5));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR6, RGBButtonPin.RGBPB6));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR7, RGBButtonPin.RGBPB7));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR8, RGBButtonPin.RGBPB8));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR9, RGBButtonPin.RGBPB9));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR10, RGBButtonPin.RGBPB10));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR11, RGBButtonPin.RGBPB11));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR12, RGBButtonPin.RGBPB12));
            AirTargetList.Add(new AirTargetController(RGBButtonPin.RGBR12, RGBButtonPin.RGBPB12));

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            Task task2 = Task.Run(() => TimingService(_cts2.Token));
            return Task.WhenAll(task1, task2);
        }
        private async Task RunService(CancellationToken cancellationToken)
        {

            bool activeTarget = false;
            Stopwatch timer = new Stopwatch();
            Stopwatch timerToStart = new Stopwatch();
            int activeTargetIndox = -1;
            Random random = new Random();
            int randomTime = random.Next(5000, 15000);
            timerToStart.Restart();
            timer.Restart();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheGameStarted && IsTimerStarted)
                {
                    if (!activeTarget && timerToStart.ElapsedMilliseconds > randomTime)
                    {
                        activeTarget = true;
                        activeTargetIndox = random.Next(0, 8);
                        AirTargetList[activeTargetIndox].Select(true);
                        timer.Restart();
                        timerToStart.Restart();
                        randomTime = random.Next(5000, 15000);
                    }
                    if (activeTarget & timer.ElapsedMilliseconds >= changingSpeed)
                    {
                        AirTargetList[activeTargetIndox].Select(false);
                        activeTargetIndox = -1;
                        activeTarget = false;
                        timer.Restart();
                        timerToStart.Restart();
                    }
                    if (activeTarget && activeTargetIndox > -1)
                    {
                        bool isPreesed = AirTargetList[activeTargetIndox].Status();
                        if (isPreesed)
                        {
                            activeTarget = false;
                            AirTargetList[activeTargetIndox].Select(false);
                            JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
                            VariableControlService.ActiveTargetPressed++;
                            Console.WriteLine($"Score {VariableControlService.ActiveTargetPressed}");
                            activeTargetIndox = -1;
                            timerToStart.Restart();
                            timer.Restart();
                        }
                    }
                }
                // Sleep for a short duration to avoid excessive checking
                Thread.Sleep(10);
            }
        }
        private async Task TimingService(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {
                if (!IsTimerStarted)
                {
                    GameStopWatch.Start();
                    IsTimerStarted = true;
                }
            }
            while (true)
            {
                if (GameStopWatch.ElapsedMilliseconds > SlowPeriod && GameStopWatch.ElapsedMilliseconds < MediumPeriod)
                {
                    changingSpeed = mediumChangeTime;
                }
                else if (GameStopWatch.ElapsedMilliseconds > MediumPeriod)
                {
                    changingSpeed = highChangeTime;
                }
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