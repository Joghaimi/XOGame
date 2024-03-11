using System.Diagnostics;
using Library.RGBLib;
using Library;
using Library.Media;
using Library.PinMapping;
using Library.GPIOLib;
using System.Device.Gpio;
using Iot.Device.Mcp3428;

namespace FloorIsLava.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private CancellationTokenSource _cts, _cts2;
        bool IN4, IN2, IN3, IN5, IN6 = false;
        bool PressureMatPressed = false;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        bool pressureMAtCount = false;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            // Init RGB
            //RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
            Console.WriteLine("Init Button#1");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
            RGBButtonList[0].TurnColorOn(RGBColor.Off);
            MCP23Controller.PinModeSetup(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN4.Chip, MasterDI.IN4.port, MasterDI.IN4.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN5.Chip, MasterDI.IN5.port, MasterDI.IN5.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN7.Chip, MasterDI.IN7.port, MasterDI.IN7.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinMode.Output);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT4.Chip, MasterOutputPin.OUTPUT4.port, MasterOutputPin.OUTPUT4.PinNumber, PinMode.Output);
            GameStopWatch.Start();
            AudioPlayer.PIBackgroundSound(SoundType.Background);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            //Task task2 = Task.Run(() => PressureMat(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            Console.WriteLine("Main");
            RGBLight.SetColor(RGBColor.Red);

            //// tEST
            //MCP23Controller.Write(MasterOutputPin.OUTPUT4.Chip, MasterOutputPin.OUTPUT4.port, MasterOutputPin.OUTPUT4.PinNumber, PinState.High);
            //while (true)
            //{
            //    if (!MCP23Controller.Read(MasterDI.IN5.Chip, MasterDI.IN5.port, MasterDI.IN5.PinNumber) && !IN5)
            //    {
            //        IN5 = true;
            //        AudioPlayer.PIStartAudio(SoundType.Bonus);
            //        RGBLight.SetColor(RGBColor.Blue);
            //        RGBLight.TurnRGBRedDelayed();
            //        Console.WriteLine("IN5 PRESSED ====");
            //    }
            //    if (!MCP23Controller.Read(MasterDI.IN7.Chip, MasterDI.IN7.port, MasterDI.IN7.PinNumber) && !IN6)
            //    {
            //        IN6 = true;
            //        AudioPlayer.PIStartAudio(SoundType.Bonus);
            //        RGBLight.SetColor(RGBColor.Blue);
            //        RGBLight.TurnRGBRedDelayed();
            //        Console.WriteLine("IN6 PRESSED ====");

            //    }
            //    if (IN5)
            //    {
            //        IN5 = !MCP23Controller.Read(MasterDI.IN5.Chip, MasterDI.IN5.port, MasterDI.IN5.PinNumber);
            //        if(!IN5)
            //            Console.WriteLine("IN5 bREAK ====");

            //    }
            //    if (IN6)
            //    {
            //        IN6 = !MCP23Controller.Read(MasterDI.IN7.Chip, MasterDI.IN7.port, MasterDI.IN7.PinNumber);
            //        if(!IN6)
            //            Console.WriteLine("IN6 bREAK ====");

            //    }

            //    //if (IN6 || IN5)
            //    //    break;

            //}
            //// tEST

            while (true)
            {


                if (!MCP23Controller.Read(MasterDI.IN2.Chip, MasterDI.IN2.port, MasterDI.IN2.PinNumber) && !IN2)
                {
                    IN2 = true;
                    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBRedDelayed();
                    Console.WriteLine("====");
                }
                if (!MCP23Controller.Read(MasterDI.IN3.Chip, MasterDI.IN3.port, MasterDI.IN3.PinNumber) && !IN3)
                {
                    IN3 = true;
                    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBRedDelayed();
                    Console.WriteLine("====");

                }
                if (!MCP23Controller.Read(MasterDI.IN4.Chip, MasterDI.IN4.port, MasterDI.IN4.PinNumber) && !IN4)
                {
                    IN4 = true;
                    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBRedDelayed();
                    Console.WriteLine("====");

                }
                pressureMateTest();
                if (IN2 && IN3 && IN4)
                {
                    Console.WriteLine("Pressed all 3");
                    RGBButtonList[0].TurnColorOn(RGBColor.Red);
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.High);
                    pressureMAtCount = true;
                    while (RGBButtonList[0].CurrentStatus() || PressureMatPressed)
                    {
                        pressureMateTest();

                        Thread.Sleep(10);
                    }
                    pressureMAtCount = false;

                    Console.WriteLine("Button Pressed");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.Low);
                    RGBButtonList[0].TurnColorOn(RGBColor.Blue);
                    RGBLight.SetColor(RGBColor.Blue);
                    RGBLight.TurnRGBRedDelayed();
                    MCP23Controller.Write(MasterOutputPin.OUTPUT4.Chip, MasterOutputPin.OUTPUT4.port, MasterOutputPin.OUTPUT4.PinNumber, PinState.High);
                    while (true)
                    {
                        if (!MCP23Controller.Read(MasterDI.IN5.Chip, MasterDI.IN5.port, MasterDI.IN5.PinNumber) && !IN5)
                        {
                            IN5 = true;
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            RGBLight.SetColor(RGBColor.Blue);
                            if (!IN6)
                                RGBLight.TurnRGBRedDelayed();
                            Console.WriteLine("IN5 PRESSED ====");
                        }
                        if (IN5)
                        {
                            IN5 = !MCP23Controller.Read(MasterDI.IN5.Chip, MasterDI.IN5.port, MasterDI.IN5.PinNumber);
                            if (!IN5)
                                Console.WriteLine("IN5 bREAK ====");

                        }
                        if (!MCP23Controller.Read(MasterDI.IN7.Chip, MasterDI.IN7.port, MasterDI.IN7.PinNumber) && !IN6)
                        {
                            IN6 = true;
                            AudioPlayer.PIStartAudio(SoundType.Bonus);
                            RGBLight.SetColor(RGBColor.Blue);
                            if (!IN5)
                                RGBLight.TurnRGBRedDelayed();
                            Console.WriteLine("IN6 PRESSED ====");

                        }

                        if (IN6)
                        {
                            IN6 = !MCP23Controller.Read(MasterDI.IN7.Chip, MasterDI.IN7.port, MasterDI.IN7.PinNumber);
                            if (!IN6)
                                Console.WriteLine("IN6 bREAK ====");

                        }
                        if (IN6 && IN5)
                            break;

                    }
                    Console.WriteLine("Game Endded");

                    RGBLight.SetColor(RGBColor.Blue);

                    //MCP23Controller.Write(MasterOutputPin.OUTPUT4.Chip, MasterOutputPin.OUTPUT4.port, MasterOutputPin.OUTPUT4.PinNumber, PinState.Low);
                    AudioPlayer.PIStartAudio(SoundType.Finish);
                    IN2 = false;
                    IN3 = false;
                    IN4 = false;
                    break;
                }
                Thread.Sleep(10);


            }
        }
        bool justDecrease = false;
        private void pressureMateTest()
        {

            bool currentValue = false;
            currentValue = MCP23Controller.Read(MasterDI.IN1.Chip, MasterDI.IN1.port, MasterDI.IN1.PinNumber);
            if (!currentValue && !justDecrease)
            {
                justDecrease = true;
                GameStopWatch.Restart();
                Console.WriteLine("Pressure mat Pressed");
                AudioPlayer.PIStartAudio(SoundType.Descend);
                PressureMatPressed = true;
                if (pressureMAtCount)
                {
                    RGBButtonList[0].TurnColorOn(RGBColor.Off);
                    Thread.Sleep(15000);
                    RGBButtonList[0].TurnColorOn(RGBColor.Red);
                }
                PressureMatPressed = false;
            }
            if (justDecrease && GameStopWatch.ElapsedMilliseconds > 3000)
            {

                justDecrease = false;
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            AudioPlayer.PIStopAudio();

            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            //_cts.Dispose();
        }
    }
}
