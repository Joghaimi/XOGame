﻿using Library.GPIOLib;

namespace Library.AirTarget
{
    public class AirTargetModel
    {
        public MCP23Pin Pin { get; set; }
        public int Score{ get; set; }
        public bool isSelected { get; set; }=false;
        public bool isShoot { get; set; }=false;
    }
}
