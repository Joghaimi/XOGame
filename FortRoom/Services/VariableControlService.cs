using Iot.Device.Mcp23xxx;
using Library;
using Library.GPIOLib;
using Library.RGBLib;

namespace FortRoom.Services
{
    public static class VariableControlService
    {
        public static bool IsTheGameStarted { get; set; } = false;
        public static bool IsTheGameFinished { get; set; } = false;
        public static bool IsTheirAnyOneInTheRoom { get; set; } = false;
        public static int TimeOfPressureHit { get; set; } = 0;
        public static int ActiveButtonPressed { get; set; } = 0;

        // Pin Numbers 
        // RGB1 
        public static MCP23Pin RGBR1 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG1 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB1 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB1 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        // RGB2 
        public static MCP23Pin RGBR2 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG2 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB2 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB2 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        // RGB3 
        public static MCP23Pin RGBR3 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG3 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB3 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB3 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        // RGB4 
        public static MCP23Pin RGBR4 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG4 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB4 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB4 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };

        // RGB5 
        public static MCP23Pin RGBR5 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG5 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB5 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB5 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };

        // RGB6
        public static MCP23Pin RGBR6 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG6 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB6 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB6 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };

        // RGB7 
        public static MCP23Pin RGBR7 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG7 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB7 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB7 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };

        // RGB8 
        public static MCP23Pin RGBR8 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG8 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB8 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB8 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };

        // RGB8 
        public static MCP23Pin RGBR9 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG9 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB9 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB9 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };

        // RGB8 
        public static MCP23Pin RGBR10 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG10 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB10 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB10 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };


        // RGB11
        public static MCP23Pin RGBR11 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG11 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301725, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB11 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB11 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };

        // RGB12
        public static MCP23Pin RGBR12 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG12 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB12 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB12 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };

        // RGB13
        public static MCP23Pin RGBR13 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG13 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBB13 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBPB13 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };

        // RGB14
        public static MCP23Pin RGBR14 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortB
        };
        public static MCP23Pin RGBG14 = new MCP23Pin
        {
            PinNumber = 0, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB14 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB14 = new MCP23Pin
        {
            PinNumber = 1, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        // RGB15
        public static MCP23Pin RGBR15 = new MCP23Pin
        {
            PinNumber = 2, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG15 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB15 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB15 = new MCP23Pin
        {
            PinNumber = 4, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        // RGB16
        public static MCP23Pin RGBR16 = new MCP23Pin
        {
            PinNumber = 5, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBG16 = new MCP23Pin
        {
            PinNumber = 6, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBB16 = new MCP23Pin
        {
            PinNumber = 7, // Set the PinNumber property
            Chip = MCP23017.MCP2301724, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };
        public static MCP23Pin RGBPB16 = new MCP23Pin
        {
            PinNumber = 3, // Set the PinNumber property
            Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
            port = Port.PortA
        };


    }
}
