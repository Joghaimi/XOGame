using Iot.Device.Mcp3428;
using Library.GPIOLib;
using System.Device.Gpio;

namespace Library.DoorControl
{
    public static class DoorControl
    {
        public static void Status(MCP23Pin doorPin, bool status)
        {
            if (!status)
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Output);
                MCP23Controller.Write(doorPin, PinState.High);
            }
            else
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Input);
                MCP23Controller.Write(doorPin, PinState.Low);
            }
        }
        public static void Control(MCP23Pin doorPin, DoorStatus status)
        {
            if (status == DoorStatus.Open)
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Output);
                MCP23Controller.Write(doorPin, PinState.High);
            }
            else
            {
                MCP23Controller.PinModeSetup(doorPin, PinMode.Input);
                MCP23Controller.Write(doorPin, PinState.Low);
            }
        }
    }
}
