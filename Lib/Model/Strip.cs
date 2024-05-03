using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.Model
{
    public class Strip
    {
        public RGBColor rgbColor { get; set; }
        public int startRGBLed { get; set; }
        public int endRGBLed { get; set; }
        public int currentLed { get; set; }
        public bool isActive { get; set; } = false;
        public bool resetLine { get; set; } = false;
        public RGBButtonPixel rGBButton1 { get; set; }
        public RGBButtonPixel rGBButton2 { get; set; }
        public RGBButtonPixel rGBButton3 { get; set; }
        public RGBButtonPixel rGBButton4 { get; set; }
        public RGBButton startLed { get; set; }




        public Strip(RGBColor rgbColor, int startRGBLed, int endRGBLed, RGBButton startLed, RGBButtonPixel rGBButton1, RGBButtonPixel rGBButton2, RGBButtonPixel rGBButton3, RGBButtonPixel rGBButton4)
        {
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            this.currentLed = startRGBLed;
            this.rGBButton1 = rGBButton1;
            this.rGBButton2 = rGBButton2;
            this.rGBButton3 = rGBButton3;
            this.rGBButton4 = rGBButton4;
            this.startLed = startLed;
        }
        public void NextLed()
        {
            if (!isActive)
                return;
            if (currentLed == startRGBLed)
                this.startLed.TurnColorOn(this.rgbColor);
            if (currentLed < endRGBLed)
                currentLed++;
            else
            {
                resetLine = true;
            }

            if (currentLed - 1 == rGBButton1.Pixel)
            {
                Console.WriteLine($"Turn RGBOne {rgbColor}");
                rGBButton1.Button.Set(true);
                rGBButton1.Button.TurnColorOn(rgbColor);
            }
            else if (currentLed - 2 == rGBButton1.Pixel || currentLed == rGBButton1.Pixel || currentLed + 1 == rGBButton1.Pixel)
            {
                rGBButton1.Button.TurnColorOn(RGBColor.Off);

            }
            else
            {
                rGBButton1.Button.Set(false);
                rGBButton1.Button.TurnColorOn(RGBColor.Off);
            }
            if (currentLed - 1 == rGBButton2.Pixel)
            {
                Console.WriteLine($"Turn RGB2 {rgbColor}");
                rGBButton2.Button.Set(true);
                rGBButton2.Button.TurnColorOn(rgbColor);
            }
            else if (currentLed - 2 == rGBButton2.Pixel || currentLed == rGBButton2.Pixel || currentLed + 1 == rGBButton2.Pixel)

            {
                rGBButton2.Button.TurnColorOn(RGBColor.Off);
            }
            else
            {

                rGBButton2.Button.Set(false);
                rGBButton2.Button.TurnColorOn(RGBColor.Off);
            }

            if (currentLed - 1 == rGBButton3.Pixel)
            {
                Console.WriteLine($"Turn RGB3 {rgbColor}");
                rGBButton3.Button.Set(true);
                rGBButton3.Button.TurnColorOn(rgbColor);
            }

            else if (currentLed - 2 == rGBButton3.Pixel || currentLed == rGBButton3.Pixel || currentLed + 1 == rGBButton3.Pixel)
            {
                rGBButton3.Button.TurnColorOn(RGBColor.Off);
            }
            else
            {

                rGBButton3.Button.Set(false);
                rGBButton3.Button.TurnColorOn(RGBColor.Off);
            }

            if (currentLed - 1 == rGBButton4.Pixel)
            {
                Console.WriteLine($"Turn RGB4 {rgbColor}");
                rGBButton4.Button.Set(true);
                rGBButton4.Button.TurnColorOn(rgbColor);
            }
            else if (currentLed - 2 == rGBButton4.Pixel || currentLed == rGBButton4.Pixel || currentLed + 1 == rGBButton4.Pixel)
            {
                rGBButton4.Button.TurnColorOn(RGBColor.Off);
            }
            else
            {
                rGBButton4.Button.Set(false);
                rGBButton4.Button.TurnColorOn(RGBColor.Off);
            }



        }
        public void LineReseted()
        {
            resetLine = false;
            currentLed = startRGBLed;
            this.startLed.TurnColorOn(RGBColor.Off);
        }


    }
}
