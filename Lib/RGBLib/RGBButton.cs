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
        public RGBColor _CurrnetColor;
        public bool _isSet = false;
        public bool _isBlocked = false;
        public int stripIndex = -1;
        public bool clickedForOnce = false;
        MCP23Pin _PushButtonPin, _RGBRPin, _RGBGPin, _RGBBPin;
        public RGBButton(MCP23Pin RPin, MCP23Pin GPin, MCP23Pin BPin, MCP23Pin Button)
        {

            _PushButtonPin = Button;
            _RGBRPin = RPin;
            _RGBGPin = GPin;
            _RGBBPin = BPin;
            // Set Pinout
            MCP23Controller.PinModeSetup(_PushButtonPin, PinMode.Input);
            MCP23Controller.PinModeSetup(_RGBGPin, PinMode.Output);
            MCP23Controller.PinModeSetup(_RGBRPin, PinMode.Output);
            MCP23Controller.PinModeSetup(_RGBBPin, PinMode.Output);
            this.TurnColorOn(RGBColor.Off);
        }
        public void TurnColorOn(RGBColor selectedColor)
        {
            if (_CurrnetColor == selectedColor)
                return;
            _CurrnetColor = selectedColor;
            switch (_CurrnetColor)
            {
                case RGBColor.Red:
                    MCP23Controller.Write(_RGBRPin, PinState.Low);
                    MCP23Controller.Write(_RGBBPin, PinState.High);
                    MCP23Controller.Write(_RGBGPin, PinState.High);
                    break;
                case RGBColor.Green:
                    MCP23Controller.Write(_RGBGPin, PinState.Low);
                    MCP23Controller.Write(_RGBBPin, PinState.High);
                    MCP23Controller.Write(_RGBRPin, PinState.High);
                    break;
                case RGBColor.Blue:
                    MCP23Controller.Write(_RGBBPin, PinState.Low);
                    MCP23Controller.Write(_RGBRPin, PinState.High);
                    MCP23Controller.Write(_RGBGPin, PinState.High);
                    break;
                case RGBColor.purple:
                    MCP23Controller.Write(_RGBBPin, PinState.Low);
                    MCP23Controller.Write(_RGBRPin, PinState.Low);
                    MCP23Controller.Write(_RGBGPin, PinState.High);
                    break;
                case RGBColor.Yellow:
                    MCP23Controller.Write(_RGBBPin, PinState.High);
                    MCP23Controller.Write(_RGBRPin, PinState.Low);
                    MCP23Controller.Write(_RGBGPin, PinState.Low);
                    break;
                case RGBColor.White:
                    MCP23Controller.Write(_RGBBPin, PinState.Low);
                    MCP23Controller.Write(_RGBRPin, PinState.Low);
                    MCP23Controller.Write(_RGBGPin, PinState.Low);
                    break;
                case RGBColor.Turquoise:
                    MCP23Controller.Write(_RGBBPin, PinState.Low);
                    MCP23Controller.Write(_RGBRPin, PinState.High);
                    MCP23Controller.Write(_RGBGPin, PinState.Low);
                    break;
                case RGBColor.Off:
                    MCP23Controller.Write(_RGBRPin, PinState.High);
                    MCP23Controller.Write(_RGBBPin, PinState.High);
                    MCP23Controller.Write(_RGBGPin, PinState.High);
                    break;
            }
        }

        public RGBColor CurrentColor()
        {
            return _CurrnetColor;
        }
        public bool CurrentStatus()
        {
            return MCP23Controller.Read(_PushButtonPin);
        }

        public bool CurrentStatusWithCheckForDelay()
        {
            if (_isBlocked)
                return true;
            return MCP23Controller.Read(_PushButtonPin);
        }
        public void BlockForASec()
        {
            Task.Run(async () =>
            {
                _isBlocked = true;
                await Task.Delay(1000);
                _isBlocked = false;
            });
        }
        public void BlockForATimeInMs(int time)
        {
            Task.Run(async () =>
            {
                _isBlocked = true;
                await Task.Delay(time);
                _isBlocked = false;
            });
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
