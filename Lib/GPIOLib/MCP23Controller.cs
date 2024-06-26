﻿using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
using Iot.Device.Nmea0183.Ais;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Device.I2c;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public static class MCP23Controller
    {

        public static Mcp23017 mcp23017x20;
        public static Mcp23017 mcp23017x21;
        public static Mcp23017 mcp23017x22;
        public static Mcp23017 mcp23017x23;
        public static Mcp23017 mcp23017x24;
        public static Mcp23017 mcp23017x25;
        public static Mcp23017 mcp23017x26;
        public static Mcp23017 mcp23017x27;
        public static bool _isItHat = false;
        public static bool isLocked = false;



        [Obsolete("Not Recommended")]
        public static void Init(bool isItHat)
        {
            try
            {
                _isItHat = isItHat;
                var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
                var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);
                mcp23017x20 = new Mcp23017(i2cDevicex20);

                if (_isItHat)
                {
                    var connectionSettingsx21 = new I2cConnectionSettings(1, 0x21);
                    var i2cDevicex21 = I2cDevice.Create(connectionSettingsx21);
                    mcp23017x21 = new Mcp23017(i2cDevicex21);

                    var connectionSettingsx22 = new I2cConnectionSettings(1, 0x22);
                    var i2cDevicex22 = I2cDevice.Create(connectionSettingsx22);
                    mcp23017x22 = new Mcp23017(i2cDevicex22);

                    var connectionSettingsx23 = new I2cConnectionSettings(1, 0x23);
                    var i2cDevicex23 = I2cDevice.Create(connectionSettingsx23);
                    mcp23017x23 = new Mcp23017(i2cDevicex23);

                    var connectionSettingsx24 = new I2cConnectionSettings(1, 0x24);
                    var i2cDevicex24 = I2cDevice.Create(connectionSettingsx24);
                    mcp23017x24 = new Mcp23017(i2cDevicex24);

                    var connectionSettingsx25 = new I2cConnectionSettings(1, 0x25);
                    var i2cDevicex25 = I2cDevice.Create(connectionSettingsx25);
                    mcp23017x25 = new Mcp23017(i2cDevicex25);

                    var connectionSettingsx26 = new I2cConnectionSettings(1, 0x26);
                    var i2cDevicex26 = I2cDevice.Create(connectionSettingsx26);
                    mcp23017x26 = new Mcp23017(i2cDevicex26);

                    var connectionSettingsx27 = new I2cConnectionSettings(1, 0x27);
                    var i2cDevicex27 = I2cDevice.Create(connectionSettingsx27);
                    mcp23017x27 = new Mcp23017(i2cDevicex27);

                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MCP23Controller Init fn Exception {ex.Message}");
            }

        }

        public static void Init(Room currentRoom)
        {
            try
            {
                Console.WriteLine("Init 0x20");
                var connectionSettings = new I2cConnectionSettings(1, 0x20);
                var i2cDevice = I2cDevice.Create(connectionSettings);
                mcp23017x20 = new Mcp23017(i2cDevice);


                switch (currentRoom)
                {
                    case Room.Gathering:
                        break;
                    case Room.Shooting:
                        connectionSettings = new I2cConnectionSettings(1, 0x21);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x21 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x22);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x22 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x23);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x23 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x24);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x24 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x25);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x25 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x27);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x27 = new Mcp23017(i2cDevice);
                        break;
                    case Room.Dark:
                        connectionSettings = new I2cConnectionSettings(1, 0x21);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x21 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x22);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x22 = new Mcp23017(i2cDevice);
                        connectionSettings = new I2cConnectionSettings(1, 0x24);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x24 = new Mcp23017(i2cDevice);
                        break;
                    default:
                        Console.WriteLine("Init 0x21");
                        connectionSettings = new I2cConnectionSettings(1, 0x21);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x21 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x22");
                        connectionSettings = new I2cConnectionSettings(1, 0x22);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x22 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x23");
                        connectionSettings = new I2cConnectionSettings(1, 0x23);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x23 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x24");
                        connectionSettings = new I2cConnectionSettings(1, 0x24);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x24 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x25");
                        connectionSettings = new I2cConnectionSettings(1, 0x25);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x25 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x26");
                        connectionSettings = new I2cConnectionSettings(1, 0x26);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x26 = new Mcp23017(i2cDevice);
                        Console.WriteLine("Init 0x27");
                        connectionSettings = new I2cConnectionSettings(1, 0x27);
                        i2cDevice = I2cDevice.Create(connectionSettings);
                        mcp23017x27 = new Mcp23017(i2cDevice);
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MCP23Controller Init fn Exception {ex.Message}");

                Console.WriteLine($"Init Again");
                Thread.Sleep(1000);
                Init(currentRoom);

            }
        }

        private static ref Mcp23017 ReturnMCPChip(MCP23017 chip)
        {
            switch (chip)
            {
                case MCP23017.MCP2301720:
                    return ref mcp23017x20;
                case MCP23017.MCP2301721:
                    return ref mcp23017x21;
                case MCP23017.MCP2301722:
                    return ref mcp23017x22;
                case MCP23017.MCP2301723:
                    return ref mcp23017x23;
                case MCP23017.MCP2301724:
                    return ref mcp23017x24;
                case MCP23017.MCP2301725:
                    return ref mcp23017x25;
                case MCP23017.MCP2301726:
                    return ref mcp23017x26;
                case MCP23017.MCP2301727:
                    return ref mcp23017x27;
                default: throw new ArgumentException($"ReturnMCPChip : Not Valid {chip}");
            }
        }
        public static void PinModeSetup(MCP23Pin _MCP23Pin, PinMode Mode)
        {

            try
            {
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        if (Mode == PinMode.Input)
                            mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301721:
                        if (Mode == PinMode.Input)
                            mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301722:
                        if (Mode == PinMode.Input)
                            mcp23017x22.WriteByte(Register.IODIR, (byte)(mcp23017x22.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x22.WriteByte(Register.IODIR, (byte)(mcp23017x22.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301723:
                        if (Mode == PinMode.Input)
                            mcp23017x23.WriteByte(Register.IODIR, (byte)(mcp23017x23.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x23.WriteByte(Register.IODIR, (byte)(mcp23017x23.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301724:
                        if (Mode == PinMode.Input)
                            mcp23017x24.WriteByte(Register.IODIR, (byte)(mcp23017x24.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x24.WriteByte(Register.IODIR, (byte)(mcp23017x24.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301725:
                        if (Mode == PinMode.Input)
                            mcp23017x25.WriteByte(Register.IODIR, (byte)(mcp23017x25.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x25.WriteByte(Register.IODIR, (byte)(mcp23017x25.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301726:
                        if (Mode == PinMode.Input)
                            mcp23017x26.WriteByte(Register.IODIR, (byte)(mcp23017x26.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x26.WriteByte(Register.IODIR, (byte)(mcp23017x26.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301727:
                        if (Mode == PinMode.Input)
                            mcp23017x27.WriteByte(Register.IODIR, (byte)(mcp23017x27.ReadByte(Register.IODIR, _MCP23Pin.port) | (1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        else
                            mcp23017x27.WriteByte(Register.IODIR, (byte)(mcp23017x27.ReadByte(Register.IODIR, _MCP23Pin.port) & ~(1 << _MCP23Pin.PinNumber)), _MCP23Pin.port);
                        break;
                }

            }
            catch (Exception e) { Console.WriteLine($"Error MCP23Controller PinModeSetup : {e.Message}"); }
        }



        public static bool ReadDelay(MCP23Pin _MCP23Pin)
        {
            // The Working One
            try
            {
                bool returnStatus = false;
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte gpioStatus1 = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus1 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301721:
                        byte gpioStatus2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus2 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301722:
                        byte gpioStatus3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus3 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301723:
                        byte gpioStatus4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus4 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301724:
                        byte gpioStatus5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus5 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301725:
                        byte gpioStatus6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus6 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301726:
                        byte gpioStatus7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus7 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    case MCP23017.MCP2301727:
                        byte gpioStatus8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        returnStatus = ((gpioStatus8 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                        break;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
                Thread.Sleep(10);
                return returnStatus;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read MCPChip Exception {ex.Message} -- Try Again");
                Thread.Sleep(10);
                return ReadAgain(_MCP23Pin);
                return false;
            }
        }




        public static bool Read(MCP23Pin _MCP23Pin)
        {
            // The Working One
            try
            {
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte gpioStatus1 = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus1 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301721:
                        byte gpioStatus2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus2 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301722:
                        byte gpioStatus3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus3 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301723:
                        byte gpioStatus4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus4 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301724:
                        byte gpioStatus5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus5 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301725:
                        byte gpioStatus6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus6 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301726:
                        byte gpioStatus7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus7 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301727:
                        byte gpioStatus8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus8 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read MCPChip {_MCP23Pin.Chip.ToString()} Exception {ex.Message} -- Try Again");
                Thread.Sleep(10);
                return Read(_MCP23Pin);
            }
        }

        public static bool ReadAgain(MCP23Pin _MCP23Pin)
        {
            // The Working One
            try
            {
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte gpioStatus1 = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus1 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301721:
                        byte gpioStatus2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus2 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301722:
                        byte gpioStatus3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus3 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301723:
                        byte gpioStatus4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus4 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301724:
                        byte gpioStatus5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus5 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301725:
                        byte gpioStatus6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus6 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301726:
                        byte gpioStatus7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus7 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301727:
                        byte gpioStatus8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus8 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Read MCPChip {_MCP23Pin.Chip.ToString()} Exception {ex.Message}");
                return false;
            }
        }



        public static bool Read(MCP23Pin _MCP23Pin, bool preferState)
        {
            return Read(_MCP23Pin);
            try
            {
                // The Working One
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte gpioStatus1 = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus1 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301721:
                        byte gpioStatus2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus2 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301722:
                        byte gpioStatus3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus3 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301723:
                        byte gpioStatus4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus4 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301724:
                        byte gpioStatus5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus5 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301725:
                        byte gpioStatus6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus6 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301726:
                        byte gpioStatus7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus7 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    case MCP23017.MCP2301727:
                        byte gpioStatus8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        return ((gpioStatus8 >> _MCP23Pin.PinNumber) & 0x01) == 1;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception)
            {
                return preferState;
            }

        }


        public static void WriteDelay(MCP23Pin _MCP23Pin, PinState PinState)
        {
            try
            {
                if (_MCP23Pin.PinNumber < 0 || _MCP23Pin.PinNumber > 15)
                {
                    throw new ArgumentException("Invalid pin number");
                }
                Thread.Sleep(10);
                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte currentValue = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x20.WriteByte(Register.GPIO, currentValue, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301721:
                        byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue2 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue2 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x21.WriteByte(Register.GPIO, currentValue2, _MCP23Pin.port);
                        break;

                    case MCP23017.MCP2301722:
                        byte currentValue3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue3 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue3 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x22.WriteByte(Register.GPIO, currentValue3, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301723:
                        byte currentValue4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue4 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue4 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x23.WriteByte(Register.GPIO, currentValue4, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301724:
                        byte currentValue5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue5 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue5 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x24.WriteByte(Register.GPIO, currentValue5, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301725:
                        byte currentValue6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue6 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue6 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x25.WriteByte(Register.GPIO, currentValue6, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301726:
                        byte currentValue7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue7 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue7 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x26.WriteByte(Register.GPIO, currentValue7, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301727:
                        byte currentValue8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue8 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue8 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x27.WriteByte(Register.GPIO, currentValue8, _MCP23Pin.port);
                        break;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(10);
                Console.WriteLine($"MCP23 Write {ex.Message} -- Try other time");
                WriteAgain(_MCP23Pin, PinState);
            }
        }


        public static void Write(MCP23Pin _MCP23Pin, PinState PinState)
        {
            try
            {
                if (_MCP23Pin.PinNumber < 0 || _MCP23Pin.PinNumber > 15)
                {
                    throw new ArgumentException("Invalid pin number");
                }

                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte currentValue = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x20.WriteByte(Register.GPIO, currentValue, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301721:
                        byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue2 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue2 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x21.WriteByte(Register.GPIO, currentValue2, _MCP23Pin.port);
                        break;

                    case MCP23017.MCP2301722:
                        byte currentValue3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue3 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue3 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x22.WriteByte(Register.GPIO, currentValue3, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301723:
                        byte currentValue4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue4 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue4 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x23.WriteByte(Register.GPIO, currentValue4, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301724:
                        byte currentValue5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue5 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue5 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x24.WriteByte(Register.GPIO, currentValue5, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301725:
                        byte currentValue6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue6 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue6 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x25.WriteByte(Register.GPIO, currentValue6, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301726:
                        byte currentValue7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue7 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue7 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x26.WriteByte(Register.GPIO, currentValue7, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301727:
                        byte currentValue8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue8 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue8 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x27.WriteByte(Register.GPIO, currentValue8, _MCP23Pin.port);
                        break;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception ex)
            {
                Thread.Sleep(10);
                Console.WriteLine($"MCP23 {_MCP23Pin.Chip.ToString()} Write {ex.Message} -- Try other time");
                WriteAgain(_MCP23Pin, PinState);
            }
        }

        public static void WriteAgain(MCP23Pin _MCP23Pin, PinState PinState)
        {
            try
            {
                if (_MCP23Pin.PinNumber < 0 || _MCP23Pin.PinNumber > 15)
                {
                    throw new ArgumentException("Invalid pin number");
                }

                switch (_MCP23Pin.Chip)
                {
                    case MCP23017.MCP2301720:
                        byte currentValue = mcp23017x20.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x20.WriteByte(Register.GPIO, currentValue, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301721:
                        byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue2 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue2 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x21.WriteByte(Register.GPIO, currentValue2, _MCP23Pin.port);
                        break;

                    case MCP23017.MCP2301722:
                        byte currentValue3 = mcp23017x22.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue3 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue3 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x22.WriteByte(Register.GPIO, currentValue3, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301723:
                        byte currentValue4 = mcp23017x23.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue4 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue4 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x23.WriteByte(Register.GPIO, currentValue4, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301724:
                        byte currentValue5 = mcp23017x24.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue5 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue5 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x24.WriteByte(Register.GPIO, currentValue5, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301725:
                        byte currentValue6 = mcp23017x25.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue6 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue6 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x25.WriteByte(Register.GPIO, currentValue6, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301726:
                        byte currentValue7 = mcp23017x26.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue7 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue7 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x26.WriteByte(Register.GPIO, currentValue7, _MCP23Pin.port);
                        break;
                    case MCP23017.MCP2301727:
                        byte currentValue8 = mcp23017x27.ReadByte(Register.GPIO, _MCP23Pin.port);
                        if (PinState == PinState.High)
                            currentValue8 |= (byte)(1 << _MCP23Pin.PinNumber);
                        else
                            currentValue8 &= (byte)~(1 << _MCP23Pin.PinNumber);
                        mcp23017x27.WriteByte(Register.GPIO, currentValue8, _MCP23Pin.port);
                        break;
                    default:
                        throw new ArgumentException("Invalid Chip Selected");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MCP23 Write {ex.Message}");
            }
        }


        [Obsolete("Not Recommended To Use")]
        public static void PinModeSetup(MCP23017 chip, Port Port, int PinNumber, PinMode Mode)
        {

            switch (chip)
            {
                case MCP23017.MCP2301720:
                    if (Mode == PinMode.Input)
                        mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301721:
                    if (Mode == PinMode.Input)
                        mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301722:
                    if (Mode == PinMode.Input)
                        mcp23017x22.WriteByte(Register.IODIR, (byte)(mcp23017x22.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x22.WriteByte(Register.IODIR, (byte)(mcp23017x22.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301723:
                    if (Mode == PinMode.Input)
                        mcp23017x23.WriteByte(Register.IODIR, (byte)(mcp23017x23.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x23.WriteByte(Register.IODIR, (byte)(mcp23017x23.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301724:
                    if (Mode == PinMode.Input)
                        mcp23017x24.WriteByte(Register.IODIR, (byte)(mcp23017x24.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x24.WriteByte(Register.IODIR, (byte)(mcp23017x24.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301725:
                    if (Mode == PinMode.Input)
                        mcp23017x25.WriteByte(Register.IODIR, (byte)(mcp23017x25.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x25.WriteByte(Register.IODIR, (byte)(mcp23017x25.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301726:
                    if (Mode == PinMode.Input)
                        mcp23017x26.WriteByte(Register.IODIR, (byte)(mcp23017x26.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x26.WriteByte(Register.IODIR, (byte)(mcp23017x26.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
                case MCP23017.MCP2301727:
                    if (Mode == PinMode.Input)
                        mcp23017x27.WriteByte(Register.IODIR, (byte)(mcp23017x27.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                    else
                        mcp23017x27.WriteByte(Register.IODIR, (byte)(mcp23017x27.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                    break;
            }
        }
        [Obsolete("Not Recommended To Use")]
        public static void Write(MCP23017 chip, Port Port, int PinNumber, PinState PinState)
        {
            if (PinNumber < 0 || PinNumber > 15)
            {
                throw new ArgumentException("Invalid pin number");
            }
            switch (chip)
            {
                case MCP23017.MCP2301720:
                    byte currentValue = mcp23017x20.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue |= (byte)(1 << PinNumber);
                    else
                        currentValue &= (byte)~(1 << PinNumber);
                    mcp23017x20.WriteByte(Register.GPIO, currentValue, Port);
                    break;
                case MCP23017.MCP2301721:
                    byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue2 |= (byte)(1 << PinNumber);
                    else
                        currentValue2 &= (byte)~(1 << PinNumber);
                    mcp23017x21.WriteByte(Register.GPIO, currentValue2, Port);
                    break;

                case MCP23017.MCP2301722:
                    byte currentValue3 = mcp23017x22.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue3 |= (byte)(1 << PinNumber);
                    else
                        currentValue3 &= (byte)~(1 << PinNumber);
                    mcp23017x22.WriteByte(Register.GPIO, currentValue3, Port);
                    break;
                case MCP23017.MCP2301723:
                    byte currentValue4 = mcp23017x23.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue4 |= (byte)(1 << PinNumber);
                    else
                        currentValue4 &= (byte)~(1 << PinNumber);
                    mcp23017x23.WriteByte(Register.GPIO, currentValue4, Port);
                    break;
                case MCP23017.MCP2301724:
                    byte currentValue5 = mcp23017x24.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue5 |= (byte)(1 << PinNumber);
                    else
                        currentValue5 &= (byte)~(1 << PinNumber);
                    mcp23017x24.WriteByte(Register.GPIO, currentValue5, Port);
                    break;
                case MCP23017.MCP2301725:
                    byte currentValue6 = mcp23017x25.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue6 |= (byte)(1 << PinNumber);
                    else
                        currentValue6 &= (byte)~(1 << PinNumber);
                    mcp23017x25.WriteByte(Register.GPIO, currentValue6, Port);
                    break;
                case MCP23017.MCP2301726:
                    byte currentValue7 = mcp23017x26.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue7 |= (byte)(1 << PinNumber);
                    else
                        currentValue7 &= (byte)~(1 << PinNumber);
                    mcp23017x26.WriteByte(Register.GPIO, currentValue7, Port);
                    break;
                case MCP23017.MCP2301727:
                    byte currentValue8 = mcp23017x27.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue8 |= (byte)(1 << PinNumber);
                    else
                        currentValue8 &= (byte)~(1 << PinNumber);
                    mcp23017x27.WriteByte(Register.GPIO, currentValue8, Port);
                    break;
                default:
                    throw new ArgumentException("Invalid Chip Selected");
            }
        }
        [Obsolete("Not Recommended To Use")]
        public static bool Read(MCP23017 chip, Port Port, int PinNumber)
        {

            switch (chip)
            {
                case MCP23017.MCP2301720:
                    byte gpioStatus1 = mcp23017x20.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus1 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301721:
                    byte gpioStatus2 = mcp23017x21.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus2 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301722:
                    byte gpioStatus3 = mcp23017x22.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus3 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301723:
                    byte gpioStatus4 = mcp23017x23.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus4 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301724:
                    byte gpioStatus5 = mcp23017x24.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus5 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301725:
                    byte gpioStatus6 = mcp23017x25.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus6 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301726:
                    byte gpioStatus7 = mcp23017x26.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus7 >> PinNumber) & 0x01) == 1;
                case MCP23017.MCP2301727:
                    byte gpioStatus8 = mcp23017x27.ReadByte(Register.GPIO, Port);
                    return ((gpioStatus8 >> PinNumber) & 0x01) == 1;
                default:
                    throw new ArgumentException("Invalid Chip Selected");
            }
        }
    }
}

