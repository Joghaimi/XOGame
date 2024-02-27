using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enum
{
    public static class RGBColorMapping
    {
        public static RGBColor[] GetRGBColors(RGBColor theMixColor) {
            switch (theMixColor)
            {
                case RGBColor.purple:
                    return new RGBColor[] { RGBColor.Red , RGBColor.Blue };
                case RGBColor.Yellow:
                    return new RGBColor[] { RGBColor.Red, RGBColor.Green };
                case RGBColor.White:
                    return new RGBColor[] { RGBColor.Red, RGBColor.Green , RGBColor.Blue };
                case RGBColor.Cyan:
                    return new RGBColor[] { RGBColor.Blue, RGBColor.Green };
                case RGBColor.Magenta:
                    return new RGBColor[] { RGBColor.Red, RGBColor.Blue };
                default:
                    return new RGBColor[] { };
            }
        }
    }
}
