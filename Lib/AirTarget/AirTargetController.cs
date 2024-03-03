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
        MCP23Pin _ShelfLightPin, _Target1Pin, _Target2Pin, _Target3Pin, _Target4Pin, _Target5Pin;
        public AirTargetController(MCP23Pin ShelfLightPin, MCP23Pin Target1Pin, MCP23Pin Target2Pin,
                                   MCP23Pin Target3Pin, MCP23Pin Target4Pin, MCP23Pin Target5Pin)
        {
            _ShelfLightPin = ShelfLightPin;
            _Target1Pin = Target1Pin;
            _Target2Pin = Target2Pin;
            _Target3Pin = Target3Pin;
            _Target4Pin = Target4Pin;
            _Target5Pin = Target5Pin;
            MCP23Controller.PinModeSetup(_ShelfLightPin.Chip, _ShelfLightPin.port, _ShelfLightPin.PinNumber, PinMode.Output);
            //MCP23Controller.PinModeSetup(_Target1Pin.Chip, _Target1Pin.port, _Target1Pin.PinNumber, PinMode.Input);
            //MCP23Controller.PinModeSetup(_Target2Pin.Chip, _Target2Pin.port, _Target2Pin.PinNumber, PinMode.Input);
            //MCP23Controller.PinModeSetup(_Target3Pin.Chip, _Target3Pin.port, _Target3Pin.PinNumber, PinMode.Input);
            //MCP23Controller.PinModeSetup(_Target4Pin.Chip, _Target4Pin.port, _Target4Pin.PinNumber, PinMode.Input);
            //MCP23Controller.PinModeSetup(_Target5Pin.Chip, _Target5Pin.port, _Target5Pin.PinNumber, PinMode.Input);
        }
        public void Select(bool Selected)
        {
            IsSelected = Selected;
            if (IsSelected)
                MCP23Controller.Write(_ShelfLightPin.Chip, _ShelfLightPin.port, _ShelfLightPin.PinNumber, PinState.High);
            else
                MCP23Controller.Write(_ShelfLightPin.Chip, _ShelfLightPin.port, _ShelfLightPin.PinNumber, PinState.Low);
        }
        public bool TargetOneStatus()
        {
            return MCP23Controller.Read(_Target1Pin.Chip, _Target1Pin.port, _Target1Pin.PinNumber);
        }
        public bool TargetTwoStatus()
        {
            return MCP23Controller.Read(_Target2Pin.Chip, _Target2Pin.port, _Target2Pin.PinNumber);
        }
        public bool TargetThreeStatus()
        {
            return MCP23Controller.Read(_Target3Pin.Chip, _Target3Pin.port, _Target3Pin.PinNumber);
        }
        public bool TargetFourStatus()
        {
            return MCP23Controller.Read(_Target4Pin.Chip, _Target4Pin.port, _Target4Pin.PinNumber);
        }
        public bool TargetFiveStatus()
        {
            return MCP23Controller.Read(_Target5Pin.Chip, _Target5Pin.port, _Target5Pin.PinNumber);
        }
    }
}
