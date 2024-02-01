using Iot.Device.Mcp23xxx;
using Iot.Device.Pn532;
using Modbus.Device;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Modbus
{
    public class ModbusLib
    {
        ModbusSerialMaster master;
        public bool Init(string SerialPort)
        {
            try
            {
                SerialPort port = new SerialPort(SerialPort);
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();
                master = ModbusSerialMaster.CreateRtu(port);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("InitModbusRTU  " + ex);
                return false;

            }
        }
        public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort valueToWrite)
        {
            try
            {
                // Write single register
                master.WriteSingleRegister(slaveAddress, registerAddress, valueToWrite);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }
        }
    }
}
