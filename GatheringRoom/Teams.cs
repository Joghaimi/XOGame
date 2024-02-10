namespace GatheringRoom
{
    //public static class Teams
    //{
    //    public static string Name { get; set; } = "";
    //    public static List<Player> player { get; set; } = new List<Player>();
    //    public static int FortRoomScore { get; set; }
    //    public static int ShootingRoomScore { get; set; }
    //    public static int DivingRoomScore { get; set; }
    //    public static int DarkRoomScore { get; set; }
    //    public static int FloorIsLavaRoomScore { get; set; }
    //}
    //public class Player
    //{
    //    public string Id { get; set; }
    //    public string FirstName { get; set; }
    //    public string SecoundName { get; set; }
    //}
    //public static bool TeamIsSignedIn = false;

    public class Player
    {
        public string Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
    public class Team
    {
        public string Name { get; set; } = "";
        public List<Player> player { get; set; } = new List<Player>();
        public int FortRoomScore { get; set; }
        public int ShootingRoomScore { get; set; }
        public int DivingRoomScore { get; set; }
        public int DarkRoomScore { get; set; }
        public int FloorIsLavaRoomScore { get; set; }
    }



}