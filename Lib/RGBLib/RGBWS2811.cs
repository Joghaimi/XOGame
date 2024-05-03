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


        public static void Commit()
        {
            python.InvokeMethod("commit");
        }





        public static void SetColor(int rgbNumber, RGBColor rGBColor)
        {
            if (rgbNumber < 0)
                return;
            PyObject[] pyParams = RGBColorToPyObj(rgbNumber, rGBColor);
            python.InvokeMethod("set_color", pyParams);
        }
        public static void SetColor(bool isActive, int rgbNumber, RGBColor rGBColor)
        {
            if (!isActive)
                return;
            SetColor(rgbNumber, rGBColor);
            Console.WriteLine($"rgbNumber {rgbNumber} rGBColor{rGBColor} ");
            //PyObject[] pyParams = RGBColorToPyObj(rgbNumber, rGBColor);
            //python.InvokeMethod("set_color", pyParams);
        }
        public static PyObject[] RGBColorToPyObj(int rgbNumber, RGBColor rGBColor)
        {
            int defaultWhite = 0;
            PyObject[] pyParams = new PyObject[5];
            pyParams[0] = rgbNumber.ToPython();
            pyParams[4] = defaultWhite.ToPython();

            switch (rGBColor)
            {
                case RGBColor.Red:

                    pyParams[1] = (255).ToPython();
                    pyParams[2] = (0).ToPython();
                    pyParams[3] = (0).ToPython();
                    break;
                case RGBColor.Green:
                    pyParams[1] = (0).ToPython();
                    pyParams[2] = (255).ToPython();
                    pyParams[3] = (0).ToPython();
                    break;
                case RGBColor.Blue:
                    pyParams[1] = (0).ToPython();
                    pyParams[2] = (0).ToPython();
                    pyParams[3] = (255).ToPython();
                    break;
                case RGBColor.Yellow:
                    pyParams[1] = (255).ToPython();
                    pyParams[2] = (255).ToPython();
                    pyParams[3] = (0).ToPython();
                    break;
                case RGBColor.Magenta:
                    pyParams[1] = (255).ToPython();
                    pyParams[2] = (0).ToPython();
                    pyParams[3] = (255).ToPython();
                    break;
                case RGBColor.Cyan:
                    pyParams[1] = (0).ToPython();
                    pyParams[2] = (255).ToPython();
                    pyParams[3] = (255).ToPython();
                    break;
                case RGBColor.White:
                    pyParams[1] = (255).ToPython();
                    pyParams[2] = (255).ToPython();
                    pyParams[3] = (255).ToPython();
                    break;
                case RGBColor.purple:
                    pyParams[1] = (128).ToPython();
                    pyParams[2] = (0).ToPython();
                    pyParams[3] = (128).ToPython();
                    break;
                case RGBColor.Off:
                    pyParams[1] = (0).ToPython();
                    pyParams[2] = (0).ToPython();
                    pyParams[3] = (0).ToPython();
                    break;
            }
            return pyParams;
        }

        [Obsolete]
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
