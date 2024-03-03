using Library;
using Library.AirTarget;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;
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

        int Score = 0;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            GameStopWatch.Start();
            // TO DO Init The RGB Light .. 
            AirTargetList.Add(new AirTargetController(MasterOutputPin.OUTPUT1, HatInputPin.IR1, HatInputPin.IR2, HatInputPin.IR3, HatInputPin.IR4, HatInputPin.IR5));
            AirTargetList.Add(new AirTargetController(MasterOutputPin.OUTPUT2, HatInputPin.IR6, HatInputPin.IR7, HatInputPin.IR8, HatInputPin.IR9, HatInputPin.IR10));
            AirTargetList.Add(new AirTargetController(MasterOutputPin.OUTPUT3, HatInputPin.IR11, HatInputPin.IR12, HatInputPin.IR13, HatInputPin.IR14, HatInputPin.IR15));
            AirTargetList.Add(new AirTargetController(MasterOutputPin.OUTPUT4, HatInputPin.IR16, HatInputPin.IR17, HatInputPin.IR18, HatInputPin.IR19, HatInputPin.IR20));

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
            //_cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //Task task1 = Task.Run(() => RunService(_cts.Token));
            //Task task2 = Task.Run(() => TimingService(_cts2.Token));
            //Task.Run(() => RunService(_cts.Token));
            //Task.CompletedTask;
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

                int index = 0;
                foreach (var item in AirTargetList)
                {
                    GameStopWatch.Restart();
                    item.Select(true);
                    while (timer.ElapsedMilliseconds >= 30000)
                    {
                        if (item.TargetOneStatus())
                        {
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            Score++;
                        }
                        if (item.TargetTwoStatus())
                        {
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            Score++;
                        }
                        if (item.TargetThreeStatus())
                        {
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            Score++;
                        }
                        if (item.TargetFourStatus())
                        {
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            Score++;
                        }
                        if (item.TargetFiveStatus())
                        {
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            Score++;
                        }
                        Thread.Sleep(10);
                    }
                    Console.WriteLine(Score);
                    item.Select(false);
                }

                //if (VariableControlService.IsTheGameStarted && IsTimerStarted)
                //{
                //    if (!activeTarget && timerToStart.ElapsedMilliseconds > randomTime)
                //    {
                //        activeTarget = true;
                //        activeTargetIndox = random.Next(0, 8);
                //        AirTargetList[activeTargetIndox].Select(true);
                //        timer.Restart();
                //        timerToStart.Restart();
                //        randomTime = random.Next(5000, 15000);
                //    }
                //    if (activeTarget & timer.ElapsedMilliseconds >= changingSpeed)
                //    {
                //        AirTargetList[activeTargetIndox].Select(false);
                //        activeTargetIndox = -1;
                //        activeTarget = false;
                //        timer.Restart();
                //        timerToStart.Restart();
                //    }
                //    if (activeTarget && activeTargetIndox > -1)
                //    {
                //        bool isPreesed = AirTargetList[activeTargetIndox].Status();
                //        if (isPreesed)
                //        {
                //            activeTarget = false;
                //            AirTargetList[activeTargetIndox].Select(false);
                //            JQ8400AudioModule.PlayAudio((int)SoundType.Bonus);
                //            VariableControlService.ActiveTargetPressed++;
                //            Console.WriteLine($"Score {VariableControlService.ActiveTargetPressed}");
                //            activeTargetIndox = -1;
                //            timerToStart.Restart();
                //            timer.Restart();
                //        }
                //    }
                //}
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