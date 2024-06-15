using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public class Spike
    {
        private int startIndex;
        private int endIndex;
        private RGBColor color;
        private int currentLocation = -1;
        public Spike(int startIndex, int endIndex, RGBColor color)
        {
            this.startIndex = startIndex;
            this.endIndex = endIndex;
            this.color = color;
        }
        public void MoveForward()
        {
            if (currentLocation == -1)
            {
                currentLocation = startIndex;
                RGBWS2811.SetColor(true, currentLocation, color);
            }
            else
            {
                if (currentLocation < endIndex)
                {
                    currentLocation++;
                    RGBWS2811.SetColor(true, currentLocation, color);
                }
                else currentLocation = -1;
            }
        }
    }
}
