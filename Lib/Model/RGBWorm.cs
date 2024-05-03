using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class RGBWorm
    {

        public int startPixel = -5;
        public int endPixel = 0;
        public RGBWorm(int startPixel, int endPixel)
        {
            this.startPixel = startPixel;
            this.endPixel = endPixel;
        }
    }
}
