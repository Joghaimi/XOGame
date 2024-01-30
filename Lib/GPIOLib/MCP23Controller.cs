using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
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
    public class MCP23Controller
    {

        Mcp23017 mcp23017x20;
        Mcp23017 mcp23017x21;
        Mcp23017 mcp23017x22;
        Mcp23017 mcp23017x23;
        Mcp23017 mcp23017x24;
        Mcp23017 mcp23017x25;
        Mcp23017 mcp23017x26;
        Mcp23017 mcp23017x27;
        bool isItHat = false;
        public MCP23Controller(bool isItHat)
        {
            this.isItHat = isItHat;
            var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
            var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);
            mcp23017x20 = new Mcp23017(i2cDevicex20);

            if (this.isItHat)
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

                //var connectionSettingsx24 = new I2cConnectionSettings(1, 0x24);
                //var i2cDevicex24 = I2cDevice.Create(connectionSettingsx24);
                //mcp23017x24 = new Mcp23017(i2cDevicex24);

                //var connectionSettingsx25 = new I2cConnectionSettings(1, 0x25);
                //var i2cDevicex25 = I2cDevice.Create(connectionSettingsx25);
                //mcp23017x25 = new Mcp23017(i2cDevicex25);

                var connectionSettingsx26 = new I2cConnectionSettings(1, 0x26);
                var i2cDevicex26 = I2cDevice.Create(connectionSettingsx26);
                mcp23017x26 = new Mcp23017(i2cDevicex26);

                var connectionSettingsx27 = new I2cConnectionSettings(1, 0x27);
                var i2cDevicex27 = I2cDevice.Create(connectionSettingsx27);
                mcp23017x27 = new Mcp23017(i2cDevicex27);

            }




        }

        public void PinModeSetup(MCP23017 chip, Port Port, int PinNumber, PinMode Mode)
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

            //if (chip == MCP23017.MCP2301720)
            //{
            //    switch (Mode)
            //    {
            //        case PinMode.Input:
            //            // Configure the specified pin as an input
            //            mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
            //            break;

            //        case PinMode.Output:
            //            // Configure the specified pin as an output
            //            mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
            //            break;
            //        default:
            //            throw new ArgumentException("Invalid pin mode specified");
            //    }
            //}
            //else
            //{

            //    switch (Mode)
            //    {
            //        case PinMode.Input:
            //            mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
            //            break;
            //        case PinMode.Output:
            //            // Configure the specified pin as an output
            //            mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
            //            break;
            //        default:
            //            // Invalid pin mode
            //            throw new ArgumentException("Invalid pin mode specified");
            //    }
            //}
        }
        public bool Read(MCP23017 chip, Port Port, int PinNumber)
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
 
        public void Write(MCP23017 chip, Port Port, int PinNumber, PinState PinState)
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
                    Console.WriteLine(Convert.ToString(currentValue, 2).PadLeft(8, '0'));
                    break;
                case MCP23017.MCP2301721:
                    byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue2 |= (byte)(1 << PinNumber);
                    else
                        currentValue2 &= (byte)~(1 << PinNumber);
                    mcp23017x21.WriteByte(Register.GPIO, currentValue2, Port);
                    Console.WriteLine(Convert.ToString(currentValue2, 2).PadLeft(8, '0'));

                    break;

                case MCP23017.MCP2301722:
                    byte currentValue3 = mcp23017x22.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue3 |= (byte)(1 << PinNumber);
                    else
                        currentValue3 &= (byte)~(1 << PinNumber);
                    mcp23017x22.WriteByte(Register.GPIO, currentValue3, Port);
                    Console.WriteLine(Convert.ToString(currentValue3, 2).PadLeft(8, '0'));

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



        //var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
        //var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);

        //// Create an instance of MCP23017
        //var mcp23017x20 = new Mcp23017(i2cDevicex20);

        //// Set the I/O direction for PortA and PortB (0 means output, 1 means input)
        //mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortA);
        //mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortB);
        //while (true) {
        //mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
        //    Task.Delay(1000).Wait();
        //    mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
        //    Task.Delay(1000).Wait();
        //}




    }
}
