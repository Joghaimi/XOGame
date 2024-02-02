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
        public static MCP23Controller _MCP23Controller = new MCP23Controller(true);
        public static bool IsMCP23ControllerInit = false;
        public static bool IsSelected = false;
        MCP23Pin _LightPin, _InputPin;
        public AirTargetController(MCP23Pin LightPin, MCP23Pin InputPin)
        {
            _LightPin = LightPin;
            _InputPin = InputPin;
            if (!IsMCP23ControllerInit)
            {
                _MCP23Controller = new MCP23Controller(true);
                IsMCP23ControllerInit = true;
            }
            _MCP23Controller.PinModeSetup(_InputPin.Chip, _InputPin.port, _InputPin.PinNumber, PinMode.Input);
            _MCP23Controller.PinModeSetup(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinMode.Output);

        }
        public void Select(bool Selected)
        {
            IsSelected = Selected;
            if (IsSelected)
            {
                _MCP23Controller.Write(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinState.High);
            }
            else
            {
                _MCP23Controller.Write(_LightPin.Chip, _LightPin.port, _LightPin.PinNumber, PinState.Low);
            }
        }
        public bool Status()
        {
            return _MCP23Controller.Read(_InputPin.Chip, _InputPin.port, _InputPin.PinNumber);
        }

    }
}
