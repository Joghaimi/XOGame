using Library;

namespace GatheringRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false;
        public static bool EnableGoingToTheNextRoom { get; set; } = false;


        // Pins
        public static int PIRPin1 = 26;
        public static int PIRPin2 = 19;
        public static int PIRPin3 = 13;
        public static int PIRPin4 = 6;

        public static int LightSwitch = 6;
        public static int DoorRelay = 6;




        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfGetTheTarget { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        public static Team TeamScore { get; set; } = new Team();
        public static string NextRoomURL { get;} = "https://foort.local:7248/api/FortRoom/ReceiveScore";
        
        public static string AuthorizationURL { get;} = "https://qmdug12n2k.execute-api.us-east-1.amazonaws.com/dev/applogin";
        public static string UserName { get; } = "frenzi";
        public static string Password { get; } = "frenzi";
        
        public static string UserInfoURL { get; } = "https://gotp4qetdh.execute-api.us-east-1.amazonaws.com/dev/getrfiddata";


        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static DoorStatus CurrentDoorStatus { get; set; } = DoorStatus.Open;
        public static DoorStatus NewDoorStatus { get; set; } = DoorStatus.Open;
    }
}
