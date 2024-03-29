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

        public static void PIStartAudio(SoundType soundType, bool isLowVolum)
        {
            try
            {
                soundFilePath = PISoundPath(soundType);
                Process audioProcess = new Process();
                audioProcess.StartInfo.FileName = "/bin/bash";
                if (isLowVolum)
                    audioProcess.StartInfo.Arguments = $"cvlc --gain +0.9 --vout none --play-and-exit {soundFilePath}";
                else
                    audioProcess.StartInfo.Arguments = $"cvlc --vout none --play-and-exit {soundFilePath}";
                audioProcess.StartInfo.RedirectStandardOutput = true;
                audioProcess.StartInfo.RedirectStandardError = true;
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.CreateNoWindow = true;
                audioProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepton ==>" + ex.Message);

            }

        }



        public static void PIStartAudio(SoundType soundType)
        {
            try
            {
                soundFilePath = PISoundPath(soundType);
                Process audioProcess = new Process();
                audioProcess.StartInfo.FileName = "/bin/bash";
                audioProcess.StartInfo.Arguments = $"cvlc --vout none --play-and-exit {soundFilePath}";
                audioProcess.StartInfo.RedirectStandardOutput = true;
                audioProcess.StartInfo.RedirectStandardError = true;
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.CreateNoWindow = true;
                audioProcess.Start();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Excepton ==>" + ex.Message);

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

        }
        public static void PIForceStopAudio()
        {
            try
            {
                Process audioProcess = new Process();
                audioProcess.StartInfo.FileName = "/bin/bash";
                audioProcess.StartInfo.Arguments = $"sudo killall vlc";
                audioProcess.StartInfo.RedirectStandardOutput = true;
                audioProcess.StartInfo.RedirectStandardError = true;
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.StandardOutputEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.StandardErrorEncoding = System.Text.Encoding.UTF8;
                audioProcess.StartInfo.CreateNoWindow = true;
                audioProcess.Start();
            }
            catch
            {
            }

        }
        [Obsolete("Use PIStartAudio instead")]
        public static void PIBackgroundSound(SoundType soundType)
        {
            soundFilePath = PISoundPath(soundType);
            audioProcess = new Process();
            audioProcess.StartInfo.FileName = "/bin/bash";
            audioProcess.StartInfo.Arguments = $"cvlc --gain +0.9 --vout none --play-and-exit {soundFilePath}";
            audioProcess.StartInfo.UseShellExecute = false;
            audioProcess.StartInfo.RedirectStandardOutput = false;
            audioProcess.StartInfo.RedirectStandardError = false;
            audioProcess.Start();
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
                    soundFilePath = $"{HomePath()}/audio/SuccessButton.wav";
                    break;
                case SoundType.Finish:
                    soundFilePath = $"{HomePath()}/audio/Gamefinish.mp3";
                    break;
                case SoundType.MissionCompleted:
                    soundFilePath = $"{HomePath()}/audio/MissionAccomplished.wav";
                    break;
                case SoundType.Descend2:
                    break;
                case SoundType.Descend:
                    soundFilePath = $"{HomePath()}/audio/FailureIndicator.wav";
                    break;
                case SoundType.Background:
                    soundFilePath = $"{HomePath()}/audio/{BackgroundSound()}";
                    break;
                case SoundType.ScanId:
                    soundFilePath = $"{HomePath()}/audio/SuccessButton.wav";
                    break;
                case SoundType.instruction:
                    soundFilePath = $"{HomePath()}/audio/{instructionSound()}";
                    break;
                case SoundType.RoundOne:
                    soundFilePath = $"{HomePath()}/audio/Round1.wav";
                    break;
                case SoundType.RoundTwo:
                    soundFilePath = $"{HomePath()}/audio/Round2.wav";
                    break;
                case SoundType.RoundThree:
                    soundFilePath = $"{HomePath()}/audio/Round3.wav";
                    break;
                case SoundType.RoundFour:
                    soundFilePath = $"{HomePath()}/audio/Round4.wav";
                    break;
                case SoundType.RoundFive:
                    soundFilePath = $"{HomePath()}/audio/Round5.wav";
                    break;
                case SoundType.MissionAccomplished:
                    soundFilePath = $"{HomePath()}/audio/MissionAccomplished.wav";
                    break;
                case SoundType.DoubleScore:
                    soundFilePath = $"{HomePath()}/audio/ShootingDoubleScore.wav";
                    break;
                case SoundType.LightsChange:
                    soundFilePath = $"{HomePath()}/audio/LightsCahnge.wav";
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
                    return "/home/dark/XOGame";
                case Room.Diving:
                    return "/home/diving/XOGame";
                case Room.FloorIsLava:
                    return "/home/floor/XOGame";
                case Room.Gathering:
                    return "/home/gathering/XOGame";
                case Room.Shooting:
                    return "/home/shooting/XOGame";
                default:
                    return "/home/pi/XOGame";
            }
        }

        private static string BackgroundSound()
        {
            switch (_currentRoom)
            {
                case Room.Fort:
                    return "FortBackTrack.wav";
                case Room.Dark:
                    return "DarkBackTrack.wav";
                case Room.Diving:
                    return "DivingBackTrack.wav";
                case Room.FloorIsLava:
                    return "FloorIsLavaBackTrack.wav";
                case Room.Gathering:
                    return "GatheringBG.wav";
                case Room.Shooting:
                    return "ShootingBackTrack.wav";
                default:
                    return "/home/pi/XOGame";
            }
        }
        private static string instructionSound()
        {
            switch (_currentRoom)
            {
                case Room.Fort:
                    return "Instruction.wav";
                case Room.Dark:
                    return "DarkInstruction.wav";
                case Room.Diving:
                    return "DivingInstruction.wav";
                case Room.FloorIsLava:
                    return "FloorIsLavaInstruction.wav";
                case Room.Gathering:
                    return "GatheringBG.wav";
                case Room.Shooting:
                    return "ShootingInstruction.wav";
                default:
                    return "/home/pi/XOGame";
            }
        }

    }
}
