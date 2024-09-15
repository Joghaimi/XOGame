using System.Diagnostics;
using Library.RGBLib;
using Library;
using Library.Media;
using Library.PinMapping;
using Library.GPIOLib;
using System.Device.Gpio;
using Iot.Device.Mcp3428;
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
        MCP23Pin MagnetRelay = MasterOutputPin.OUTPUT4;
        MCP23Pin CellingUPRelay = MasterOutputPin.OUTPUT1;
        MCP23Pin CellingDownRelay = MasterOutputPin.OUTPUT5;
        bool magnetStarted = false;


        bool restartedBefore = false;
        bool taskOneFinished = false;
        bool taskTwoFinished = false;

        bool kidsButtonOneClicked = false;
        bool kidsButtonTwoClicked = false;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            // TO DO Init The RGB Light .. 
            // Init RGB
            Console.WriteLine("Floor Is Lava Service");
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            RGBButtonList.Add(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6));
            RGBButtonList[0].TurnColorOn(RGBColor.Off);
            RGBButtonList[1].TurnColorOn(RGBColor.Off);
            RGBButtonList[2].TurnColorOn(RGBColor.Off);



            MCP23Controller.PinModeSetup(MasterDI.IN8, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN2, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN3, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN4, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN5, PinMode.Input);
            MCP23Controller.PinModeSetup(MasterDI.IN7, PinMode.Input);


            MCP23Controller.PinModeSetup(CellingUPRelay, PinMode.Output);
            MCP23Controller.PinModeSetup(CellingDownRelay, PinMode.Output);


            RelayController.Status(MagnetRelay, false);


            //MCP23Controller.PinModeSetup(MagnetRelay, PinMode.Output);
            //MCP23Controller.Write(MagnetRelay, PinState.Low); // Relese Magnet



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

            //while (true)
            //{
            //    RGBButtonList[1].TurnColorOn(RGBColor.Red);
            //    RGBButtonList[2].TurnColorOn(RGBColor.Red);
            //    //Console.WriteLine($"Button One {RGBButtonList[1].TurnColorOn(RGBColor.Red)}");
            //    //Console.WriteLine($"Button One {RGBButtonList[2].TurnColorOn(RGBColor.Red)}");
            //    //Console.WriteLine($"Button Two {RGBButtonList[2].CurrentStatus()}");
            //    Console.WriteLine($"***********************");
            //    Thread.Sleep(1000);
            //    //kidsButtonOneClicked = RGBButtonList[1].CurrentStatus();
            //}

            while (!cancellationToken.IsCancellationRequested)
            {
                if (VariableControlService.GameStatus == GameStatus.Empty && !restartedBefore)
                {
                    Console.WriteLine("Restart The Game");
                    // Reset All Variable 
                    IN2 = false;
                    IN3 = false;
                    IN4 = false;
                    IN5 = false;
                    IN7 = false;
                    taskOneFinished = false;
                    taskTwoFinished = false;
                    numberOfPressedMotor = 0;
                    // Turn Off The Magnet
                    Console.WriteLine("Stop The Magnet");
                    RelayController.Status(MagnetRelay, false);
                    restartedBefore = true;
                    RGBLight.SetColor(RGBColor.Red);
                    RGBButtonList[0].TurnColorOn(RGBColor.Off);
                    CellingDirection(true, 10000);


                }
                if (IsGameStartedOrInGoing())
                {
                    restartedBefore = false;
                    // Game Sequence 
                    //if (!taskOneFinished)
                    //    TaskOne();
                    if (!taskOneFinished)
                        TaskOneCeilingTask();

                    //bool TaskOneEnded = IN2 && IN3 && IN4 && numberOfPressedMotor == 3 && !ceilingMotorDown;
                    bool CeilingDown = numberOfPressedMotor == 3 && !ceilingMotorDown;
                    if (CeilingDown && !taskTwoFinished)
                    {
                        Console.WriteLine("All 3 Button Pressed , Task One Ended");
                        RGBButtonList[0].TurnColorOn(RGBColor.Red);
                        pressureMAtCount = true;

                        while (RGBButtonList[0].CurrentStatus() || PressureMatPressed)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            pressureMat();
                            Thread.Sleep(10);

                        }

                        VariableControlService.TeamScore.FloorIsLavaRoomScore += 150;
                        motorTiming = MotorStopWatch.ElapsedMilliseconds + 15000;
                        ceilingMotoruUp = true;
                        pressureMAtCount = false;

                        Console.WriteLine("Button Pressed");

                        RGBButtonList[0].TurnColorOn(RGBColor.Blue);
                        RGBLight.SetColor(RGBColor.Blue);
                        AudioPlayer.PIStartAudio(SoundType.Bonus);
                        RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);

                        Console.WriteLine("Magnet Start");
                        RelayController.Status(MagnetRelay, true);
                        bool magnetOneAttached = false;
                        bool magnetTwoAttached = false;



                        while (true)
                        {
                            if (!IsGameStartedOrInGoing())
                                break;
                            pressureMat();
                            if (!MCP23Controller.Read(MasterDI.IN5, true) && !IN5)
                            {
                                IN5 = true;
                                if (!magnetOneAttached)
                                {
                                    magnetOneAttached = true;
                                    AudioPlayer.PIStartAudio(SoundType.Charge);
                                    RGBLight.SetColor(RGBColor.Blue);
                                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 100;
                                }

                                if (!IN7)
                                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                                Console.WriteLine("IN5 PRESSED ====");
                            }
                            pressureMat();
                            if (IN5)
                            {
                                IN5 = !MCP23Controller.Read(MasterDI.IN5, true);
                                if (!IN5)
                                    Console.WriteLine("IN5 bREAK ====");

                            }
                            if (!MCP23Controller.Read(MasterDI.IN7, true) && !IN7)
                            {
                                IN7 = true;
                                if (!magnetTwoAttached)
                                {
                                    AudioPlayer.PIStartAudio(SoundType.Charge);
                                    RGBLight.SetColor(RGBColor.Blue);
                                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 100;
                                    magnetTwoAttached = true;
                                }
                                if (!IN5)
                                    RGBLight.TurnRGBColorDelayedASec(RGBColor.Red);
                                Console.WriteLine("IN6 PRESSED ====");

                            }
                            pressureMat();
                            if (IN7)
                            {
                                IN7 = !MCP23Controller.Read(MasterDI.IN7, true);
                                if (!IN7)
                                    Console.WriteLine("IN6 bREAK ====");

                            }
                            if (IN7 && IN5)
                                break;
                            Thread.Sleep(10);
                        }

                        Console.WriteLine("Game Ended");
                        RGBLight.SetColor(RGBColor.Blue);
                        Thread.Sleep(5000);
                        AudioPlayer.PIStopAudio();
                        Thread.Sleep(300);
                        AudioPlayer.PIStartAudio(SoundType.Finish);
                        VariableControlService.GameStatus = GameStatus.FinishedNotEmpty;
                        IN2 = false;
                        IN3 = false;
                        IN4 = false;
                        taskTwoFinished = true;
                        Console.WriteLine("Task Two Finished");
                        //break;
                    }



                }

            }


        }


        void TaskOneCeilingTask()
        {
            kidsButtonOneClicked = false; kidsButtonTwoClicked = false;
            Console.WriteLine();
            while (true)
            {
                if (!IsGameStartedOrInGoing())
                    break;
                pressureMat();
                if (VariableControlService.TeamScore.isAdult)
                {
                    var taskFinished = AdultPadsTask();
                    if (taskFinished)
                        break;
                }
                else
                {
                    var taskFinished = KidsButtonsTask();
                    if (taskFinished)
                        break;
                }
                taskOneFinished = true;
            }

        }


        bool AdultPadsTask()
        {
            if (!IN2)
            {
                IN2 = CeilingButton(!MCP23Controller.Read(MasterDI.IN2, true));
                if (IN2)
                {
                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                    Console.WriteLine("IN2 Scored");
                    Thread.Sleep(100);
                }
            }
            if (!IN3)
            {
                IN3 = CeilingButton(!MCP23Controller.Read(MasterDI.IN3, true));
                if (IN3)
                {
                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                    Console.WriteLine("IN3 Scored");
                    Thread.Sleep(100);
                }
            }
            if (!IN4)
            {
                IN4 = CeilingButton(!MCP23Controller.Read(MasterDI.IN4, true));
                if (IN4)
                {
                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                    Console.WriteLine("IN4 Scored");
                    Thread.Sleep(100);
                }
            }
            if (IN2 && IN3 && IN4)
            {
                taskOneFinished = true;
                return true;
            }
            Thread.Sleep(100);
            return false;
        }
        bool KidsButtonsTask()
        {
            if (!kidsButtonOneClicked)
            {
                RGBButtonList[1].TurnColorOn(RGBColor.Red);
                kidsButtonOneClicked = !RGBButtonList[1].CurrentStatus();
                if (kidsButtonOneClicked)
                {
                    Console.WriteLine("RGB Button One Pressed");
                    RGBButtonList[1].TurnColorOn(RGBColor.Off);
                    CeilingButton(true);
                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                    Thread.Sleep(100);
                }
            }
            if (!kidsButtonTwoClicked)
            {
                RGBButtonList[2].TurnColorOn(RGBColor.Red);
                kidsButtonTwoClicked = !RGBButtonList[2].CurrentStatus();

                if (kidsButtonTwoClicked)
                {
                    Console.WriteLine("RGB Button Two Pressed");

                    RGBButtonList[2].TurnColorOn(RGBColor.Off);
                    VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                    CeilingButton(true);
                    Thread.Sleep(100);
                }
            }
            if (kidsButtonOneClicked && kidsButtonTwoClicked)
            {
                VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                CeilingButton(true);
                Thread.Sleep(100);
                return true;

            }

            return false;
        }


        void TaskOne()
        {
            while (true)
            {
                if (!IsGameStartedOrInGoing())
                    break;
                pressureMat();

                try
                {
                    if (VariableControlService.TeamScore.isAdult)
                    {
                        if (!IN2)
                        {
                            IN2 = CeilingButton(!MCP23Controller.Read(MasterDI.IN2, true));
                            if (IN2)
                            {
                                VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                                Console.WriteLine("IN2 Scored");
                                Thread.Sleep(100);
                            }
                        }
                        if (!IN3)
                        {
                            IN3 = CeilingButton(!MCP23Controller.Read(MasterDI.IN3, true));
                            if (IN3)
                            {
                                VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                                Console.WriteLine("IN3 Scored");
                                Thread.Sleep(100);
                            }
                        }
                        if (!IN4)
                        {
                            IN4 = CeilingButton(!MCP23Controller.Read(MasterDI.IN4, true));
                            if (IN4)
                            {
                                VariableControlService.TeamScore.FloorIsLavaRoomScore += 50;
                                Console.WriteLine("IN4 Scored");
                                Thread.Sleep(100);
                            }
                        }
                        if (IN2 && IN3 && IN4)
                        {
                            taskOneFinished = true;
                            break;
                        }
                        Thread.Sleep(100);
                    }
                    else
                    {
                    }


                    //if (!IN2)
                    //{
                    //    IN2 = CeilingButton(!MCP23Controller.Read(MasterDI.IN2, true));
                    //    if (IN2)
                    //    {
                    //        VariableControlService.TeamScore.FloorIsLavaRoomScore += 25;
                    //        Console.WriteLine("IN2 Scored");
                    //        Thread.Sleep(100);
                    //    }
                    //}
                    //if (!IN3)
                    //{
                    //    IN3 = CeilingButton(!MCP23Controller.Read(MasterDI.IN3, true));
                    //    if (IN3)
                    //    {
                    //        VariableControlService.TeamScore.FloorIsLavaRoomScore += 25;
                    //        Console.WriteLine("IN3 Scored");
                    //        Thread.Sleep(100);
                    //    }
                    //}
                    //if (!IN4)
                    //{
                    //    IN4 = CeilingButton(!MCP23Controller.Read(MasterDI.IN4, true));
                    //    if (IN4)
                    //    {
                    //        VariableControlService.TeamScore.FloorIsLavaRoomScore += 25;
                    //        Console.WriteLine("IN4 Scored");
                    //        Thread.Sleep(100);
                    //    }
                    //}
                    //if (IN2 && IN3 && IN4)
                    //{
                    //    taskOneFinished = true;
                    //    break;
                    //}
                    //Thread.Sleep(100);
                }
                catch (Exception ex) { Console.WriteLine(ex.ToString()); }

            }
        }

        void TaskTwo()
        {

        }









        bool justDecrease = false;
        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.GameStatus == GameStatus.Started;
        }
        private void pressureMat()
        {
            bool currentValue = MCP23Controller.Read(MasterDI.IN8, false);
            if (!currentValue && !justDecrease)
            {
                VariableControlService.TeamScore.FloorIsLavaRoomScore -= 25;
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
                    MCP23Controller.Write(CellingDownRelay, PinState.High);
                    Thread.Sleep(3000);
                    //while (MotorStopWatch.ElapsedMilliseconds < motorTiming) { }
                    MCP23Controller.Write(CellingDownRelay, PinState.Low);
                    ceilingMotorDown = false;
                    Console.WriteLine("stop Ceiling down");
                }
                if (ceilingMotoruUp)
                {
                    MCP23Controller.Write(CellingDownRelay, PinState.Low);
                    Console.WriteLine("Start Ceiling up");
                    MCP23Controller.Write(CellingUPRelay, PinState.High);
                    while (MotorStopWatch.ElapsedMilliseconds < motorTiming) { }
                    MCP23Controller.Write(CellingUPRelay, PinState.Low);
                    Console.WriteLine("stop Ceiling up");
                    ceilingMotoruUp = false;
                    MCP23Controller.Write(CellingDownRelay, PinState.Low);
                }
            }
        }

        private void CellingDirection(bool IsUp, int Timing)
        {
            if (IsUp)
            {
                MCP23Controller.Write(CellingDownRelay, PinState.Low);
                MCP23Controller.Write(CellingUPRelay, PinState.High);
                Task.Run(async () =>
                {
                    await Task.Delay(Timing);
                    MCP23Controller.Write(CellingUPRelay, PinState.Low);
                    MCP23Controller.Write(CellingDownRelay, PinState.Low);
                });
            }
            else
            {
            }


        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            AudioPlayer.PIStopAudio();
            MCP23Controller.Write(MagnetRelay, PinState.Low);
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
    }
}
