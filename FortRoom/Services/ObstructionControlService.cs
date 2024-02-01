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
        ModbusLib Modbus = new ModbusLib();
        Stopwatch GameStopWatch = new Stopwatch();
        bool IsTimerStarted = false;
        bool IsMotorOneStarted = false;
        bool IsMotorOneStartPeriod2 = false;
        bool IsMotorOneStartPeriod3 = false;
        int SlowPeriod = 3000;
        int MeduimPeriod = 3000;

        //        var modbus = new ModbusLib();
        //        modbus.Init(SerialPortMapping.PortMap["Serial0"]);
        //while (true)
        //{
        //    //MotorStatus
        //    //ModbusAddres
        //    Console.WriteLine("Start Motor Forward");
        //    modbus.WriteSingleRegister(1, (int) ModbusAddress.startStop, (int) MotorStatus.Run);
        //        modbus.WriteSingleRegister(1, (int) ModbusAddress.Speed, (int) MotorSpeed.Medium);
        //        Thread.Sleep(5000);
        //    Console.WriteLine("Start Motor Reverse");
        //    modbus.WriteSingleRegister(1, (int) ModbusAddress.startStop, (int) MotorStatus.Reverse);
        //        modbus.WriteSingleRegister(1, (int) ModbusAddress.Speed, (int) MotorSpeed.Slow);
        //        Thread.Sleep(5000);

        //}

        private CancellationTokenSource _cts1, _cts2, _cts3, _cts4, _cts5;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Modbus.Init(SerialPort.Serial);
            _cts1 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts3 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts4 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts5 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task task1 = Task.Run(() => RunService1(_cts1.Token));
            Task task2 = Task.Run(() => RunService2(_cts2.Token));
            Task task3 = Task.Run(() => RunService3(_cts3.Token));
            Task task4 = Task.Run(() => RunService4(_cts4.Token));
            Task task5 = Task.Run(() => TimingService(_cts5.Token));
            return Task.WhenAll(task1, task2, task3, task4, task5);
        }


        private async Task RunService1(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (VariableControlService.IsTheGameStarted && IsTimerStarted)
                {
                    if (!IsMotorOneStarted)
                    {
                        Modbus.WriteSingleRegister(1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        Modbus.WriteSingleRegister(1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        IsMotorOneStarted = true;
                    }
                    if (!IsMotorOneStartPeriod2 && IsMotorOneStarted && GameStopWatch.ElapsedMilliseconds > SlowPeriod)
                    {
                        Modbus.WriteSingleRegister(1, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
                        IsMotorOneStartPeriod2 = true;
                    }
                    if (!IsMotorOneStartPeriod3 && IsMotorOneStarted && GameStopWatch.ElapsedMilliseconds > MeduimPeriod)
                    {
                        Modbus.WriteSingleRegister(1, (int)ModbusAddress.Speed, (int)MotorSpeed.High);
                        IsMotorOneStartPeriod3 = true;
                    }
                }

                Thread.Sleep(10);
            }

        }

        private async Task RunService2(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {


                // Your logic for service 2

            }
        }

        private async Task RunService3(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {

                // Your logic for service 3

            }
        }

        private async Task RunService4(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {

                // Your logic for service 1

            }
        }
        private async Task TimingService(CancellationToken cancellationToken)
        {
            if (VariableControlService.IsTheGameStarted)
            {
                if (!IsTimerStarted)
                {
                    GameStopWatch.Start();
                    IsTimerStarted = true;
                }

                // Your logic for service 1

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
    }
}
