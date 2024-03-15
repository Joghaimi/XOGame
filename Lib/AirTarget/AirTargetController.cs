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
        AirTargetModel _ShelfLight, _Target1, _Target2, _Target3, _Target4, _Target5;
        public AirTargetController(
            AirTargetModel ShelfLight,
            AirTargetModel Target1,
            AirTargetModel Target2,
            AirTargetModel Target3,
            AirTargetModel Target4,
            AirTargetModel Target5
            )
        {
            _ShelfLight = ShelfLight;
            _Target1 = Target1;
            _Target2 = Target2;
            _Target3 = Target3;
            _Target4 = Target4;
            _Target5 = Target5;
            MCP23Controller.PinModeSetup(_ShelfLight.Pin, PinMode.Output);
            MCP23Controller.PinModeSetup(_Target1.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target2.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target3.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target4.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target5.Pin, PinMode.Input);
        }
        public void Select(bool Selected)
        {
            MCP23Controller.Write(_ShelfLight.Pin, PinState.High);
            _ShelfLight.isSelected = true;

            _Target1.isSelected = true;
            _Target2.isSelected = true;
            _Target3.isSelected = true;
            _Target4.isSelected = true;
            _Target5.isSelected = true;
        }
        public (bool, int) TargetOneStatus()
        {
            int targetScore = 0;
            if (!MCP23Controller.Read(_Target1.Pin))
            {
                if (_Target1.isSelected)
                {
                    _Target1.isShoot = true;
                    return (true, targetScore);
                }
                else
                    return (true, -1 * targetScore);

            }
            else
                return (false, targetScore);
        }
        public (bool, int) TargetTwoStatus()
        {
            int targetScore = 0;
            if (!MCP23Controller.Read(_Target2.Pin))
            {
                if (_Target2.isSelected)
                {
                    _Target2.isShoot = true;
                    return (true, targetScore);
                }
                else
                    return (true, -1 * targetScore);
            }
            else
                return (false, targetScore);
        }
        public (bool, int) TargetThreeStatus()
        {
            int targetScore = 0;
            if (!MCP23Controller.Read(_Target3.Pin))
            {
                if (_Target3.isSelected)
                {
                    _Target3.isShoot = true;
                    return (true, targetScore);
                }
                else
                    return (true, -1 * targetScore);
            }
            else
                return (false, targetScore);
        }
        public (bool, int) TargetFourStatus()
        {
            int targetScore = 0;
            if (!MCP23Controller.Read(_Target4.Pin))
            {
                if (_Target4.isSelected)
                {
                    _Target4.isShoot = true;
                    return (true, targetScore);
                }
                else
                    return (true, -1 * targetScore);
            }
            else
                return (false, targetScore);
        }
        public (bool, int) TargetFiveStatus()
        {
            int targetScore = 0;
            if (!MCP23Controller.Read(_Target5.Pin))
            {
                if (_Target5.isSelected)
                {
                    _Target5.isShoot = true;
                    return (true, targetScore);
                }
                else
                    return (true, -1 * targetScore);
            }
            else
                return (false, targetScore);
        }
        public void UnSelectTarget()
        {
            MCP23Controller.Write(_ShelfLight.Pin, PinState.Low);

            _ShelfLight.isSelected = false;

            _Target1.isSelected = false;
            _Target2.isSelected = false;
            _Target3.isSelected = false;
            _Target4.isSelected = false;
            _Target5.isSelected = false;

            _Target1.isShoot = false;
            _Target2.isShoot = false;
            _Target3.isShoot = false;
            _Target4.isShoot = false;
            _Target5.isShoot = false;
        }
    }
}
