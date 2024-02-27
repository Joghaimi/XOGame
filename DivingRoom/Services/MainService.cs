﻿using Iot.Device.Mcp3428;
using Library;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;

namespace DivingRoom.Services
{

    // The Main Part of the game

    public class MainService : IHostedService, IDisposable
    {

        private GPIOController _controller;
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
            //JQ8400AudioModule.init(SerialPort.Serial2);
            MCP23Controller.Init(true);
            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);

            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinMode.Output);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT6.Chip, MasterOutputPin.OUTPUT6.port, MasterOutputPin.OUTPUT6.PinNumber, PinState.Low);
            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(MasterDI.PIRPin1);
                PIR2 = _controller.Read(MasterDI.PIRPin2);
                PIR3 = _controller.Read(MasterDI.PIRPin3);
                PIR4 = _controller.Read(MasterDI.PIRPin4);
                //Console.WriteLine($"PIR Status PIR1:{PIR1} PIR2:{PIR2} PIR3:{PIR3} PIR4:{PIR4}");
                //VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;
                //MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.Low);


                bool DoneOneTimeFlage = false;
                //if (VariableControlService.IsTheGameStarted)
                //{
                //    if (!DoneOneTimeFlage)
                //    {
                //        // Turn the Light Green
                //        //RGBLight.SetColor(RGBColor.Green);
                //        //JQ8400AudioModule.PlayAudio((int)SoundType.Beeb);
                //        DoneOneTimeFlage = true;
                //    }
                //}

                Thread.Sleep(1000);
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