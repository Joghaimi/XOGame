using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Python.Runtime;

namespace Library.RGBLib
{
    public static class RGBWS2811
    {
        private static PyObject python;
        public static void Init()
        {
            Runtime.PythonDLL = "/usr/lib/python3.11/config-3.11-aarch64-linux-gnu/libpython3.11.so";
            PythonEngine.Initialize();
            python = Py.Import("RGBLight");
            python.InvokeMethod("init_strip");
        }

        public static void SetColor(int rgbNumber, byte red, byte green, byte blue, byte white)
        {
            PyObject[] pyParams = new PyObject[5]; // This is an array of python parameters passed into a function
            pyParams[0] = rgbNumber.ToPython();
            pyParams[1] = red.ToPython();
            pyParams[2] = green.ToPython();
            pyParams[3] = blue.ToPython();
            pyParams[4] = white.ToPython();
            python.InvokeMethod("set_color", pyParams);
        }
    }
}
