using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public class RGBButton
    {
        public static MCP23Controller _MCP23Controller = new MCP23Controller();
        public static bool IsMCP23ControllerInit = false;
        MCP23017 _Chip;
        Port _Port;
        RGBColor _CurrnetColor;
        int _PushButtonPin, _RGBRPin, _RGBGPin, _RGBBPin;
        public RGBButton(MCP23017 chip, int pushButtonPin, int rGBRPin, int rGBGPin, int rGBBPin, Port port)
        {
            if (!IsMCP23ControllerInit)
            {
                _MCP23Controller = new MCP23Controller();
                IsMCP23ControllerInit = true;
            }
            _Port = port;
            _PushButtonPin = pushButtonPin;
            _RGBRPin = rGBRPin;
            _RGBGPin = rGBGPin;
            _RGBBPin = rGBBPin;
            _Chip = chip;
            // Set Pinout
            _MCP23Controller.PinModeSetup(chip, _Port, _PushButtonPin, PinMode.Input);
            _MCP23Controller.PinModeSetup(chip, _Port, _RGBRPin, PinMode.Output);
            _MCP23Controller.PinModeSetup(chip, _Port, _RGBGPin, PinMode.Output);
            _MCP23Controller.PinModeSetup(chip, _Port, _RGBBPin, PinMode.Output);
        }
        public void TurnColorOn(RGBColor selectedColor)
        {
            _CurrnetColor = selectedColor;
            switch (_CurrnetColor)
            {
                case RGBColor.Red:
                    _MCP23Controller.Write(_Chip, _Port, _RGBRPin, PinState.High);
                    _MCP23Controller.Write(_Chip, _Port, _RGBBPin, PinState.Low);
                    _MCP23Controller.Write(_Chip, _Port, _RGBGPin, PinState.Low);
                    break;
                case RGBColor.Green:
                    _MCP23Controller.Write(_Chip, _Port, _RGBGPin, PinState.High);
                    _MCP23Controller.Write(_Chip, _Port, _RGBBPin, PinState.Low);
                    _MCP23Controller.Write(_Chip, _Port, _RGBRPin, PinState.Low);
                    break;
                case RGBColor.Blue:
                    _MCP23Controller.Write(_Chip, _Port, _RGBBPin, PinState.High);
                    _MCP23Controller.Write(_Chip, _Port, _RGBGPin, PinState.Low);
                    _MCP23Controller.Write(_Chip, _Port, _RGBRPin, PinState.Low);
                    break;
                case RGBColor.Off:
                    _MCP23Controller.Write(_Chip, _Port, _RGBBPin, PinState.Low);
                    _MCP23Controller.Write(_Chip, _Port, _RGBGPin, PinState.Low);
                    _MCP23Controller.Write(_Chip, _Port, _RGBRPin, PinState.Low);
                    break;
            }
        }
        public RGBColor CurrentColor()
        {
            return _CurrnetColor;
        }
        public bool CurrentStatus()
        {
            return _MCP23Controller.Read(_Chip, _Port, _PushButtonPin);
        }
    }
}
