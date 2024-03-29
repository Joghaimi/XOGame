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
        private ModbusLib Modbus = new ModbusLib();
        private readonly ILogger<ObstructionControlService> _logger;
        private CancellationTokenSource _cts1;

        public ObstructionControlService(ILogger<ObstructionControlService> logger)
        {
            _logger = logger;

        }
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start Obstruction Service");
            Modbus.Init(SerialPort.Serial);
            _cts1 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task task1 = Task.Run(() => RunService1(_cts1.Token));
            return Task.CompletedTask;
        }


        private async Task RunService1(CancellationToken cancellationToken)
        {
            while (true)
            {
                if (IsGameStartedOrInGoing())
                {
                    if (!VariableControlService.IsObstructionServiceStarted)
                        VariableControlService.IsObstructionServiceStarted = true;
                    try
                    {
                        if (!VariableControlService.IsThingsChangedForTheNewRound)
                        {
                            _logger.LogInformation("Change Motor Speed For the New Round");
                            ControlObstructionSpeed(VariableControlService.GameRound);
                        }


                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq Slow");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 2 Started freq Slow");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{

                        //    Console.WriteLine($"Motor 3 Started freq Slow");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 1000);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 4 Started freq Slow");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, 2000);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);

                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 2 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 3 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 3000);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 4 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, /*(int)MotorSpeed.Medium*/ 3000);  // Start As Mode #1 
                        //}
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //    Thread.Sleep(1000);
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 1 Started freq Heigh");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.High);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 2 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.High);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 3 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, 3000);  // Start As Mode #1 
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
                        //    Thread.Sleep(500);
                        //}
                        //if (IsGameStartedOrInGoing())
                        //{
                        //    Console.WriteLine($"Motor 4 Started freq Meduim");
                        //    Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, 4000);  // Start As Mode #1 
                        //}
                        //VariableControlService.IsTheGameStarted = false;
                    }
                    catch (Exception ex)
                    {
                    }


                }
                else if (!IsGameStartedOrInGoing() && VariableControlService.IsObstructionServiceStarted)
                {

                    StopObstructionService();
                }

                Thread.Sleep(10);
            }

        }


        private void ControlObstructionSpeed(Round round)
        {
            switch (round)
            {
                case Round.Round1:
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round1, MotorStatus.Run);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round1, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round1, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round1, MotorStatus.Reverse);
                    break;
                case Round.Round2:
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round2, MotorStatus.Run);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round2, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round2, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round2, MotorStatus.Reverse);
                    break;
                case Round.Round3:
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round3, MotorStatus.Run);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round3, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round3, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round3, MotorStatus.Reverse);
                    break;
                case Round.Round4:
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round4, MotorStatus.Run);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round4, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round4, MotorStatus.Reverse);
                    if (IsGameStartedOrInGoing())
                        RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round4, MotorStatus.Reverse);
                    break;
            }
            VariableControlService.IsThingsChangedForTheNewRound = true;

        }
        private void RunCommand(ModbusSlave slave, MotorSpeed speed, MotorStatus status)
        {
            Modbus.WriteSingleRegister((byte)slave, (int)ModbusAddress.Speed, (ushort)speed);  // Start As Mode #1 
            Modbus.WriteSingleRegister((byte)slave, (int)ModbusAddress.startStop, (ushort)status);
            Thread.Sleep(500);
        }


        private bool IsGameStartedOrInGoing()
        {
            return VariableControlService.IsTheGameStarted && !VariableControlService.IsTheGameFinished;
        }
        void StopObstructionService()
        {
            _logger.LogInformation("Obstruction Stopped ***");
            VariableControlService.IsObstructionServiceStarted = false;
            RunCommand(ModbusSlave.Slave1, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave2, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave3, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave4, MotorSpeed.Stop, MotorStatus.Stop);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave1, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            //Thread.Sleep(500);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave2, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            //Thread.Sleep(500);

            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave3, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);
            //Thread.Sleep(500);

            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.startStop, (int)MotorStatus.Stop);
            //Modbus.WriteSingleRegister((byte)ModbusSlave.Slave4, (int)ModbusAddress.Speed, (int)MotorSpeed.Stop);

            //Thread.Sleep(500);
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
            StopObstructionService();
            Modbus.ReleasePort();
            _logger.LogInformation("Obstruction - Port Released");
        }

        

    }
}
