using Iot.Device.Mcp3428;
using Library;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
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
        private CancellationTokenSource _cts;
        private readonly ILogger<ObstructionControlService> _logger;
        public PressureMatService(ILogger<ObstructionControlService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            MCP23Controller.PinModeSetup(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber, PinMode.Input);

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
                try
                {
                    if (VariableControlService.IsTheGameStarted)
                    {
                        currentValue = MCP23Controller.Read(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber);
                        if (!currentValue && !scoreJustDecreased)
                        {
                            VariableControlService.TimeOfPressureHit++;
                            MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.High);
                            //AudioPlayer.PIStartAudio(SoundType.Descend);
                            RGBLight.SetColor(RGBColor.Red);
                            RGBLight.TurnRGBOffAfter1Sec();
                            scoreJustDecreased = true;
                            timer.Restart();
                            Console.WriteLine($"Pressure Mate Pressed {VariableControlService.TimeOfPressureHit}");
                        }
                        //previousValue = currentValue;
                        if (scoreJustDecreased && timer.ElapsedMilliseconds >= 3000)
                        {
                            scoreJustDecreased = false;
                            timer.Restart();
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error {ex.Message}");
                }
                //Console.Write($"Is Pressed {!currentValue} !scoreJustDecreased {!scoreJustDecreased} ==> {!currentValue && !scoreJustDecreased} Timer =====>{timer.ElapsedMilliseconds}");
                Thread.Sleep(500);
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
