using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RFIDLib
{
    public class RFIDSerial
    {
        SerialPort SerialPort;
        public bool isBusy = false;
        public RFIDSerial(string SerialPort)
        {
            this.SerialPort = new SerialPort(SerialPort, 9600); // Replace "COM1" with your actual serial port name
            if (!this.SerialPort.IsOpen)
                this.SerialPort.Open();
        }
        public string GetRFIDUID()
        {
            SerialPort.WriteLine("RFID");
            string receivedData = SerialPort.ReadTo("\r");
            receivedData = receivedData.Replace("\n", "");
            return receivedData.Replace("UID: ", "");
        }
        public bool TurnOffAllDisplay()
        {
            return false;
        }
    }
}
