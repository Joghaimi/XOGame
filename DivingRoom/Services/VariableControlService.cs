using Iot.Device.Mcp23xxx;
using Library;
using Library.GPIOLib;
using Library.Model;
using Library.RGBLib;

namespace DivingRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false;
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfPressureHit { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        public static bool IsOccupied { get; set; }
        public static Team TeamScore { get; set; } = new Team();
        public static bool EnableGoingToTheNextRoom = false;
        public static bool IsGameTimerStarted = false;
        public static int RoomTiming = 360000;// Time in Mill
        public static bool IsRGBButtonServiceStarted = false;
        public static Round GameRound = Round.Round1;
        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static DoorStatus CurrentDoorStatus { get; set; } = DoorStatus.Open;
        public static DoorStatus NewDoorStatus { get; set; } = DoorStatus.Open;

        public static RGBColor DefaultColor = RGBColor.White;

      
        public static string NextRoomURL = "https://dark.local:7248/api/darkRoom/RoomStatus";
        public static string SendScoreToTheNextRoom = "https://dark.local:7248/api/darkRoom/ReceiveScore";



    }
}
