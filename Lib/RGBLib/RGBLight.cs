﻿using Library.GPIOLib;
using Library.PinMapping;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;
using System.Device.I2c;
using System.Device;
using Iot.Device.Common;
using Library.Delay;

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
            uint blue = 0;
            uint green = 0;
            uint red = 0;
            uint dx = 0;

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
            Console.WriteLine($"Color {selectedColor.ToString()} R:{red} G:{green} B:{blue}");
            dx |= (uint)0x03 << 30;
            dx |= TakeAntiCode(blue) << 28;
            dx |= TakeAntiCode(green) << 26;
            dx |= TakeAntiCode(red) << 24;
            dx |= blue << 16;
            dx |= green << 8;
            dx |= red;
            DatSend(dx);

        }
        public static void DatSend(uint dx)
        {
            for (int i = 0; i < 32; i++)
            {
                _controller.Write(DataPin, (dx & 0x80000000) != 0 ? true : false);
                dx <<= 1;
                ClkRise();
            }
        }




        public static void BeginTransition()
        {
            Send32Zero();
        }
        public static void EndTransition()
        {
            Send32Zero();
        }
        private static void Send32Zero()
        {
            for (byte i = 0; i < 32; i++)
            {
                _controller.Write(DataPin, false);
                ClkRise();
            }
        }
        private static void ClkRise()
        {
            _controller.Write(CLKPin, false);
            //Thread.Sleep(0);
            //Thread.sleepM
            //Sleep(20);
            DelayLib.DelayMicroseconds(20, false);
            _controller.Write(CLKPin, true);
            DelayLib.DelayMicroseconds(20, false);
            //Sleep(20);
        }
        private static uint TakeAntiCode(uint dat)
        {
            uint tmp = 0;
            if ((dat & 0x80) == 0)
                tmp |= 0x02;

            if ((dat & 0x40) == 0)
                tmp |= 0x01;
            return tmp;
        }
        public static void Sleep(int microseconds)
        {
            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();

            while (stopwatch.ElapsedMicroseconds() < microseconds)
            {
                // Busy waiting
            }
        }
        // Extension method to get elapsed microseconds
        public static long ElapsedMicroseconds(this Stopwatch stopwatch)
        {
            return stopwatch.ElapsedTicks * 1_000_000 / Stopwatch.Frequency;
        }
    }
}
