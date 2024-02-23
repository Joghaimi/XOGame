using Library.Media;
using Library.RGBLib;
using Library;
using System.Device.Gpio;
using System.Diagnostics;
using Library.Modbus;
using Library.Enum;
using Library.PinMapping;

namespace FortRoom.Services
{
    public class ObstructionControlService : IHostedService, IDisposable
    {
        private readonly IHostApplicationLifetime _appLifetime;
        private ModbusLib Modbus = new ModbusLib();
        private Stopwatch GameStopWatch = new Stopwatch();
        private readonly ILogger<ObstructionControlService> _logger;
        private CancellationTokenSource _cts1, _cts2, _cts3, _cts4, _cts5;

        bool IsTimerStarted = false;
        bool IsMotorOneStarted = false;
        bool IsMotorOneStartPeriod2 = false;
        bool IsMotorOneStartPeriod3 = false;
        int SlowPeriod = 5000;
        int MediumPeriod = 10000;


        bool IsMotorTwoStarted = false;
        bool IsMotorTwoStartPeriod2 = false;
        bool IsMotorTwoStartPeriod3 = false;
        int MotorTwoDiffPeriod = 1000;

        bool IsMotorThreeStarted = false;
        bool IsMotorThreeStartPeriod2 = false;
        bool IsMotorThreeStartPeriod3 = false;
        int MotorThreeDiffPeriod = 2000;


        bool IsMotorFourStarted = false;
        bool IsMotorFourStartPeriod2 = false;
        bool IsMotorFourStartPeriod3 = false;
        int MotorFourDiffPeriod = 3000;

        public ObstructionControlService(ILogger<ObstructionControlService> logger, IHostApplicationLifetime appLifetime)
        {
            _appLifetime = appLifetime;
            _logger = logger;

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Obstruction Service");
            Modbus.Init(SerialPort.Serial);
            _appLifetime.ApplicationStopping.Register(Stopped);
            _cts1 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //_cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //_cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //_cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            //_cts5 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task task1 = Task.Run(() => RunService1(_cts1.Token));
            //Task task2 = Task.Run(() => RunService2(_cts2.Token));
            //Task task3 = Task.Run(() => RunService3(_cts3.Token));
            //Task task4 = Task.Run(() => RunService4(_cts4.Token));
            //Task task5 = Task.Run(() => TimingService(_cts5.Token));

            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 

            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
            ////Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

            return Task.CompletedTask;

            //return Task.WhenAll(task1, task2, task3, task4, task5);
        }


        private async Task RunService1(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.IsTheGameStarted)
                {
                    try
                    {
                        Console.WriteLine($"Motor 1 Started freq Slow");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 2 Started freq Slow");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 3 Started freq Slow");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 4 Started freq Slow");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, 2000);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        Thread.Sleep(5000);

                        Console.WriteLine($"Motor 1 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 2 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 3 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 4 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, /*(int)MotorSpeed.Medium*/ 2000);  // Start As Mode #1 

                        Thread.Sleep(8000);
                        Console.WriteLine($"Motor 1 Started freq Heigh");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.High);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 2 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.High);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 3 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 4 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, 2000);  // Start As Mode #1 

                        VariableControlService.IsTheGameStarted = false;
                        //if (!IsMotorOneStarted)
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq Slow");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        //    Thread.Sleep(300);
                        //    IsMotorOneStarted = true;
                        //}
                        //if (!IsMotorOneStartPeriod2 && IsMotorOneStarted && GameStopWatch.ElapsedMilliseconds > SlowPeriod)
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq Medium");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);

