using Iot.Device.BrickPi3.Sensors;
using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnitsNet;

namespace Library.Model
{
    public class RGBSneak
    {

        public int HeadPixel;
        public int TailPixel;
        public int SneakLength;
        public int StripStartPixel;
        public int StripEndPixel;
        public RGBColor onColor;
        public RGBColor offColor;
        public bool ActiveSneak = true;
        public RGBSneak(RGBColor onColor, RGBColor offColor, int startStripPixel, int endStripPixel, int length)
        {
            this.StripStartPixel = startStripPixel;
            this.StripEndPixel = endStripPixel;
            this.HeadPixel = startStripPixel;
            this.SneakLength = length;
            this.TailPixel = startStripPixel - length;
            this.onColor = onColor;
            this.offColor = offColor;
        }

        public void MoveSneakForward()
        {
            // Move Tail To The Next Pixel 
            MoveHeadForward();
            // Move Head To The Next Pixel 
            MoveTailForward();
        }

        private void MoveHeadForward()
        {
            bool HeadNotReachTheEndOfTheLine = this.HeadPixel <= this.StripEndPixel;
            if (HeadNotReachTheEndOfTheLine)
                this.HeadPixel++;
            else
                this.HeadPixel = this.StripStartPixel;
            ChangePixelColor(this.HeadPixel, this.onColor);
            //Console.WriteLine($"Head Forward {this.HeadPixel} , Color {this.onColor} ,HeadNotReachTheEndOfTheLine {HeadNotReachTheEndOfTheLine}");
        }
        private void MoveTailForward()
        {
            bool TailNotReachTheEndOfTheLine = this.TailPixel <= this.StripEndPixel;
            if (TailNotReachTheEndOfTheLine)
                this.TailPixel++;
            else
                this.TailPixel = this.StripStartPixel;
            ChangePixelColor(this.TailPixel, this.offColor);
            //Console.WriteLine($"Move Tail Forward {this.TailPixel} , Color {this.offColor} ,TailNotReachTheEndOfTheLine {TailNotReachTheEndOfTheLine}");

        }
        private void ChangePixelColor(int pixelNumber, RGBColor rGBColor)
        {
            bool pixelInTheStrip = pixelNumber >= this.StripStartPixel && pixelNumber <= this.StripEndPixel;
            if (pixelInTheStrip)
                RGBWS2811.SetColor(this.ActiveSneak, pixelNumber, rGBColor);
        }
        public void ActivateSneak(bool isActive)
        {
            this.ActiveSneak = isActive;
        }
        public void UpdateSneakLength(int newLength)
        {
            if (newLength > this.SneakLength)
                this.TailPixel--;
            else if(newLength < this.SneakLength)
                this.MoveTailForward();
            this.SneakLength = newLength;
        }
        public void Reset()
        {
            this.HeadPixel = this.StripStartPixel;
            this.TailPixel = this.StripStartPixel - this.SneakLength;
        }
    }
}
