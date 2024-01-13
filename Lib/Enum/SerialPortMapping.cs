﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Enum
{
    public class SerialPortMapping
    {
        public static Dictionary<string, string> PortMap = new Dictionary<string, string>
        {
            { "Serial0", "/dev/ttyS0" },
            { "Serial2", "/dev/ttyS1" },
        };
    }
}
