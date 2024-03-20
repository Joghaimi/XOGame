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
        private DisplayController display;
        private CancellationTokenSource _cts, _cts2;
        Stopwatch GameStopWatch = new Stopwatch();
        private int Score = 0;

        DarkRoomSensor IN1 = new DarkRoomSensor(HatInputPin.IR1, -1);
        DarkRoomSensor IN2 = new DarkRoomSensor(HatInputPin.IR2, 2);
        DarkRoomSensor IN3 = new DarkRoomSensor(HatInputPin.IR3, -3);
        DarkRoomSensor IN4 = new DarkRoomSensor(HatInputPin.IR4, -4);
        DarkRoomSensor IN5 = new DarkRoomSensor(HatInputPin.IR5, 5);
        DarkRoomSensor IN6 = new DarkRoomSensor(HatInputPin.IR6, -6);
        DarkRoomSensor IN7 = new DarkRoomSensor(HatInputPin.IR7, -7);
        DarkRoomSensor IN8 = new DarkRoomSensor(HatInputPin.IR8, 8);
        DarkRoomSensor IN9 = new DarkRoomSensor(HatInputPin.IR9, -9);
        DarkRoomSensor IN10 = new DarkRoomSensor(HatInputPin.IR10, 10);
        DarkRoomSensor IN11 = new DarkRoomSensor(HatInputPin.IR11, -11);
        DarkRoomSensor IN12 = new DarkRoomSensor(HatInputPin.IR12, 12);


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
                foreach (var sensor in DarkRoomSensorList)
                {
                    (bool state, int addedScore) = sensor.SensorStatus();
                    if (state)
                    {
                        Score += addedScore;
                        if (addedScore > 0)
                        {
                            RGBLight.SetColor(RGBColor.Blue);
                            RGBLight.TurnRGBColorDelayed(RGBColor.White);
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            sensor.BlockScoreFor1Sec();
                            Console.WriteLine($"Scored, Total {Score}");

                        }
                        else
                        {
                            sensor.BlockScoreFor1Sec();
                            RGBLight.SetColor(RGBColor.Red);
                            RGBLight.TurnRGBColorDelayed(RGBColor.White);
                            AudioPlayer.PIStartAudio(SoundType.Descend);
                            Console.WriteLine($"Scored Wrong, Total {Score}");
                        }
                    }
                }
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
