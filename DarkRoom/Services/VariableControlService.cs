using Iot.Device.Mcp23xxx;
using Library;
using Library.GPIOLib;
using Library.Model;
using Library.RGBLib;

namespace DarkRoom.Services
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

        public static int RoomTiming = 360000;// Time in Mill
        public static int TimeOfGetTheTarget { get; set; } = 0;
        public static int ActiveTargetPressed { get; set; } = 0;

        public static bool IsAirTargetServiceStarted = false;

        // IR PIN OUT

        public static int GameScore { get; set; } = 0;
        public static bool IsGameTimerStarted = false;
        public static Round GameRound = Round.Round1;
        public static RGBColor DefaultColor = RGBColor.Blue;

        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static DoorStatus CurrentDoorStatus { get; set; } = DoorStatus.Open;
        public static DoorStatus NewDoorStatus { get; set; } = DoorStatus.Open;

        public static string NextRoomURL = "https://floor.local:7248/api/floorislava/RoomStatus";
        public static string SendScoreToTheNextRoom = "https://floor.local:7248/api/floorislava/ReceiveScore";



    }
}
