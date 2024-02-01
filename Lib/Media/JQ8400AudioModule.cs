using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Media
{
    public static class JQ8400AudioModule
    {
        private static SerialPort serialPort;

        public static void init(string portName)
        {
            serialPort = new SerialPort(portName, 9600);  // Adjust baud rate as per the module's specs
            serialPort.Open();
        }
        public static void PlayAudio(int trackNumber)
        {
            //int totalSize = 18;
            //int currentFileNumber = CurrentFileNumber();
            //Console.WriteLine($"the Currwnt File {currentFileNumber}");
            //int theDifference = trackNumber - currentFileNumber;
            //int numberOfClick = 0;
            //// 5 -3 =>2
            //// 3 -5 =>-2 -> 
            //// Total 6 suppose 
            //if (theDifference > 0)
            //    numberOfClick = theDifference;
            //else
            //    numberOfClick = (totalSize - currentFileNumber) + trackNumber;
            //for (int i = 0; i < numberOfClick; i++)
            //    NextAudio();

            //PlayAudio();
            //Console.WriteLine($"the One That will be played  {currentFileNumber}");


            //byte[] command = { 0xAA, 0x0C, 0x02, 0x00, 0x06, 0xBE };
            //serialPort.Write(command, 0, command.Length);
            //Console.WriteLine($"Playing audio file {trackNumber}");
            // Assuming the first track number is 1
            //int trackNumber = 1;

            // Convert the track number into high and low bytes
            //byte highByte = (byte)((trackNumber >> 8) & 0xFF);
            //byte lowByte = (byte)(trackNumber & 0xFF);

            //// Construct the command
            //byte[] command = { 0xAA, 0x07, 0x02, highByte, lowByte, 0xBB };

            //// Send the command to play the first audio track
            //serialPort.Write(command, 0, command.Length);
            //PlayAudio();



            // Convert the track number into high and low bytes
            byte highByte = (byte)((trackNumber >> 8) & 0xFF);
            byte lowByte = (byte)(trackNumber & 0xFF);

            // Construct the command
            byte[] command = { 0xAA, 0x07, 0x02, highByte, lowByte, 0xBB };

            // Calculate the checksum
            byte checksum = CalculateChecksum(command);

            // Set the checksum in the command
            command[command.Length - 1] = checksum;

            // Send the command to play the first audio track
            serialPort.Write(command, 0, command.Length);

            Console.WriteLine($"Playing the first audio track (Track Number: {trackNumber})");

        }
        public static byte CalculateChecksum(byte[] data)
        {
            int sum = 0;

            // Exclude the start byte (0xAA) and end byte (0xBB)
            for (int i = 1; i < data.Length - 1; i++)
            {
                sum += data[i];
            }

            // Take the least significant 8 bits
            byte checksum = (byte)(sum & 0xFF);

            return checksum;
        }

        public static void StopPlayback()
        {
            byte[] command = { 0xAA, 0x04, 0x00, 0xAE };
            serialPort.Write(command, 0, command.Length);
        }

        //Inquiry of current file number (0D) :->AA 0D 00 B7 
        public static int CurrentFileNumber()
        {
            byte[] command = { 0xAA, 0x0D, 0x00, 0xB7 };
            serialPort.Write(command, 0, command.Length);
            System.Threading.Thread.Sleep(100);
            byte[] response = new byte[serialPort.BytesToRead];
            serialPort.Read(response, 0, response.Length);
            if (response.Length == 6 && response[0] == 0xAA && response[1] == 0x0D)
            {
                int currentFileNumber = (response[3] << 8) | response[4];
                return currentFileNumber;
            }
            return 0;
        }
        public static void NextAudio()
        {
            byte[] command = { 0xAA, 0x06, 0x00, 0xB0 };
            serialPort.Write(command, 0, command.Length);
            Thread.Sleep(100);
        }
        public static void PlayAudio()
        {
            byte[] command = { 0xAA, 0x02, 0x00, 0xAC };
            serialPort.Write(command, 0, command.Length);
        }

        public static void Close()
        {
            serialPort.Close();
        }
    }
}
