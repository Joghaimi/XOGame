using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public class RGBButtonSneak : RGBButton
    {
        public int ActivatedBySneakIndex { get; set; } = -1;
        public RGBButtonSneak(
            MCP23Pin RPin, 
            MCP23Pin GPin, 
            MCP23Pin BPin, 
            MCP23Pin Button) : base(RPin, GPin, BPin, Button)
        {
        }
        public void Set(bool isSet , int SneakIndex)
        {
           this._isSet = isSet;
           this.ActivatedBySneakIndex= SneakIndex;
        }
        public int AssignedFor() { 
            return this.ActivatedBySneakIndex;
        }



    }
}
