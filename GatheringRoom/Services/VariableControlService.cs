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
        public static string NextRoomURL { get; set; } = "'http://fort.local:5000/ReceiveScore";

    }
}
