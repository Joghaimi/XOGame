namespace ShootingRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false;
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfGetTheTarget { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;
        // IR PIN OUT
        //public static MCP23Pin MCP23Pin = new MCP23Pin { port = , Chip =, PinNumber =}




    }
}
