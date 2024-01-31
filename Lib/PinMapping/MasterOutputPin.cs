using Iot.Device.Mcp23xxx;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.PinMapping
{
    public class MasterOutputPin
    {
        public static MCP23Pin OUTPUT1 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT2 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT3 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT4 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT5 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT6 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT7 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT8 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
    }
}
