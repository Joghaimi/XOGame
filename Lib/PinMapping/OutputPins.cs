using Iot.Device.Mcp23xxx;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.PinMapping
{
    public class OutputPins
    {
        public static MCP23Pin OUTPUT1 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT2 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT3 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT4 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT5 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT6 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin OUTPUT7 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301722, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };



        //test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 4, PinMode.Output); LED #12
        //test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 5, PinMode.Output); LED #11 
        //test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 6, PinMode.Output); LED #10
        //test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 7, PinMode.Output); LED #09
        //test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 0, PinMode.Output); Output 8


        //// U29  ---> 23
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 0, PinMode.Output); Output 20
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 1, PinMode.Output); Output 19
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 2, PinMode.Output); Output 18
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 3, PinMode.Output); Output 17
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 4, PinMode.Output); Output 16
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 5, PinMode.Output); Output 15
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 6, PinMode.Output); Output 14
        //test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 7, PinMode.Output); Output 13




    }
}
