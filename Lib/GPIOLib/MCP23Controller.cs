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

        public MCP23Controller()
        {
            var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
            //var connectionSettingsx21 = new I2cConnectionSettings(1, 0x21);
            var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);
            //var i2cDevicex21 = I2cDevice.Create(connectionSettingsx21);
            mcp23017x20 = new Mcp23017(i2cDevicex20);
            //mcp23017x21 = new Mcp23017(i2cDevicex21);
        }

        public void PinModeSetup(MCP23017 chip, Port Port, int PinNumber, PinMode Mode)
        {

            if (chip == MCP23017.MCP2301720)
            {
                switch (Mode)
                {
                    case PinMode.Input:
                        // Configure the specified pin as an input
                        mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                        break;

                    case PinMode.Output:
                        // Configure the specified pin as an output
                        mcp23017x20.WriteByte(Register.IODIR, (byte)(mcp23017x20.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                        break;
                    default:
                        throw new ArgumentException("Invalid pin mode specified");
                }
            }
            else
            {

                switch (Mode)
                {
                    case PinMode.Input:
                        mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) | (1 << PinNumber)), Port);
                        break;
                    case PinMode.Output:
                        // Configure the specified pin as an output
                        mcp23017x21.WriteByte(Register.IODIR, (byte)(mcp23017x21.ReadByte(Register.IODIR, Port) & ~(1 << PinNumber)), Port);
                        break;
                    default:
                        // Invalid pin mode
                        throw new ArgumentException("Invalid pin mode specified");
                }
            }
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
                    //byte currentValue = mcp23017x20.ReadByte(Register.GPIO, Port);
                    //if (PinState == PinState.High)
                    //    currentValue |= (byte)(1 << PinNumber);
                    //else
                    //    currentValue &= (byte)~(1 << PinNumber);
                    mcp23017x20.WriteByte(Register.GPIO, 0b11111111, Port);
                    byte currentValue = mcp23017x20.ReadByte(Register.GPIO, Port);
                    Console.WriteLine(Convert.ToString(currentValue, 2).PadLeft(8, '0'));
                    break;
                case MCP23017.MCP2301721:
                    byte currentValue2 = mcp23017x21.ReadByte(Register.GPIO, Port);
                    if (PinState == PinState.High)
                        currentValue2 |= (byte)(1 << PinNumber);
                    else
                        currentValue2 &= (byte)~(1 << PinNumber);
                    mcp23017x21.WriteByte(Register.GPIO, currentValue2, Port);
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
