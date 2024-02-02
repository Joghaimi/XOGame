using Library.GPIOLib;
using Library.PinMapping;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public static class RGBLight
    {
        private static int CLKPin = 0;
        private static int DataPin = 0;
        private static GPIOController _controller;

        public static void Init(int _clkPin, int _dataPin)
        {
            CLKPin = _clkPin;
            DataPin = _dataPin;
            _controller = new GPIOController();
            _controller.Setup(CLKPin, PinMode.Output);
            _controller.Setup(DataPin, PinMode.Output);

        }
        public static void SetColor(RGBColor selectedColor)
        {
            byte blue = 0;
            byte green = 0;
            byte red = 0;
            int dx = 0;

            switch (selectedColor)
            {
                case RGBColor.Red:
                    blue = 0;
                    green = 0;
                    red = 255;
                    break;
                case RGBColor.Green:
                    blue = 0;
                    green = 255;
                    red = 0;
                    break;
                case RGBColor.Blue:
                    blue = 255;
                    green = 0;
                    red = 0;
                    break;
                case RGBColor.Off:
                    blue = 0;
                    green = 0;
                    red = 0;
                    break;

            }

            dx |= (int)0x03 << 30;
            dx |= (int)TakeAntiCode(blue) << 28;
            dx |= (int)TakeAntiCode(green) << 26;
            dx |= (int)TakeAntiCode(red) << 24;
            dx |= (int)blue << 16;
            dx |= (int)green << 8;
            dx |= red;
            DatSend(dx);



        }
        public static void DatSend(int dx)
        {
            byte i;

            for (i = 0; i < 32; i++)
            {
                if ((dx & 0x80000000) != 0)
                {
                    _controller.Write(DataPin, true);
                }
                else
                {
                    _controller.Write(DataPin, false);
                }

                dx <<= 1;
                ClkRise();
            }
        }




        private static void BeginTransition()
        {
            Send32Zero();
        }
        private static void EndTransition()
        {
            Send32Zero();
        }
        private static void Send32Zero()
        {
            for (byte i = 0; i < 32; i++)
            {
                _controller.Write(DataPin, true);
                ClkRise();
            }
        }
        private static void ClkRise()
        {
            _controller.Write(CLKPin, false);
            Thread.Sleep(20);
            _controller.Write(CLKPin, true);
            Thread.Sleep(20);
        }
        private static byte TakeAntiCode(byte dat)
        {
            byte tmp = 0;
            if ((dat & 0x80) == 0)
                tmp |= 0x02;

            if ((dat & 0x40) == 0)
                tmp |= 0x01;
            return tmp;
        }
    }
}
