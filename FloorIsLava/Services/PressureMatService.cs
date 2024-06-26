﻿using Library;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualBasic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Numerics;

namespace FloorIsLava.Services
{
    public class PressureMatService : IHostedService, IDisposable
    {
        private CancellationTokenSource _cts;


        public Task StartAsync(CancellationToken cancellationToken)
        {
            MCP23Controller.PinModeSetup(MasterDI.IN8, PinMode.Input);

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
                    currentValue = !MCP23Controller.Read(MasterDI.IN8, previousValue);
                    if (currentValue && !scoreJustDecreased)
                    {

                        VariableControlService.TimeOfPressureHit++;
                        JQ8400AudioModule.PlayAudio((int)SoundType.Descend);
                        RGBLight.SetColor(RGBColor.Red);
                        scoreJustDecreased = true;
                        timer.Restart();
                        Console.WriteLine($"Pressure Mate Pressed {VariableControlService.TimeOfPressureHit}");
                    }
                    //previousValue = currentValue;
                    if (scoreJustDecreased && timer.ElapsedMilliseconds >= 3000)
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
