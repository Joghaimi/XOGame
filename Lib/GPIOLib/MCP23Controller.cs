using Iot.Device.Mcp23xxx;
using System;
using System.Collections.Generic;
using System.Device.I2c;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public class MCP23Controller
    {

        Mcp23017 mcp23017x20; 
        Mcp23017 mcp23017x21;

        //var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
        //var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);

        //// Create an instance of MCP23017
        //var mcp23017x20 = new Mcp23017(i2cDevicex20);

        //// Set the I/O direction for PortA and PortB (0 means output, 1 means input)
        //mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortA);
        //mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortB);
        //while (true) {
        //    mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
        //    Task.Delay(1000).Wait();
        //    mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
        //    Task.Delay(1000).Wait();
        //}

        public void init() {
            var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
            var connectionSettingsx21 = new I2cConnectionSettings(1, 0x21);
            var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);
            var i2cDevicex21 = I2cDevice.Create(connectionSettingsx21);
            mcp23017x20 = new Mcp23017(i2cDevicex20);
            mcp23017x21 = new Mcp23017(i2cDevicex21);
        }
    }
}
