using Library;
using Library.Model;

namespace CatchyGame.Service
{
    public static class VariableControlService
    {
        public static int LevelTimeInSec = 120;


        public static string GetPlayersURL { get; set; } = "https://qmdug12n2k.execute-api.us-east-1.amazonaws.com/dev/getrfiddataofplayers";
        public static string UserName { get; set; } = "frenzi";
        public static string Password { get; set; } = "frenzi";

        public static GameStatus GameStatus { get; set; } = GameStatus.Empty;
        public static Round GameRound = Round.Round1;


        public static int CurrentTime = 0;
        public static int TopScore = 0;
        public static string TopScoreTeam = "";
        public static int GameTiming = 120000;
        public static int LevelTime = 60000;

        public static int PlayerScore = 0;
        public static bool IsGameTimerStarted = false;

        public static CatchyTeam Team { get; set; } = new CatchyTeam();

        public static RGBColor WormColor { get; set; } = RGBColor.purple;
        public static RGBColor DefaultColor { get; set; } = RGBColor.Off;

        public static int DefaultWarmLength { get; set; } = 5;

        public static RGBColor PlayerOneWarmColor { get; set; } = RGBColor.Yellow;
        public static RGBColor PlayerOneStripDefaultColor { get; set; } = RGBColor.Off;

        public static int PlayerOneWarmLength { get; set; } = 5;
        public static RGBColor PlayerTwoWarmColor { get; set; } = RGBColor.Green;
        public static RGBColor PlayerTwoStripDefaultColor { get; set; } = RGBColor.Off;
        public static int PlayerTwoWarmLength { get; set; } = 5;

        public static RGBColor PlayerThreeWarmColor { get; set; } = RGBColor.Blue;
        public static RGBColor PlayerThreeStripDefaultColor { get; set; } = RGBColor.Off;
        public static int PlayerThreeWarmLength { get; set; } = 5;

        public static RGBColor PlayerFourWarmColor { get; set; } = RGBColor.Red;
        public static RGBColor PlayerFourStripDefaultColor { get; set; } = RGBColor.Off;
        public static int PlayerFourWarmLength { get; set; } = 5;

        // Strip Defenition 
        public static int StripOneStartIndex { get; set; } = 0;
        public static int StripOneEndIndex { get; set; } = 211;
        
        public static int StripTwoStartIndex { get; set; } = 212;
        public static int StripTwoEndIndex { get; set; } = 438;
        
        public static int StripThreeStartIndex { get; set; } = 439;
        public static int StripThreeEndIndex { get; set; } = 664;
        
        public static int StripFourStartIndex { get; set; } = 665;
        public static int StripFourEndIndex { get; set; } = 880;

        public static int StripFiveStartIndex { get; set; } = 881;
        public static int StripFiveEndIndex { get; set; } = 1074;
        
        public static int StripSixStartIndex { get; set; } = 1075;
        public static int StripSixEndIndex { get; set; } = 1240;

        public static int StripSevenStartIndex { get; set; } = 1241;
        public static int StripSevenEndIndex { get; set; } = 1431;
        public static RGBColor StripSevenDefaultColor { get; set; } = RGBColor.White;

        public static RGBColor StripFiveWarmColor { get; set; } = RGBColor.purple;
        public static RGBColor StripFiveStripDefaultColor { get; set; } = RGBColor.Off;
        public static RGBColor StripSixWarmColor { get; set; } = RGBColor.purple;
        public static RGBColor StripSixStripDefaultColor { get; set; } = RGBColor.Off;



    }
}
