using NAudio.Wave;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Media
{
    public class AudioPlayer
    {
        private WaveOutEvent waveOut;
        private AudioFileReader audioFile;

        public void StartAudio(string soundFilePath)
        {
            if (waveOut == null)
            {
                waveOut = new WaveOutEvent();
                audioFile = new AudioFileReader(soundFilePath);

                waveOut.Init(audioFile);
                waveOut.Play();

                Console.WriteLine("Audio playback started.");
            }
            else
            {
                Console.WriteLine("Audio is already playing.");
            }
        }

        public void StopAudio()
        {
            if (waveOut != null)
            {
                waveOut.Stop();
                waveOut.Dispose();
                audioFile.Dispose();

                waveOut = null;
                audioFile = null;

                Console.WriteLine("Audio playback stopped.");
            }
            else
            {
                Console.WriteLine("No audio is currently playing.");
            }
        }
    }
}
