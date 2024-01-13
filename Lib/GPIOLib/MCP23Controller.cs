using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    public class MCP23Controller
    {
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
        }
    }
}
