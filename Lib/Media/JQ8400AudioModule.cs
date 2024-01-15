using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Media
{
    public class JQ8400AudioModule
    {
        private SerialPort serialPort;

        public JQ8400AudioModule(string portName)
        {
            serialPort = new SerialPort(portName, 9600);  // Adjust baud rate as per the module's specs
            serialPort.Open();
        }
        public void PlayAudio(int trackNumber)
        {
            
            byte[] command = { 0xAA, 0x0C, 0x02, 0x00, 0x06, 0xBE };
            serialPort.Write(command, 0, command.Length);

            Console.WriteLine($"Playing audio file {trackNumber}");
        }

        public void StopPlayback()
        {
            // Implement the logic to send the command to stop playback
            // Example: serialPort.Write(new byte[] { 0xAA, 0x0D, 0x00, 0xB8 }, 0, 4);
            byte[] command = { 0xAA, 0x04, 0x00, 0xAE };
            serialPort.Write(command, 0, command.Length);
        }
        //Inquiry of current file number (0D) :->AA 0D 00 B7 
        //public byte[] CurrentFileNumber() {
        //    byte[] command = { 0xAA, 0x0D, 0x00, 0xB7 };


        //}

        public void Close()
        {
            serialPort.Close();
        }
    }
}
