using Library;
using Library.Model;

namespace CatchyGame.Service
{
    public static class VariableControlService
    {
        public static int LevelTimeInSec = 60;


        public static string GetPlayersURL { get; set; } = "https://qmdug12n2k.execute-api.us-east-1.amazonaws.com/dev/getrfiddataofplayers";
        public static string UserName { get; set; } = "frenzi";
        public static string Password { get; set; } = "frenzi";

        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static Round GameRound = Round.Round1;


        public static int CurrentTime = 0;
        public static int GameTiming = 300000;
        public static int LevelTime = 60000;

        public static int PlayerScore = 0;
        public static bool IsGameTimerStarted = false;

        public static CatchyTeam Team { get; set; } = new CatchyTeam();

        public static RGBColor WormColor { get; set; } = RGBColor.Red;
        public static RGBColor DefaultColor { get; set; } = RGBColor.Blue;


    }
}
