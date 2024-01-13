using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Display
{
    public class DisplayController
    {
        SerialPort SerialPort;
        public bool isBusy=false;
        public void Init(string SerialPort)
        {
            this.SerialPort = new SerialPort(SerialPort, 9600); // Replace "COM1" with your actual serial port name
            if (!this.SerialPort.IsOpen)
                this.SerialPort.Open();
        }
        public bool SendCommand(Displays DisplayNumber  , DisplayCommand command) {
            if(isBusy)
                return false;
            SerialPort.WriteLine($"{(int)DisplayNumber}{(int)command}");
            isBusy = true;
            string receivedData = SerialPort.ReadTo("\r");
            isBusy = false;
            return receivedData =="received";
        }
        public bool TurnOffAllDisplay() { 
            return false;
        }
    }
}
