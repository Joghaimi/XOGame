using Iot.Device.Mcp23xxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public class MCP23Pin
    {
        public int PinNumber { get; set; }
        public MCP23017 Chip { get; set; }
        public Port port { get; set; }
    }
}
