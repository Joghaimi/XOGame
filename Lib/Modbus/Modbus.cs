using Iot.Device.Mcp23xxx;
using Iot.Device.Pn532;
using Library.RFIDLib;
using Microsoft.Extensions.Logging;
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
        private SerialPort port = new();
        private static readonly ILogger _logger = LoggerFactory.Create(builder => { builder.AddConsole(); }).CreateLogger<RFID>();
        public bool Init(string SerialPort)
        {
            try
            {
                port = new SerialPort(SerialPort);
                port.BaudRate = 9600;
                port.DataBits = 8;
                port.Parity = Parity.None;
                port.StopBits = StopBits.One;
                port.Open();
                master = ModbusSerialMaster.CreateRtu(port);
                _logger.LogInformation("Modbus Initialized");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("InitModbusRTU  " + ex);
                return false;
            }
        }
        public void WriteSingleRegister(byte slaveAddress, ushort registerAddress, ushort valueToWrite)
        {
            try
            {
                _logger.LogTrace($"Write To {slaveAddress} at register {registerAddress} value {valueToWrite} ");
                //master.WriteSingleRegister(slaveAddress, registerAddress, valueToWrite);
            }
            catch (Exception ex)
            {
                _logger.LogError($"InitModbusRTU  {ex.Message}");
            }
        }
        public void ReleasePort()
        {
            try
            {
                master.Dispose();
                port.Close();
                port.Dispose();
                _logger.LogInformation("Release Modbus Port");
            }
            catch (Exception ex)
            {
                _logger.LogError($"Release Modbus Port Feild {ex.Message}");
            }
        }
    }
}
