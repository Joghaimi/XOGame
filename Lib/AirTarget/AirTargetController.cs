﻿using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
using Iot.Device.Nmea0183.Ais;
using Library.GPIOLib;
using Library.PinMapping;
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
        public static bool TargetOneShootBefore, TargetTwoShootBefore, TargetThreeShootBefore, TargetFourShootBefore, TargetFiveShootBefore = false;
        AirTargetModel _ShelfLight, _Target1, _Target2, _Target3, _Target4, _Target5;
        public static int numberOfTargetDown = 0;
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

        }
        public void Init()
        {
            MCP23Controller.PinModeSetup(_ShelfLight.Pin, PinMode.Output);
            MCP23Controller.PinModeSetup(_Target1.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target2.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target3.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target4.Pin, PinMode.Input);
            MCP23Controller.PinModeSetup(_Target5.Pin, PinMode.Input);
        }
        public void test()
        {

            Console.WriteLine($"{MCP23Controller.Read(_Target1.Pin)} {MCP23Controller.Read(_Target2.Pin)} {MCP23Controller.Read(_Target3.Pin)} {MCP23Controller.Read(_Target4.Pin)} {MCP23Controller.Read(_Target5.Pin)}");


        }
        public void Select()
        {
            MCP23Controller.Write(_ShelfLight.Pin, PinState.High);
            _ShelfLight.isSelected = true;
            _Target1.isSelected = true;
            _Target2.isSelected = true;
            _Target3.isSelected = true;
            _Target4.isSelected = true;
            _Target5.isSelected = true;
        }

        public (bool status, int score, int numberOfHitTarget, int targetNumber) TargetStatus()
        {
            if (!_Target1.isShoot)
            {
                _Target1.isShoot = true;
                numberOfTargetDown++;
                if (!MCP23Controller.Read(_Target1.Pin))
                {
                    if (_Target1.isSelected)
                        return (true, _Target1.Score, numberOfTargetDown, 1);
                    else
                        return (true, -1 * _Target1.Score, numberOfTargetDown, 1);
                }
            }
            else if (!_Target2.isShoot)
            {
                _Target2.isShoot = true;
                numberOfTargetDown++;
                if (!MCP23Controller.Read(_Target2.Pin))
                {
                    if (_Target2.isSelected)
                        return (true, _Target2.Score, numberOfTargetDown, 2);
                    else
                        return (true, -1 * _Target2.Score, numberOfTargetDown, 2);
                }
            }
            else if (!_Target3.isShoot)
            {
                _Target3.isShoot = true;
                numberOfTargetDown++;
                if (!MCP23Controller.Read(_Target3.Pin))
                {
                    if (_Target3.isSelected)
                        return (true, _Target3.Score, numberOfTargetDown, 3);
                    else
                        return (true, -1 * _Target3.Score, numberOfTargetDown, 3);
                }
            }
            else if (!_Target3.isShoot)
            {
                _Target4.isShoot = true;
                numberOfTargetDown++;
                if (!MCP23Controller.Read(_Target4.Pin))
                {
                    if (_Target4.isSelected)
                        return (true, _Target4.Score, numberOfTargetDown, 4);
                    else
                        return (true, -1 * _Target4.Score, numberOfTargetDown, 4);
                }
            }

            else if (!_Target5.isShoot)
            {
                _Target5.isShoot = true;
                numberOfTargetDown++;
                if (!MCP23Controller.Read(_Target5.Pin))
                {
                    if (_Target5.isSelected)
                        return (true, _Target5.Score, numberOfTargetDown, 5);
                    else
                        return (true, -1 * _Target5.Score, numberOfTargetDown, 5);
                }
            }
            return (false, 0, numberOfTargetDown, 0);
        }

        //private static ref AirTargetModel returnAirTargetModel(Target target)
        //{
        //    return ref _Target1;
        //    switch (target)
        //    {
        //        case Target.Target1:
        //            ;
        //    }
        //}

        public (bool, int, int) TargetOneStatus()
        {

            if (!MCP23Controller.Read(_Target1.Pin) && !_Target1.isShoot)
            {
                if (!_Target1.isShoot)
                    numberOfTargetDown++;
                if (_Target1.isSelected)
                {
                    _Target1.isShoot = true;
                    return (true, _Target1.Score, numberOfTargetDown);
                }
                else
                    return (true, -1 * _Target1.Score, numberOfTargetDown);

            }
            else
                return (false, 0, numberOfTargetDown);
        }
        public (bool, int, int) TargetTwoStatus()
        {
            if (!MCP23Controller.Read(_Target2.Pin) && !_Target2.isShoot)
            {
                if (!_Target2.isShoot)
                    numberOfTargetDown++;
                _Target2.isShoot = true;
                if (_Target2.isSelected)
                    return (true, _Target2.Score, numberOfTargetDown);
                else
                    return (true, -1 * _Target2.Score, numberOfTargetDown);
            }
            else
                return (false, 0, numberOfTargetDown);
        }
        public (bool, int, int) TargetThreeStatus()
        {
            if (!MCP23Controller.Read(_Target3.Pin) && !_Target3.isShoot)
            {
                if (!_Target3.isShoot)
                    numberOfTargetDown++;
                _Target3.isShoot = true;
                if (_Target3.isSelected)
                    return (true, _Target3.Score, numberOfTargetDown);
                else
                    return (true, -1 * _Target3.Score, numberOfTargetDown);
            }
            else
                return (false, 0, numberOfTargetDown);
        }
        public (bool, int, int) TargetFourStatus()
        {
            if (!MCP23Controller.Read(_Target4.Pin) && !_Target4.isShoot)
            {
                if (!_Target4.isShoot)
                    numberOfTargetDown++;
                _Target4.isShoot = true;
                if (_Target4.isSelected)
                    return (true, _Target4.Score, numberOfTargetDown);
                else
                    return (true, -1 * _Target4.Score, numberOfTargetDown);
            }
            else
                return (false, 0, numberOfTargetDown);
        }
        public (bool, int, int) TargetFiveStatus()
        {
            if (!MCP23Controller.Read(_Target5.Pin) && !_Target5.isShoot)
            {
                if (!_Target5.isShoot)
                    numberOfTargetDown++;
                _Target5.isShoot = true;
                if (_Target5.isSelected)
                    return (true, _Target5.Score, numberOfTargetDown);
                else
                    return (true, -1 * _Target5.Score, numberOfTargetDown);
            }
            else
                return (false, 0, numberOfTargetDown);
        }
        public void UnSelectTarget(bool finishLevel)
        {
            MCP23Controller.Write(_ShelfLight.Pin, PinState.Low);

            _ShelfLight.isSelected = false;
            _Target1.isSelected = false;
            _Target2.isSelected = false;
            _Target3.isSelected = false;
            _Target4.isSelected = false;
            _Target5.isSelected = false;
            if (finishLevel)
            {
                _Target1.isShoot = false;
                _Target2.isShoot = false;
                _Target3.isShoot = false;
                _Target4.isShoot = false;
                _Target5.isShoot = false;
            }
        }
    }
}
