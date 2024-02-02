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


        // Test 2 
        private static byte _clkPin;
        private static byte _dataPin;
        private static byte _numLEDs;
        private static byte[] _ledState;
        private const byte _CLKPulseDelay = 1; // Adjust the delay value as needed

        private const byte _clRed = 0;
        private const byte _clGreen = 1;
        private const byte _clBlue = 2;
        private static GPIOController _controller;

        public static void Init(byte clkPin, byte dataPin, byte numberOfLEDs)
        {
            _clkPin = clkPin;
            _dataPin = dataPin;
            _numLEDs = numberOfLEDs;
            _ledState = new byte[_numLEDs * 3];
            _controller = new GPIOController();
            _controller.Setup(_clkPin, PinMode.Output);
            _controller.Setup(_dataPin, PinMode.Output);

            for (byte i = 0; i < _numLEDs; i++)
                SetColorRGB(i, 0, 0, 0);

        }
        private static void Clk()
        {
            _controller.Write(_clkPin, false);
            Thread.Sleep(20);
            _controller.Write(_clkPin, true);
            Thread.Sleep(20);
        }
        private static void SendByte(byte b)
        {
            for (byte i = 0; i < 8; i++)
            {
                if ((b & 0x80) != 0)
                    _controller.Write(_clkPin, true);
                else
                    _controller.Write(_clkPin, false);
                Clk();
                b <<= 1;
            }
        }

        private static void SendColor(byte red, byte green, byte blue)
        {
            byte prefix = 0b11000000;
            if ((blue & 0x80) == 0) prefix |= 0b00100000;
            if ((blue & 0x40) == 0) prefix |= 0b00010000;
            if ((green & 0x80) == 0) prefix |= 0b00001000;
            if ((green & 0x40) == 0) prefix |= 0b00000100;
            if ((red & 0x80) == 0) prefix |= 0b00000010;
            if ((red & 0x40) == 0) prefix |= 0b00000001;
            SendByte(prefix);

            SendByte(blue);
            SendByte(green);
            SendByte(red);
        }
        public static void SetColorRGB(byte led, byte red, byte green, byte blue)
        {
            SendByte(0x00);
            SendByte(0x00);
            SendByte(0x00);
            SendByte(0x00);

            for (byte i = 0; i < _numLEDs; i++)
            {
                if (i == led)
                {
                    _ledState[i * 3 + _clRed] = red;
                    _ledState[i * 3 + _clGreen] = green;
                    _ledState[i * 3 + _clBlue] = blue;
                }

                SendColor(_ledState[i * 3 + _clRed],
                          _ledState[i * 3 + _clGreen],
                          _ledState[i * 3 + _clBlue]);
            }

            SendByte(0x00);
            SendByte(0x00);
            SendByte(0x00);
            SendByte(0x00);
        }



        public static void SetColorHSL(byte led, float hue, float saturation, float lightness)
        {
            float r, g, b;

            constrain(ref hue, 0.0f, 1.0f);
            constrain(ref saturation, 0.0f, 1.0f);
            constrain(ref lightness, 0.0f, 1.0f);

            if (saturation == 0.0f)
            {
                r = g = b = lightness;
            }
            else
            {
                float q = lightness < 0.5f ? lightness * (1.0f + saturation) : lightness + saturation - lightness * saturation;
                float p = 2.0f * lightness - q;
                r = hue2rgb(p, q, hue + 1.0f / 3.0f);
                g = hue2rgb(p, q, hue);
                b = hue2rgb(p, q, hue - 1.0f / 3.0f);
            }

            SetColorRGB(led, (byte)(255.0f * r), (byte)(255.0f * g), (byte)(255.0f * b));
        }

        private static void constrain(ref float value, float min, float max)
        {
            value = Math.Max(min, Math.Min(max, value));
        }
        private static void constrain(ref double value, double min, double max)
        {
            value = Math.Max(min, Math.Min(max, value));
        }
        private static void constrain(ref byte value, byte min, byte max)
        {
            value = Math.Max(min, Math.Min(max, value));
        }
        private static float hue2rgb(float p, float q, float t)
        {
            if (t < 0.0f)
                t += 1.0f;
            if (t > 1.0f)
                t -= 1.0f;
            if (t < 1.0f / 6.0f)
                return p + (q - p) * 6.0f * t;
            if (t < 1.0f / 2.0f)
                return q;
            if (t < 2.0f / 3.0f)
                return p + (q - p) * (2.0f / 3.0f - t) * 6.0f;

            return p;
        }











        //private static int CLKPin = 0;
        //private static int DataPin = 0;
        //private static GPIOController _controller;

        //public static void Init(int _clkPin, int _dataPin)
        //{
        //    CLKPin = _clkPin;
        //    DataPin = _dataPin;
        //    _controller = new GPIOController();
        //    _controller.Setup(CLKPin, PinMode.Output);
        //    _controller.Setup(DataPin, PinMode.Output);

        //}
        //public static void SetColor(RGBColor selectedColor)
        //{
        //    uint blue = 0;
        //    uint green = 0;
        //    uint red = 0;
        //    uint dx = 0;

        //    switch (selectedColor)
        //    {
        //        case RGBColor.Red:
        //            blue = 0;
        //            green = 0;
        //            red = 255;
        //            break;
        //        case RGBColor.Green:
        //            blue = 0;
        //            green = 255;
        //            red = 0;
        //            break;
        //        case RGBColor.Blue:
        //            blue = 255;
        //            green = 0;
        //            red = 0;
        //            break;
        //        case RGBColor.Off:
        //            blue = 0;
        //            green = 0;
        //            red = 0;
        //            break;

        //    }
        //    Console.WriteLine($"Color {selectedColor.ToString()} R:{red} G:{green} B:{blue}");
        //    BeginTransition();

        //    dx |= (uint)0x03 << 30;
        //    dx |= TakeAntiCode(blue) << 28;
        //    dx |= TakeAntiCode(green) << 26;
        //    dx |= TakeAntiCode(red) << 24;
        //    dx |= blue << 16;
        //    dx |= green << 8;
        //    dx |= red;
        //    DatSend(dx);
        //    EndTransition();



        //}
        //public static void DatSend(uint dx)
        //{
        //    for (int i = 0; i < 32; i++)
        //    {
        //        _controller.Write(DataPin, (dx & 0x80000000) != 0 ? true : false);
        //        dx <<= 1;
        //        ClkRise();
        //    }
        //}




        //private static void BeginTransition()
        //{
        //    Send32Zero();
        //}
        //private static void EndTransition()
        //{
        //    Send32Zero();
        //}
        //private static void Send32Zero()
        //{
        //    for (byte i = 0; i < 32; i++)
        //    {
        //        _controller.Write(DataPin, false);
        //        ClkRise();
        //    }
        //}
        //private static void ClkRise()
        //{
        //    _controller.Write(CLKPin, false);
        //    Thread.Sleep(20);
        //    _controller.Write(CLKPin, true);
        //    Thread.Sleep(20);
        //}
        //private static uint TakeAntiCode(uint dat)
        //{
        //    uint tmp = 0;
        //    if ((dat & 0x80) == 0)
        //        tmp |= 0x02;

        //    if ((dat & 0x40) == 0)
        //        tmp |= 0x01;
        //    return tmp;
        //}
    }
}
