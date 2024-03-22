using Iot.Device.Mcp3428;
using Library;
using Library.Enum;
using Library.GPIOLib;
using Library.Media;
using Library.PinMapping;
using Library.RGBLib;
using System.Device.Gpio;

namespace FortRoom.Services
{

    // The Main Part of the game

    public class MainService : IHostedService, IDisposable
    {

        private GPIOController _controller;
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts, cts2;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data, Room.Fort);
            AudioPlayer.Init(Room.Fort);
            MCP23Controller.Init(Room.Fort);
            // Init the Pin's
            _controller.Setup(MasterDI.PIRPin1, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin2, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin3, PinMode.InputPullDown);
            _controller.Setup(MasterDI.PIRPin4, PinMode.InputPullDown);
            //AudioPlayer.PIBackgroundSound(SoundType.Background);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT6, PinMode.Output);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            Task.Run(() => CheckIFRoomIsEmpty(cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            bool lastRoomState = false;
            RGBLight.SetColor(RGBColor.Red);
            MCP23Controller.Write(MasterOutputPin.OUTPUT6, PinState.Low);
            while (!cancellationToken.IsCancellationRequested)
            {
                PIR1 = _controller.Read(MasterDI.PIRPin1);
                PIR2 = _controller.Read(MasterDI.PIRPin2);
                PIR3 = _controller.Read(MasterDI.PIRPin3);
                PIR4 = _controller.Read(MasterDI.PIRPin4);
                //Console.WriteLine($"PIR Status PIR1:{PIR1} PIR2:{PIR2} PIR3:{PIR3} PIR4:{PIR4}");
                VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4 || VariableControlService.IsTheirAnyOneInTheRoom;

                //MCP23Controller.Write(MasterOutputPin.OUTPUT8.Chip, MasterOutputPin.OUTPUT8.port, MasterOutputPin.OUTPUT8.PinNumber, PinState.Low);
                //if (VariableControlService.IsTheirAnyOneInTheRoom != lastRoomState)
                //{
                //    lastRoomState = VariableControlService.IsTheirAnyOneInTheRoom;
                //    if (VariableControlService.IsTheirAnyOneInTheRoom)
                //        RGBLight.SetColor(RGBColor.Green);
                //    else
                //        RGBLight.SetColor(RGBColor.Red);

                //}
                //Thread.Sleep(1000);
                bool DoneOneTimeFlage = false;
                if (VariableControlService.IsTheGameStarted)
                {
                    if (!DoneOneTimeFlage)
                    {
                        // Turn the Light Green
                        //RGBLight.SetColor(RGBColor.Green);
                        //JQ8400AudioModule.PlayAudio((int)SoundType.Beeb);
                        DoneOneTimeFlage = true;
                    }
                }

                Thread.Sleep(1000);
            }
        }
        private async Task CheckIFRoomIsEmpty(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.IsTheirAnyOneInTheRoom && !VariableControlService.IsTheGameStarted)
                {
                    VariableControlService.IsTheirAnyOneInTheRoom = PIR1 || PIR2 || PIR3 || PIR4;
                    Thread.Sleep(10000);
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
