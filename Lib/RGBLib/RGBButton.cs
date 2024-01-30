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
        public static MCP23Controller _MCP23Controller = new MCP23Controller(true);
        public static bool IsMCP23ControllerInit = false;
        RGBColor _CurrnetColor;
        RGBPinMapping _PushButtonPin, _RGBRPin, _RGBGPin, _RGBBPin;
        //public RGBButton(MCP23017 chip, int pushButtonPin, int rGBRPin, int rGBGPin, int rGBBPin, Port port)
        public RGBButton(RGBPinMapping RPin, RGBPinMapping GPin, RGBPinMapping BPin, RGBPinMapping Button)
        {
            if (!IsMCP23ControllerInit)
            {
                _MCP23Controller = new MCP23Controller(true);
                IsMCP23ControllerInit = true;
            }
            _PushButtonPin = Button;
            _RGBRPin = RPin;
            _RGBGPin = GPin;
            _RGBBPin = BPin;
            // Set Pinout
            _MCP23Controller.PinModeSetup(_PushButtonPin.Chip, _PushButtonPin.port, _PushButtonPin.PinNumber, PinMode.Input);
            _MCP23Controller.PinModeSetup(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinMode.Output);
            _MCP23Controller.PinModeSetup(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinMode.Output);
            _MCP23Controller.PinModeSetup(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinMode.Output);
        }
        public void TurnColorOn(RGBColor selectedColor)
        {
            _CurrnetColor = selectedColor;
            switch (_CurrnetColor)
            {
                case RGBColor.Red:
                    _MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    _MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    _MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    break;
                case RGBColor.Green:
                    _MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    _MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    _MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    break;
                case RGBColor.Blue:
                    _MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    _MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    _MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    break;
                case RGBColor.Off:
                    _MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.Low);
                    _MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    _MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    break;
            }
        }
        public RGBColor CurrentColor()
        {
            return _CurrnetColor;
        }
        public bool CurrentStatus()
        {
            return _MCP23Controller.Read(_PushButtonPin.Chip, _PushButtonPin.port, _PushButtonPin.PinNumber);
        }
    }
}
