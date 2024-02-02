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
        private Process audioProcess;
        string soundFilePath;

        // For the PI
        public void PIStartAudio(SoundType soundType)
        {
            switch (soundType)
            {
                case SoundType.Start:
                    soundFilePath = "file bath";
                    break;
                case SoundType.Start2:
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
    }
}
