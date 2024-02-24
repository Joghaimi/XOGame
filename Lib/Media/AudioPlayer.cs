﻿using Iot.Device.Max7219;
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
        private static bool audioBessy = false;

        // For the PI
        public static void PIStartAudio(SoundType soundType)
        {
            try
            {
                //if (true)
                //{
                //audioBessy = true;
                soundFilePath = PISoundPath(soundType);
                audioProcess = new Process();
                audioProcess.StartInfo.FileName = "/bin/bash";
                audioProcess.StartInfo.Arguments = $"cvlc --vout none {soundFilePath}";
                audioProcess.StartInfo.UseShellExecute = false;
                audioProcess.StartInfo.RedirectStandardOutput = false;
                audioProcess.StartInfo.RedirectStandardError = false;
                audioProcess.Start();
                //using (StreamReader reader = audioProcess.StandardOutput)
                //{
                //    string result = reader.ReadToEnd();
                //}
                //    audioBessy = false;
                //}
                //else {
                //    Console.WriteLine($"Audio Bussy {soundType.ToString()}");

                //}


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
                    soundFilePath = "/home/fort/XOGame/audio";
                    break;
                case SoundType.Button:
                    soundFilePath = "/home/fort/XOGame/audio/Button.mp3";
                    break;
                case SoundType.Done:
                    break;
                case SoundType.Bonus:
                    soundFilePath = "/home/fort/XOGame/audio/GameBonus.mp3";
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
                    soundFilePath = "/home/fort/XOGame/audio/GameBonus.mp3";
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
                    soundFilePath = "/home/fort/XOGame/audio/Background.wav";

                    break;
                default:
                    break;
            }
            return soundFilePath;


        }
    }
}
