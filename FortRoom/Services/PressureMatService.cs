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
        private readonly ILogger<PressureMatService> _logger;
        public PressureMatService(ILogger<PressureMatService> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool scoreJustDecreased = false;
            Stopwatch timer = new Stopwatch();

            while (!cancellationToken.IsCancellationRequested)
            {
                if (IsGameStartedOrInGoing())
                {
                    try
                    {
                        bool currentValue = MCP23Controller.Read(MasterDI.IN1);
                        VariableControlService.IsPressureMateActive = !currentValue;
                        if(!VariableControlService.IsPressureMateActive)
                            RGBLight.SetColor(VariableControlService.DefaultColor);

                        if (!currentValue && !scoreJustDecreased)
                        {
                            VariableControlService.TimeOfPressureHit++;
                            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.High);
                            AudioPlayer.PIStartAudio(SoundType.Descend);
                            RGBLight.SetColor(RGBColor.Red);
                            //RGBLight.TurnRGBColorDelayedASec(VariableControlService.DefaultColor, 3000);
                            scoreJustDecreased = true;
                            timer.Restart();
                            VariableControlService.TeamScore.FortRoomScore -= 20;

                        }
                        //previousValue = currentValue;
                        if (scoreJustDecreased && timer.ElapsedMilliseconds >= 3000)
                        {
                            scoreJustDecreased = false;
                            timer.Restart();
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error {ex.Message}");
                    }

                }
                Thread.Sleep(500);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Pressure Mat Stopped");
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
        }
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
    }
}
