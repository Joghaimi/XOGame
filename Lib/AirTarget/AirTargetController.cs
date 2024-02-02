using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
using Iot.Device.Nmea0183.Ais;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.AirTarget
{
    public class AirTargetController
    {
        public static bool IsMCP23ControllerInit = false;
        public static bool IsSelected = false;
        MCP23Pin _LightPin, _InputPin;
        public AirTargetController(MCP23Pin LightPin, MCP23Pin InputPin)
        {
            _LightPin = LightPin;
            _InputPin = InputPin;

            MCP23Controller.PinModeSetup(_InputPin.Chip, _InputPin.port, _InputPin.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinMode.Output);

        }
        public void Select(bool Selected)
        {
            IsSelected = Selected;
            if (IsSelected)
            {
                MCP23Controller.Write(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinState.High);
            }
            else
            {
                MCP23Controller.Write(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinState.Low);
            }
        }
        public bool Status()
        {
            return MCP23Controller.Read(_InputPin.Chip, _InputPin.port, _InputPin.PinNumber);
        }

    }
}
