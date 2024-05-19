using Iot.Device.Mcp23xxx;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.PinMapping
{
    public static class HatInputPin
    {
        public static MCP23Pin IR1 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR2 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR3 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR4 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR5 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR6 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin IR7 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        //public static MCP23Pin IR8 = new MCP23Pin
        //{
        //    PinNumber = 7, // Set the PinNumber property
        //    Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
        //    port = Port.PortB
        //};

        public static MCP23Pin IR9 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR10 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR11 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR12 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR13 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR14 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR15 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        //public static MCP23Pin IR16 = new MCP23Pin
        //{
        //    PinNumber = 7, // Set the PinNumber property
        //    Chip = MCP23017.MCP2301721, // Set the Chip property with the MCP23017 instance
        //    port = Port.PortA
        //};
        public static MCP23Pin IR17 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR18 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR19 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IR20 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
    }
}
