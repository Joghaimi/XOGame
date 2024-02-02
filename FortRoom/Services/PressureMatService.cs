using Library;
using Library.GPIOLib;
using Library.Media;
using Library.RGBLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Numerics;

namespace FortRoom.Services
{
    public class PressureMatService : IHostedService, IDisposable
    {
        private int PressureMatPin = 7;
        private GPIOController _controller;
        private CancellationTokenSource _cts;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            _controller.Setup(PressureMatPin, PinMode.InputPullDown);

            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool currentValue = false;
            bool previousValue = false;
            bool scoreJustDecreased = false;
            Stopwatch timer = new Stopwatch();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    currentValue = _controller.Read(PressureMatPin);
                    if (currentValue && !previousValue)
                    {

                        VariableControlService.TimeOfPressureHit++;
                        JQ8400AudioModule.PlayAudio((int)SoundType.Beeb);
                        RGBLight.SetColor(RGBColor.Red);
                        scoreJustDecreased = true;
                        timer.Restart();
                        Console.WriteLine($"Pressure Mate Pressed {VariableControlService.TimeOfPressureHit}");
                    }
                    previousValue = currentValue;
                    if (scoreJustDecreased && timer.ElapsedMilliseconds >= 1000)
                    {

                        scoreJustDecreased = false;
                        JQ8400AudioModule.PlayAudio((int)SoundType.Start);
                        RGBLight.SetColor(RGBColor.Green);
                        timer.Restart();
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
