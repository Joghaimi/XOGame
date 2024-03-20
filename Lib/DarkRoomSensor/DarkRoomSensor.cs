using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DarkRoomSensor
{
    public class DarkRoomSensor
    {
        public MCP23Pin Pin { get; set; }
        public int Score { get; set; }
        public bool isSelected { get; set; } = false;
        public bool isShoot { get; set; } = false;
        public DarkRoomSensor(MCP23Pin Pin, int Score)
        {
            this.Pin = Pin;
            this.Score = Score;
        }
    }

}
