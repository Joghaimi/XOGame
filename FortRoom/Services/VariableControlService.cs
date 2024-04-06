using Iot.Device.Mcp23xxx;
using Library;
using Library.GPIOLib;
using Library.RGBLib;

namespace FortRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false; // Make It True Will stop the game and enable going to the next room
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfPressureHit { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        public static Team TeamScore { get; set; } = new Team();
        public static bool IsOccupied { get; set; }
        public static bool EnableGoingToTheNextRoom = false;
        public static int RoomTiming = 540000;// Time in Mill

        public static bool IsRGBButtonServiceStarted = false;
        public static bool IsMatServiceStarted = false;
        public static bool IsObstructionServiceStarted = false;
        public static bool IsGameTimerStarted = false;

        public static Round GameRound = Round.Round1;
        public static bool IsThingsChangedForTheNewRound = false;

        public static RGBColor DefaultColor = RGBColor.White;
        public static bool IsPressureMateActive = false;


        public static void ResetTheGame()
        {
        }
    }
}
