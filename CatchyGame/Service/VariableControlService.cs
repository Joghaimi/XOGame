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
        public static RGBColor DefaultColor { get; set; } = RGBColor.Off;

        public static int DefaultWarmLength { get; set; } = 5;

        public static int StripOneStartIndex { get; set; } = 0;
        public static int StripOneEndIndex { get; set; } = 211;
        public static RGBColor PlayerOneWarmColor { get; set; } = RGBColor.Red;
        public static RGBColor PlayerOneStripDefaultColor { get; set; } = RGBColor.Blue;
        public static int PlayerOneWarmLength { get; set; } = 5;

        public static RGBColor PlayerTwoWarmColor { get; set; } = RGBColor.Red;
        public static RGBColor PlayerTwoStripDefaultColor { get; set; } = RGBColor.Blue;
        public static int PlayerTwoWarmLength { get; set; } = 5;

        public static RGBColor PlayerThreeWarmColor { get; set; } = RGBColor.Red;
        public static RGBColor PlayerThreeStripDefaultColor { get; set; } = RGBColor.Blue;
        public static int PlayerThreeWarmLength { get; set; } = 5;

        public static RGBColor PlayerFourWarmColor { get; set; } = RGBColor.Red;
        public static RGBColor PlayerFourStripDefaultColor { get; set; } = RGBColor.Blue;
        public static int PlayerFourWarmLength { get; set; } = 5;



    }
}
