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
        public static void Commit()
        {
            python.InvokeMethod("commit");
        }
        public static void SetColor(int rgbNumber, RGBColor rGBColor)
        {
        }

        //public static PyObject[] RGBColorToPyObj(int rgbNumber, RGBColor rGBColor)
        //{
        //    int defaultWhite = 0;
        //    PyObject[] pyParams = new PyObject[5]; // This is an array of python parameters passed into a function
        //    pyParams[0] = rgbNumber.ToPython();
        //    pyParams[4] = defaultWhite.ToPython(); ;

        //    switch (rGBColor)
        //    {
        //        case RGBColor.Red:

        //            pyParams[1] = (255).ToPython();
        //            pyParams[2] = (0).ToPython();
        //            pyParams[3] = (0).ToPython();
        //            break;
        //        case RGBColor.Green:
        //            pyParams[1] = (0).ToPython();
        //            pyParams[2] = (255).ToPython();
        //            pyParams[3] = (0).ToPython();
        //            break;
        //        case RGBColor.Blue:
        //            pyParams[1] = (0).ToPython();
        //            pyParams[2] = (0).ToPython();
        //            pyParams[3] = (255).ToPython();
        //            break;
        //        case RGBColor.Yellow:
        //            red = 255;
        //            green = 255;
        //            blue = 0;
        //            break;
        //        case RGBColor.Magenta:
        //            red = 255;
        //            green = 0;
        //            blue = 255;
        //            break;
        //        case RGBColor.Cyan:
        //            red = 0;
        //            green = 255;
        //            blue = 255;
        //            break;
        //        case RGBColor.White:
        //            red = 255;
        //            green = 255;
        //            blue = 255;
        //            break;
        //        case RGBColor.purple:
        //            red = 128;
        //            green = 0;
        //            blue = 128;
        //            break;
        //        case RGBColor.Off:
        //            blue = 0;
        //            green = 0;
        //            red = 0;
        //            break;

        //    }
        //    return pyParams;
        //}
    
    
    
    }
}
