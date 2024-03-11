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
        public static Room _currentRoom = Room.Gathering;
        public static void Init(Room currentRoom)
        {
            _currentRoom = currentRoom;
        }

        // For the PI
        public static void PIStartAudio(SoundType soundType)
        {
            return;
            try
            {
                //if (true)
                //{
                //audioBessy = true;
                soundFilePath = PISoundPath(soundType);
                Process audioProcess = new Process();
                audioProcess.StartInfo.FileName = "/bin/bash";
                audioProcess.StartInfo.Arguments = $"cvlc --vout none --play-and-exit {soundFilePath}";
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.RedirectStandardOutput = false;
                audioProcess.StartInfo.RedirectStandardError = false;
                audioProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

            }

        }
        public static void PIStopAudio()
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
        public static void PIBackgroundSound(SoundType soundType)
        {
            return;

            soundFilePath = PISoundPath(soundType);
            //Console.WriteLine(soundFilePath);
            audioProcess = new Process();
            audioProcess.StartInfo.FileName = "/bin/bash";
            audioProcess.StartInfo.Arguments = $"cvlc -R --gain +0.5 --vout none {soundFilePath}";
            audioProcess.StartInfo.UseShellExecute = false;
            audioProcess.StartInfo.RedirectStandardOutput = false;
            audioProcess.StartInfo.RedirectStandardError = false;
            audioProcess.Start();
            Console.WriteLine("Audio playback started.");
        }
        private static string PISoundPath(SoundType soundType)
        {
            string soundFilePath = "";

            switch (soundType)
            {
                case SoundType.Ticking:
                    soundFilePath = $"{HomePath()}/audio";
                    break;
                case SoundType.Button:
                    soundFilePath = $"{HomePath()}/audio/Button.mp3";
                    break;
                case SoundType.Done:
                    break;
                case SoundType.Bonus:
                    soundFilePath = $"{HomePath()}/audio/GameBonus.mp3";
                    break;
                case SoundType.Finish:
                    soundFilePath = $"{HomePath()}/audio/Gamefinish.mp3";
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
                    soundFilePath = $"{HomePath()}/audio/GameBonus.mp3";
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
                case SoundType.Background:
                    soundFilePath = $"{HomePath()}/audio/{BackgroundSound()}";

                    break;
                default:
                    break;
            }
            return soundFilePath;


        }

        private static string HomePath()
        {
            switch (_currentRoom)
            {
                case Room.Fort:
                    return "/home/fort/XOGame";
                case Room.Dark:
                    return "/home/Dark/XOGame";
                case Room.Diving:
                    return "/home/diving/XOGame";
                case Room.FloorIsLava:
                    return "/home/floor/XOGame";
                case Room.Gathering:
                    return "/home/Gathering/XOGame";
                case Room.Shooting:
                    return "/home/Shooting/XOGame";
                default:
                    return "/home/pi/XOGame";
            }
        }

        private static string BackgroundSound()
        {
            switch (_currentRoom)
            {
                case Room.Fort:
                    return "Background.wav";
                case Room.Dark:
                    return "/home/Dark/XOGame";
                case Room.Diving:
                    return "/home/diving/XOGame";
                case Room.FloorIsLava:
                    return "/home/floor/XOGame";
                case Room.Gathering:
                    return "/home/Gathering/XOGame";
                case Room.Shooting:
                    return "/home/Shooting/XOGame";
                default:
                    return "/home/pi/XOGame";
            }
        }

    }
}
