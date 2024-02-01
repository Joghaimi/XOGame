using Iot.Device.Mcp23xxx;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.PinMapping
{
    public class MasterDI
    {
        public static MCP23Pin IN1 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN2 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN3 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN4 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN5 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN6 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN7 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin IN8 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301720, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };

        // RIP Sensor 
        public static int PIRPin1 = 26;
        public static int PIRPin2 = 19;
        public static int PIRPin3 = 13;
        public static int PIRPin4 = 6;
    }
}
