using Iot.Device.Mcp3428;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public static class RelayController
    {
        public static void Status(MCP23Pin relayPin, bool status)
        {
            if (status)
            {
                MCP23Controller.PinModeSetup(relayPin, PinMode.Output);
                MCP23Controller.Write(relayPin, PinState.High);
            }
            else
            {
                MCP23Controller.PinModeSetup(relayPin, PinMode.Input);
                MCP23Controller.Write(relayPin, PinState.Low);
            }
        }
    }
}