                        //    Thread.Sleep(300);
                        //    IsMotorOneStartPeriod2 = true;
                        //}
                        //if (!IsMotorOneStartPeriod3 && IsMotorOneStarted && GameStopWatch.ElapsedMilliseconds > MediumPeriod)
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq High");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.High);
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        //    Thread.Sleep(300);
                        //    IsMotorOneStartPeriod3 = true;
                        //}

                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Obstruction RunService1 {ex.Message}");
                    }


                }

                Thread.Sleep(10);
            }

        }

        private async Task RunService2(CancellationToken cancellationToken)
        {
            while (true)
            {
                //if (VariableControlService.IsTheGameStarted && IsTimerStarted)
                //{
                //    if (!IsMotorTwoStarted && IsMotorOneStarted)
                //    {
                //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                //        IsMotorTwoStarted = true;
                //    }
                //    if (IsMotorOneStartPeriod2 && !IsMotorTwoStartPeriod2 && IsMotorTwoStarted && GameStopWatch.ElapsedMilliseconds > (SlowPeriod + MotorTwoDiffPeriod))
                //    {
                //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
                //        IsMotorTwoStartPeriod2 = true;
                //    }
                //    if (IsMotorOneStartPeriod3 &&!IsMotorTwoStartPeriod3 && IsMotorTwoStarted && GameStopWatch.ElapsedMilliseconds > (MediumPeriod + MotorTwoDiffPeriod))
                //    {
                //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.High);
                //        IsMotorTwoStartPeriod3 = true;
                //    }

                //}
            }

        }

        private async Task RunService3(CancellationToken cancellationToken)
        {
            //while (true) {

            //if (VariableControlService.IsTheGameStarted && IsTimerStarted)
            //{
            //    if (!IsMotorThreeStarted)
            //    {
            //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
            //        IsMotorThreeStarted = true;
            //    }
            //    if (!IsMotorThreeStartPeriod2 && IsMotorThreeStarted && GameStopWatch.ElapsedMilliseconds > (SlowPeriod + MotorThreeDiffPeriod))
            //    {
            //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
            //        IsMotorThreeStartPeriod2 = true;
            //    }
            //    if (!IsMotorThreeStartPeriod3 && IsMotorThreeStarted && GameStopWatch.ElapsedMilliseconds > (MediumPeriod + MotorThreeDiffPeriod))
            //    {
            //        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.High);
            //        IsMotorThreeStartPeriod3 = true;
            //    }
            //}
            //}

        }

        private async Task RunService4(CancellationToken cancellationToken)
        {
            //while (true)
            //{
            //    if (VariableControlService.IsTheGameStarted && IsTimerStarted)
            //    {
            //        if (!IsMotorFourStarted)
            //        {
            //            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
            //            IsMotorFourStarted = true;
            //        }
            //        if (!IsMotorFourStartPeriod2 && IsMotorFourStarted && GameStopWatch.ElapsedMilliseconds > (SlowPeriod + MotorFourDiffPeriod))
            //        {
            //            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
            //            IsMotorFourStartPeriod2 = true;
            //        }
            //        if (!IsMotorFourStartPeriod3 && IsMotorFourStarted && GameStopWatch.ElapsedMilliseconds > (MediumPeriod + MotorFourDiffPeriod))
            //        {
            //            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, (int)MotorSpeed.High);
            //            IsMotorFourStartPeriod3 = true;
            //        }

            //    }

            //}

        }

        private async Task TimingService(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {
                if (!IsTimerStarted)
                {
                    Console.WriteLine("Start The Timer");
                    IsTimerStarted = true;
                    GameStopWatch.Start();
                    Console.WriteLine("Start The Timer2");

                }
            }
        }


        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts1?.Cancel();
            _cts2?.Cancel();
            _cts3?.Cancel();
            _cts4?.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts1?.Dispose();
            _cts2?.Dispose();
            _cts3?.Dispose();
            _cts4?.Dispose();
        }
        public void Stopped()
        {
            _logger.LogInformation("Stop The Service");
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Thread.Sleep(500);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Thread.Sleep(500);

            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Thread.Sleep(500);

            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Thread.Sleep(500);

            Modbus.ReleasePort();
            _logger.LogInformation("Port Released");

        }


    }
}
