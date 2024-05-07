using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modbus
{
    public static class ObstructionLib
    {
        private static ModbusLib Modbus = new ModbusLib();
        public static void init(string SerialPort)
        {
            Modbus.Init(SerialPort);
        }
        public static void RunCommand(ModbusSlave slave, MotorSpeed speed, MotorStatus status)
        {
            Modbus.WriteSingleRegister((byte)slave, (int)ModbusAddress.Speed, (ushort)speed);  // Start As Mode #1 
            Modbus.WriteSingleRegister((byte)slave, (int)ModbusAddress.startStop, (ushort)status);
            Thread.Sleep(500);
        }

        public static void Start()
        {
            RunCommand(ModbusSlave.Slave1, MotorSpeed.Motor1Round1, MotorStatus.Run);
            RunCommand(ModbusSlave.Slave2, MotorSpeed.Motor2Round1, MotorStatus.Reverse);
            RunCommand(ModbusSlave.Slave3, MotorSpeed.Motor3Round1, MotorStatus.Reverse);
            RunCommand(ModbusSlave.Slave4, MotorSpeed.Motor4Round1, MotorStatus.Reverse);
        }
        public static void Stop()
        {
            RunCommand(ModbusSlave.Slave1, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave2, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave3, MotorSpeed.Stop, MotorStatus.Stop);
            RunCommand(ModbusSlave.Slave4, MotorSpeed.Stop, MotorStatus.Stop);
        }
        public static void Release() {
            Modbus.ReleasePort();
        }
    }
}
