using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class RGBButtonSneakPixel
    {
        public int Pixel { get; set; }
        public RGBButtonSneak Button { get; set; }
        public RGBButtonSneakPixel(int pixel, RGBButtonSneak button)
        {
            Pixel = pixel;
            Button = button;
        }

    }
}
