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
        public static bool TargetOneShootBefore, TargetTwoShootBefore, TargetThreeShootBefore, TargetFourShootBefore, TargetFiveShootBefore = false;
        MCP23Pin _ShelfLightPin, _Target1Pin, _Target2Pin, _Target3Pin, _Target4Pin, _Target5Pin;
        public AirTargetController(
            MCP23Pin ShelfLightPin, MCP23Pin Target1Pin, MCP23Pin Target2Pin,
            MCP23Pin Target3Pin, MCP23Pin Target4Pin, MCP23Pin Target5Pin)
        {
            _ShelfLightPin = ShelfLightPin;
            _Target1Pin = Target1Pin;
            _Target2Pin = Target2Pin;
            _Target3Pin = Target3Pin;
            _Target4Pin = Target4Pin;
            _Target5Pin = Target5Pin;
            MCP23Controller.PinModeSetup(_ShelfLightPin, PinMode.Output);
            MCP23Controller.PinModeSetup(_Target1Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target2Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target3Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target4Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target5Pin, PinMode.Input);
        }
        public void Select(bool Selected)
        {
            IsSelected = Selected;
            if (IsSelected)
                MCP23Controller.Write(_ShelfLightPin, PinState.High);
            else
                MCP23Controller.Write(_ShelfLightPin, PinState.Low);
        }
        public bool TargetOneStatus()
        {
            bool statusShoot = !MCP23Controller.Read(_Target1Pin);
            if (statusShoot && !TargetOneShootBefore)
            {
                TargetOneShootBefore = true;
                return statusShoot;
            }
            else
                return false;

        }
        public bool TargetTwoStatus()
        {

            bool statusShoot = !MCP23Controller.Read(_Target2Pin);
            if (statusShoot && !TargetTwoShootBefore)
            {
                TargetTwoShootBefore = true;
                return statusShoot;
            }
            else
            {
                return false;
            }
        }
        public bool TargetThreeStatus()
        {
            bool statusShoot = !MCP23Controller.Read(_Target3Pin);
            if (statusShoot && !TargetThreeShootBefore)
            {
                TargetThreeShootBefore = true;
                return statusShoot;
            }
            else
            {
                return false;
            }
        }
        public bool TargetFourStatus()
        {
            bool statusShoot = !MCP23Controller.Read(_Target4Pin);
            if (statusShoot && !TargetFourShootBefore)
            {
                TargetFourShootBefore = true;
                return statusShoot;
            }
            else
            {
                return false;
            }
        }
        public bool TargetFiveStatus()
        {
            bool statusShoot = !MCP23Controller.Read(_Target5Pin);
            if (statusShoot && !TargetFourShootBefore)
            {
                TargetFourShootBefore = true;
                return statusShoot;
            }
            else
            {
                return false;
            }
        }
        public void resetTarget()
        {
            TargetOneShootBefore = false;
            TargetTwoShootBefore = false;
            TargetThreeShootBefore = false;
            TargetFourShootBefore = false;
            TargetFiveShootBefore = false;
        }
    }
}
