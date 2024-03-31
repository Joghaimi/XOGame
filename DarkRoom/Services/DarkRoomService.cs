using DarkRoom.Controllers;
using Library;
using Library.AirTarget;
using Library.DarkRoomSensor;
using Library.Display;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;
using System.Diagnostics;

namespace DarkRoom.Services
{
    public class DarkRoomService : IHostedService, IDisposable
    {
        private List<DarkRoomSensorController> DarkRoomSensorList = new List<DarkRoomSensorController>();
        private CancellationTokenSource _cts;
        Stopwatch GameStopWatch = new Stopwatch();
        private int Score = 0;

        DarkRoomSensor IN1 = new DarkRoomSensor(HatInputPin.IR1, -1, true);
        DarkRoomSensor IN2 = new DarkRoomSensor(HatInputPin.IR2, 2, false);
        DarkRoomSensor IN3 = new DarkRoomSensor(HatInputPin.IR3, -3, false);
        DarkRoomSensor IN4 = new DarkRoomSensor(HatInputPin.IR4, -4, false);
        DarkRoomSensor IN5 = new DarkRoomSensor(HatInputPin.IR5, 5, false);
        DarkRoomSensor IN6 = new DarkRoomSensor(HatInputPin.IR6, -6, false);
        DarkRoomSensor IN7 = new DarkRoomSensor(HatInputPin.IR7, -7, false);
        DarkRoomSensor IN8 = new DarkRoomSensor(HatInputPin.IR8, 8, false);
        DarkRoomSensor IN9 = new DarkRoomSensor(HatInputPin.IR9, -9, false);
        DarkRoomSensor IN10 = new DarkRoomSensor(HatInputPin.IR10, 10, false);
        DarkRoomSensor IN11 = new DarkRoomSensor(HatInputPin.IR11, -11, false);
        DarkRoomSensor IN12 = new DarkRoomSensor(HatInputPin.IR12, 12, true);


        public Task StartAsync(CancellationToken cancellationToken)
        {
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN1));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN2));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN3));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN4));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN5));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN6));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN7));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN8));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN9));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN10));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN11));
            DarkRoomSensorList.Add(new DarkRoomSensorController(IN12));
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;

        }
        private async Task RunService(CancellationToken cancellationToken)
        {

            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    //Console.WriteLine($"Is Game Started {VariableControlService.IsTheGameStarted}");
                    int i = 0;
                    foreach (var sensor in DarkRoomSensorList)
                    {
                        if (!IsGameStartedOrInGoing())
                            break;
                        bool status = sensor.stauts();
                        if (status)
                        {
                            Console.WriteLine($"Sensor #{i}");
                            int addedScore = sensor.sensor.Score;
                            Score += addedScore;
                            if (addedScore > 0)
                            {
                                RGBLight.SetColor(RGBColor.Blue);
                                RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                                AudioPlayer.PIStartAudio(SoundType.Bonus);
                                sensor.BlockScoreFor1Sec();
                                Console.WriteLine($"Scored, Total {Score}");

                            }
                            else
                            {
                                sensor.BlockScoreFor1Sec();
                                RGBLight.SetColor(RGBColor.Red);
                                RGBLight.TurnRGBColorDelayedASec(RGBColor.White);
                                AudioPlayer.PIStartAudio(SoundType.Descend);
                                Console.WriteLine($"Scored Wrong, Total {Score}");
                            }

                        }

                    }
                }



                Thread.Sleep(100);
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
        private void Reset()
        {
            Score = 0;
        }
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
    }
}
