using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class RGBButtonPixel
    {
        public int Pixel { get; set; }
        public RGBButton Button { get; set; }
        //public int WormIndex = -1;
        //public int stripIndex = -1;
        public RGBButtonPixel(int pixel, RGBButton button)
        {
            Pixel = pixel;
            Button = button;
        }

    }
}
