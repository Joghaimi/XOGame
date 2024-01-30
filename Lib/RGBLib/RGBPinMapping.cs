using Iot.Device.Mcp23xxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public class RGBPinMapping
    {
        public int PinNumber { get; set; }
        public MCP23017 Chip { get; set; }
        public Port port { get; set; }
        public RGBPinMapping() { }
        public RGBPinMapping(int pinNumber, MCP23017 chip, Port port)
        {
            PinNumber = pinNumber;
            Chip = chip;
            this.port = port;
        }

    }
}
