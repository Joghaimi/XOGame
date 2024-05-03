using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace Library.Model
{
    public class RGBWorm
    {

        public int startStripPixel = -5;
        public int endStripPixel = 0;
        public int startPixel = 0;
        public int endPixel = 0;
        public int initStartPixel = 0;
        public int initendPixel = 0;
        public RGBWorm(int startStripPixel, int endStripPixel, int length, int WormIndex)
        {
            this.startStripPixel = startStripPixel;
            this.endStripPixel = endStripPixel;

            int numberOfRGBLed = (endStripPixel - startStripPixel) / 5;
            Console.WriteLine($"Strip Led Number {numberOfRGBLed}");
            Random random = new Random();
            this.startPixel = (startStripPixel) + -1 * (numberOfRGBLed * WormIndex + random.Next(0, numberOfRGBLed));
            this.initStartPixel = startPixel;
            this.endPixel = this.startPixel - length;
            this.initendPixel = endPixel;
            Console.WriteLine($"Worm Start Pixel {endPixel}");
        }


        public void updateLength(int newLength)
        {
            this.endPixel = this.startPixel - newLength;
            this.initendPixel = endPixel;
        }
    }
}
