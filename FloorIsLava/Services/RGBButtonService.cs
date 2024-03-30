﻿using System.Diagnostics;
using Library.RGBLib;
using Library;
using Library.Media;
using Library.PinMapping;
using Library.GPIOLib;
using System.Device.Gpio;
using Iot.Device.Mcp3428;
using static IronPython.Modules._ast;
using System;

namespace FloorIsLava.Services
{
    public class RGBButtonService : IHostedService, IDisposable
    {
        List<RGBButton> RGBButtonList = new List<RGBButton>();
        private CancellationTokenSource _cts, _cts2;
        bool IN4, IN2, IN3, IN5, IN7 = false;
        bool PressureMatPressed = false;
        bool IsTimerStarted = false;
        Stopwatch GameStopWatch = new Stopwatch();
        Stopwatch MotorStopWatch = new Stopwatch();
        bool pressureMAtCount = false;
        bool ceilingMotorDown = false;
        bool ceilingMotoruUp = false;
        int numberOfPressedMotor = 0;
        long motorTiming = 0;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            // Init RGB
            Console.WriteLine("Init Button#1");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
            RGBButtonList[0].TurnColorOn(RGBColor.Off);

            MCP23Controller.PinModeSetup(MasterDI.IN1, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN2, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN4, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN5, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN7, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT1, PinMode.Output);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT5, PinMode.Output);
            MCP23Controller.PinModeSetup(MasterOutputPin.OUTPUT4, PinMode.Output);
            GameStopWatch.Start();
            MotorStopWatch.Start();
            CellingDirection(true, 10000);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService(_cts.Token));
            Task task2 = Task.Run(() => StopMotorAfter(_cts2.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {

            RGBLight.SetColor(RGBColor.Red);
            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    if (!IsGameStartedOrInGoing())
                        break;

                    if (!IN2)
                        IN2 = CeilingButton(!MCP23Controller.Read(MasterDI.IN2));
                    if (!IN3)
                        IN3 = CeilingButton(!MCP23Controller.Read(MasterDI.IN3));
                    if (!IN4)
                        IN4 = CeilingButton(!MCP23Controller.Read(MasterDI.IN4));
                    pressureMat();

                    // Test 
                    if (IN2 && IN3 && IN4 && !ceilingMotorDown)
                    {
                        Console.WriteLine("Pressed all 3");
                        RGBButtonList[0].TurnColorOn(RGBColor.Red);
                        pressureMAtCount = true;
                        while (RGBButtonList[0].CurrentStatus() || PressureMatPressed)
                        {
                            pressureMat();
                            Thread.Sleep(10);
                        }
                        motorTiming = MotorStopWatch.ElapsedMilliseconds + 15000;
                        ceilingMotoruUp = true;

                        pressureMAtCount = false;
                        Console.WriteLine("Button Pressed");

                        RGBButtonList[0].TurnColorOn(RGBColor.Blue);
                        RGBLight.SetColor(RGBColor.Blue);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                        Console.WriteLine("Magnet Start");
                        MCP23Controller.Write(MasterOutputPin.OUTPUT4, PinState.High);
                        while (true)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            pressureMat();
                            if (!MCP23Controller.Read(MasterDI.IN5) && !IN5)
                            {
                                IN5 = true;
                                AudioPlayer.PIStartAudio(SoundType.Charge);
                                RGBLight.SetColor(RGBColor.Blue);
                                if (!IN7)
                                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                                Console.WriteLine("IN5 PRESSED ====");
                            }
                            pressureMat();
                            if (IN5)
                            {
                                IN5 = !MCP23Controller.Read(MasterDI.IN5);
                                if (!IN5)
                                    Console.WriteLine("IN5 bREAK ====");

                            }
                            if (!MCP23Controller.Read(MasterDI.IN7) && !IN7)
                            {
                                IN7 = true;
                                AudioPlayer.PIStartAudio(SoundType.Charge);
                                RGBLight.SetColor(RGBColor.Blue);
                                if (!IN5)
                                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                                Console.WriteLine("IN6 PRESSED ====");

                            }
                            pressureMat();
                            if (IN7)
                            {
                                IN7 = !MCP23Controller.Read(MasterDI.IN7);
                                if (!IN7)
                                    Console.WriteLine("IN6 bREAK ====");

                            }
                            if (IN7 && IN5)
                                break;

                        }
                        Console.WriteLine("Game Ended");
                        RGBLight.SetColor(RGBColor.Blue);
                        Console.WriteLine("Magnet Stop");

                        MCP23Controller.Write(MasterOutputPin.OUTPUT4, PinState.Low);
                        AudioPlayer.PIStopAudio();
                        Thread.Sleep(300);
                        AudioPlayer.PIStartAudio(SoundType.Finish);
                        IN2 = false;
                        IN3 = false;
                        IN4 = false;
                        break;
                    }
















                    //if (!MCP23Controller.Read(MasterDI.IN2) && !IN2)
                    //{
                    //    IN2 = true;
                    //    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //    RGBLight.SetColor(RGBColor.Blue);
                    //    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //    Console.WriteLine("====");
                    //    if (!ceilingMotorDown)
                    //    {
                    //        motorTiming = MotorStopWatch.ElapsedMilliseconds;
                    //        MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
                    //    }
                    //    motorTiming += 3000;
                    //    numberOfPressedMotor++;
                    //    ceilingMotorDown = true;

                    //}
                    //if (!MCP23Controller.Read(MasterDI.IN3) && !IN3)
                    //{
                    //    IN3 = true;
                    //    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //    RGBLight.SetColor(RGBColor.Blue);
                    //    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //    Console.WriteLine("====");
                    //    if (!ceilingMotorDown)
                    //    {
                    //        motorTiming = MotorStopWatch.ElapsedMilliseconds;
                    //        MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
                    //    }
                    //    numberOfPressedMotor++;

                    //    motorTiming += 3000;
                    //    ceilingMotorDown = true;

                    //}
                    //if (!MCP23Controller.Read(MasterDI.IN4) && !IN4)
                    //{
                    //    IN4 = true;
                    //    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //    RGBLight.SetColor(RGBColor.Blue);
                    //    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //    Console.WriteLine("====");
                    //    if (!ceilingMotorDown)
                    //    {
                    //        motorTiming = MotorStopWatch.ElapsedMilliseconds;
                    //        MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
                    //    }
                    //    numberOfPressedMotor++;
                    //    motorTiming += 3000;
                    //    ceilingMotorDown = true;

                    //}
                    //if (IN2 && IN3 && IN4 && numberOfPressedMotor == 3 && !ceilingMotorDown)
                    //{
                    //    Console.WriteLine("Pressed all 3");
                    //    RGBButtonList[0].TurnColorOn(RGBColor.Red);
                    //    pressureMAtCount = true;
                    //    while (RGBButtonList[0].CurrentStatus() || PressureMatPressed)
                    //    {
                    //        pressureMat();
                    //        Thread.Sleep(10);
                    //    }
                    //    motorTiming = MotorStopWatch.ElapsedMilliseconds + 15000;
                    //    ceilingMotoruUp = true;

                    //    pressureMAtCount = false;
                    //    Console.WriteLine("Button Pressed");
                    //    RGBButtonList[0].TurnColorOn(RGBColor.Blue);
                    //    RGBLight.SetColor(RGBColor.Blue);
                    //    AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //    Console.WriteLine("Magnet Start");
                    //    MCP23Controller.Write(MasterOutputPin.OUTPUT4, PinState.High);
                    //    while (true)
                    //    {

                    //        if (!IsGameStartedOrInGoing())
                    //            break;
                    //        pressureMat();
                    //        if (!MCP23Controller.Read(MasterDI.IN5) && !IN5)
                    //        {
                    //            IN5 = true;
                    //            AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //            RGBLight.SetColor(RGBColor.Blue);
                    //            if (!IN6)
                    //                RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //            Console.WriteLine("IN5 PRESSED ====");
                    //        }
                    //        pressureMat();
                    //        if (IN5)
                    //        {
                    //            IN5 = !MCP23Controller.Read(MasterDI.IN5);
                    //            if (!IN5)
                    //                Console.WriteLine("IN5 bREAK ====");

                    //        }
                    //        if (!MCP23Controller.Read(MasterDI.IN7) && !IN6)
                    //        {
                    //            IN6 = true;
                    //            AudioPlayer.PIStartAudio(SoundType.Bonus);
                    //            RGBLight.SetColor(RGBColor.Blue);
                    //            if (!IN5)
                    //                RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                    //            Console.WriteLine("IN6 PRESSED ====");

                    //        }
                    //        pressureMat();
                    //        if (IN6)
                    //        {
                    //            IN6 = !MCP23Controller.Read(MasterDI.IN7);
                    //            if (!IN6)
                    //                Console.WriteLine("IN6 bREAK ====");

                    //        }
                    //        if (IN6 && IN5)
                    //            break;

                    //    }
                    //    Console.WriteLine("Game Ended");
                    //    RGBLight.SetColor(RGBColor.Blue);
                    //    Console.WriteLine("Magnet Stop");

                    //    MCP23Controller.Write(MasterOutputPin.OUTPUT4, PinState.Low);
                    //    AudioPlayer.PIStopAudio();
                    //    Thread.Sleep(300);
                    //    AudioPlayer.PIStartAudio(SoundType.Finish);
                    //    IN2 = false;
                    //    IN3 = false;
                    //    IN4 = false;
                    //    break;
                    //}
                }
                Thread.Sleep(10);


            }




        }
        bool justDecrease = false;
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
        private void pressureMat()
        {
            bool currentValue = MCP23Controller.Read(MasterDI.IN1);
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
        private async Task StopMotorAfter(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (ceilingMotorDown)
                {
                    Console.WriteLine("Start Ceiling down");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
                    Thread.Sleep(3000);
                    //while (MotorStopWatch.ElapsedMilliseconds < motorTiming) { }
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
                    ceilingMotorDown = false;
                    Console.WriteLine("stop Ceiling down");
                }
                if (ceilingMotoruUp)
                {
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
                    Console.WriteLine("Start Ceiling up");
                    MCP23Controller.Write(MasterOutputPin.OUTPUT1, PinState.High);
                    while (MotorStopWatch.ElapsedMilliseconds < motorTiming) { }
                    MCP23Controller.Write(MasterOutputPin.OUTPUT1, PinState.Low);
                    Console.WriteLine("stop Ceiling up");
                    ceilingMotoruUp = false;
                    MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
                }
            }
        }

        private void CellingDirection(bool IsUp, int Timing)
        {
            MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
            MCP23Controller.Write(MasterOutputPin.OUTPUT1, PinState.High);
            Task.Run(async () =>
            {
                await Task.Delay(Timing);
                MCP23Controller.Write(MasterOutputPin.OUTPUT1, PinState.Low);
                MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.Low);
            });

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





        // Test
        private bool CeilingButton(bool state)
        {
            if (state)
            {
                AudioPlayer.PIStartAudio(SoundType.Bonus);
                RGBLight.SetColor(RGBColor.Blue);
                RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                ceilingMotorDown = true;
                numberOfPressedMotor++;
                return true;
            }
            return false;
        }

        //     if (!MCP23Controller.Read(MasterDI.IN2) && !IN2)
        //                {
        //                    IN2 = true;
        //                    AudioPlayer.PIStartAudio(SoundType.Bonus);
        //                    RGBLight.SetColor(RGBColor.Blue);
        //                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
        //                    Console.WriteLine("====");
        //                    if (!ceilingMotorDown)
        //                    {
        //                        motorTiming = MotorStopWatch.ElapsedMilliseconds;
        //                        MCP23Controller.Write(MasterOutputPin.OUTPUT5, PinState.High);
        //                    }
        //                    motorTiming += 3000;
        //                    numberOfPressedMotor++;
        //                    ceilingMotorDown = true;

        //                }
    }
}










//if (!ceilingMotorDown)
//{
//    motorTiming = MotorStopWatch.ElapsedMilliseconds;
//    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.High);
//}
//motorTiming += 5000;
//ceilingMotorDown = true;
//Thread.Sleep(1000);

//if (!ceilingMotorDown)
//{
//    motorTiming = MotorStopWatch.ElapsedMilliseconds;
//    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.High);
//}
//motorTiming += 5000;
//Thread.Sleep(1000);

//ceilingMotorDown = true;
//if (!ceilingMotorDown)
//{
//    motorTiming = MotorStopWatch.ElapsedMilliseconds;
//    MCP23Controller.Write(MasterOutputPin.OUTPUT5.Chip, MasterOutputPin.OUTPUT5.port, MasterOutputPin.OUTPUT5.PinNumber, PinState.High);
//}
//motorTiming += 5000;
//ceilingMotorDown = true;
//Thread.Sleep(15000);

////==================
//motorTiming = MotorStopWatch.ElapsedMilliseconds + 15000;
//ceilingMotoruUp = true;

//Console.WriteLine("Main");
