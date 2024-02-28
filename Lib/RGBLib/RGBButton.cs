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

        public static bool IsMCP23ControllerInit = false;
        RGBColor _CurrnetColor;
        bool _isSet = false;
        MCP23Pin _PushButtonPin, _RGBRPin, _RGBGPin, _RGBBPin;
        public RGBButton(MCP23Pin RPin, MCP23Pin GPin, MCP23Pin BPin, MCP23Pin Button)
        {

            _PushButtonPin = Button;
            _RGBRPin = RPin;
            _RGBGPin = GPin;
            _RGBBPin = BPin;
            // Set Pinout
            MCP23Controller.PinModeSetup(_PushButtonPin.Chip, _PushButtonPin.port, _PushButtonPin.PinNumber, PinMode.Input);
            MCP23Controller.PinModeSetup(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinMode.Output);
            MCP23Controller.PinModeSetup(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinMode.Output);
            MCP23Controller.PinModeSetup(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinMode.Output);
            this.TurnColorOn(RGBColor.Off);
        }
        public void TurnColorOn(RGBColor selectedColor)
        {
            _CurrnetColor = selectedColor;
            switch (_CurrnetColor)
            {
                case RGBColor.Red:
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.High);
                    break;
                case RGBColor.Green:
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    break;
                case RGBColor.Blue:
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.High);
                    break;
                case RGBColor.purple:
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.High);
                    break;
                case RGBColor.Turquoise:
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.Low);
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.Low);
                    break;
                case RGBColor.Off:
                    MCP23Controller.Write(_RGBRPin.Chip, _RGBRPin.port, _RGBRPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBBPin.Chip, _RGBBPin.port, _RGBBPin.PinNumber, PinState.High);
                    MCP23Controller.Write(_RGBGPin.Chip, _RGBGPin.port, _RGBGPin.PinNumber, PinState.High);
                    break;
            }
        }

        public RGBColor CurrentColor()
        {
            return _CurrnetColor;
        }
        public bool CurrentStatus()
        {
            return MCP23Controller.Read(_PushButtonPin.Chip, _PushButtonPin.port, _PushButtonPin.PinNumber);
        }
        public bool isSet()
        {
            return _isSet;
        }
        public void Set(bool isSet)
        {
            _isSet = isSet;
        }
    }
}
