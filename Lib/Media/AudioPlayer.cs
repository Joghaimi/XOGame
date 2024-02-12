using Iot.Device.Max7219;
using Mono.Unix.Native;
using NAudio.SoundFont;
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
        private static Process audioProcess;
        public static string soundFilePath;

        // For the PI
        public static void PIStartAudio(SoundType soundType)
        {

            switch (soundType)
            {
                case SoundType.Ticking:
                    soundFilePath = "/XOGame/audio";
                    break;
                case SoundType.Button:
                    soundFilePath = "/XOGame/audio/Button.mp3";
                    break;
                case SoundType.Done:
                    break;
                case SoundType.Bonus:
                    soundFilePath = "/XOGame/audio/GameBonus.mp3";
                    break;
                case SoundType.Finish:
                    break;
                case SoundType.Start:
                    break;
                case SoundType.Start2:
                    break;
                case SoundType.LaserScan:
                    break;
                case SoundType.LevelWin:
                    break;
                case SoundType.MarioJump:
                    break;
                case SoundType.MissionCompleted:
                    break;
                case SoundType.Descend2:
                    break;
                case SoundType.Descend:
                    break;
                case SoundType.GameOver:
                    break;
                case SoundType.GameScoreTally:
                    break;
                case SoundType.WinSquare:
                    break;
                case SoundType.Winner2:
                    break;
                case SoundType.Winner1:
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
