using Library;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.RGBLib;
using System.Device.Gpio;

namespace FortRoom.Services
{
    public class MainService : IHostedService, IDisposable
    {

        private GPIOController _controller;
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private int PIRPin1 = 26;
        private int PIRPin2 = 19;
        private int PIRPin3 = 13;
        private int PIRPin4 = 6;





        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.init();
            JQ8400AudioModule.init(SerialPortMapping.PortMap["Serial2"]);

            // Init the Pin's
            _controller.Setup(PIRPin1, PinMode.InputPullDown);
            _controller.Setup(PIRPin3, PinMode.InputPullDown);
            _controller.Setup(PIRPin4, PinMode.InputPullDown);
            _controller.Setup(PIRPin2, PinMode.InputPullDown);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(PIRPin1);
                PIR2 = _controller.Read(PIRPin2);
                PIR3 = _controller.Read(PIRPin3);
                PIR4 = _controller.Read(PIRPin4);
                //Console.WriteLine($"PIR Status PIR1:{PIR1} PIR2:{PIR2} PIR3:{PIR3} PIR4:{PIR4}");
                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
                bool DoneOneTimeFlage = false;
                if (VariableControlService.IsTheGameStarted)
                {
                    if (DoneOneTimeFlage)
                    {
                        // Turn the Light Green
                        RGBLight.TurnColorOn(RGBColor.Green);
                        JQ8400AudioModule.PlayAudio((int)SoundType.Start);
                        DoneOneTimeFlage = true;
                    }
                    else
                    {
                        // Activate RBG 



                    }



                }



                //// One Time Thing!
                //if (isAnyOfRIPSensorActive && !isTheirAreSomeOneInTheRoom)
                //{
                //    // Turn the Light on 

                //    // rise a flag 
                //    isTheirAreSomeOneInTheRoom = true;
                //}
                // Sleep for a short duration to avoid excessive checking
                Thread.Sleep(10);
                //await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
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
