using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Media
{
    public class AudioPlayer
    {
        SerialPort SerialPort;
        private Process audioProcess;
        string soundFilePath;
        
        // For the PI
        public void PIStartAudio(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Beeb:
                    soundFilePath = "file bath";
                    break;
                case SoundType.Warning:
                    break;
                default:
                    break;
            }


            if (audioProcess == null)
            {
                audioProcess = new Process();
                audioProcess.StartInfo.FileName = "aplay";
                audioProcess.StartInfo.Arguments = soundFilePath;
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.RedirectStandardOutput = true;
                audioProcess.StartInfo.RedirectStandardError = true;
                audioProcess.Start();
                Console.WriteLine("Audio playback started.");
                // Allow some time for playback before returning
                Thread.Sleep(5000);
            }
            else
            {
                Console.WriteLine("Audio is already playing.");
            }
        }
        public void PIStopAudio()
        {
            if (audioProcess != null && !audioProcess.HasExited)
            {
                audioProcess.Kill();
                audioProcess.WaitForExit();

                Console.WriteLine("Audio playback stopped.");
            }
            else
            {
                Console.WriteLine("No audio is currently playing.");
            }
        }
        // For the external chip
        public void JQ8400Init(string serialPort) {
            this.SerialPort = new SerialPort(serialPort, 9600); // Replace "COM1" with your actual serial port name
        }
        public void JQ8400PlayAudio() { 
        }
        public void JQ8400PauseAudio()
        {
        }
    }
}
