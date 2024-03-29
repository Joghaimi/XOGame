﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
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
