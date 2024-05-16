using Iot.Device.Mcp23xxx;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public class MCP23Chip
    {
        public MCP23017 Chip { get; set; }
        public bool isEnable { get; set; }
        public byte PortA { get; set; }
        public byte PortB { get; set; }
    }
}
