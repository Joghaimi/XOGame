using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Media
{
    public static class ControlBackgroundAudio
    {
        public static void ControlRoomAudio(bool IsOccupied, bool IsTheGameStarted, bool IsTheGameFinished, bool thereAreInstructionSoundPlays, bool thereAreBackgroundSoundPlays)
        {
            // Control Background Audio
            if (IsOccupied && !IsTheGameStarted && !IsTheGameFinished && !thereAreInstructionSoundPlays)
            {
                //_logger.LogTrace("Start Instruction Audio");
                thereAreInstructionSoundPlays = true;
                AudioPlayer.PIBackgroundSound(SoundType.instruction);
            }
            else if (IsOccupied && IsTheGameStarted
                && !IsTheGameFinished
                && thereAreInstructionSoundPlays && !thereAreBackgroundSoundPlays)
            {
                // Stop Background Audio 
                //_logger.LogTrace("Stop Instruction Audio");
                thereAreInstructionSoundPlays = false;
                AudioPlayer.PIStopAudio();
                Thread.Sleep(500);
                // Start Background Audio
                //_logger.LogTrace("Start Background Audio");
                thereAreBackgroundSoundPlays = true;
                AudioPlayer.PIBackgroundSound(SoundType.Background);
            }
            else if (IsTheGameFinished && thereAreBackgroundSoundPlays)
            {
                // Game Finished .. 
                //_logger.LogTrace("Stop Background Audio");
                thereAreBackgroundSoundPlays = false;
                AudioPlayer.PIStopAudio();
            }
        }
    }
}
