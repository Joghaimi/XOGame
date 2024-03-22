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
        //private readonly IHostApplicationLifetime _appLifetime;
        private ModbusLib Modbus = new ModbusLib();
        private readonly ILogger<ObstructionControlService> _logger;
        private CancellationTokenSource _cts1;

        
        public ObstructionControlService(ILogger<ObstructionControlService> logger, IHostApplicationLifetime appLifetime)
        {
            //_appLifetime = appLifetime;
            _logger = logger;

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Obstruction Service");
            Modbus.Init(SerialPort.Serial);
            //_appLifetime.ApplicationStopping.Register(Stopped);
            _cts1 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            Task task1 = Task.Run(() => RunService1(_cts1.Token));
            return Task.CompletedTask;
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
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 1000);  // Start As Mode #1 
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
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 3000);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 4 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, /*(int)MotorSpeed.Medium*/ 3000);  // Start As Mode #1 

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
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 3000);  // Start As Mode #1 
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        Thread.Sleep(500);
                        Console.WriteLine($"Motor 4 Started freq Meduim");
                        Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, 4000);  // Start As Mode #1 
                          
                        //VariableControlService.IsTheGameStarted = false;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Obstruction RunService1 {ex.Message}");
                    }


                }

                Thread.Sleep(10);
            }

        }



        public Task StopAsync(CancellationToken cancellationToken)
        {
            Stopped();
            //_cts1?.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts1?.Dispose();
        }
        public void Stopped()
        {
            _logger.LogInformation("Obstruction Stopped");
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            Thread.Sleep(500);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            Thread.Sleep(500);

            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            Thread.Sleep(500);

            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);

            Thread.Sleep(500);
            Modbus.ReleasePort();
            _logger.LogInformation("Obstruction - Port Released");
        }


    }
}
