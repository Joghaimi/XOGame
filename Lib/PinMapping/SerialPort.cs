using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.PinMapping
{
    public static class SerialPort
    {
        public static string Serial { get; } = "/dev/ttyS0";
        public static string Serial2 { get; } = "/dev/ttyAMA2";
    }
}
