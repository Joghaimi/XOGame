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
        public RGBColor rgbOffColor { get; set; }

        public int startRGBLed { get; set; }
        public int endRGBLed { get; set; }
        public int currentLed { get; set; }
        public bool isActive { get; set; } = true;
        public bool resetLine { get; set; } = false;
        public RGBButton rGBButton0 { get; set; }
        public RGBButtonPixel rGBButton1 { get; set; }
        public RGBButtonPixel rGBButton2 { get; set; }
        public RGBButtonPixel rGBButton3 { get; set; }
        public RGBButtonPixel rGBButton4 { get; set; }

        public RGBWorm worm1 { get; set; }
        public RGBWorm worm2 { get; set; }
        public RGBWorm worm3 { get; set; }
        public RGBWorm worm4 { get; set; }

        public int wormLength { get; set; } = 5;


        public List<RGBWorm> Worms = new List<RGBWorm>();



        //public Strip(
        //    RGBColor rgbColor,
        //    RGBColor rgbOffColor,
        //    int startRGBLed,
        //    int endRGBLed,
        //    RGBButtonPixel rGBButton0,
        //    RGBButtonPixel rGBButton1,
        //    RGBButtonPixel rGBButton2,
        //    RGBButtonPixel rGBButton3,
        //    RGBButtonPixel rGBButton4,
        //    int wormLength
        //    )
        //{
        //    this.rgbColor = rgbColor;
        //    this.startRGBLed = startRGBLed;
        //    this.endRGBLed = endRGBLed;
        //    this.currentLed = startRGBLed;
        //    this.rGBButton1 = rGBButton1;
        //    this.rGBButton2 = rGBButton2;
        //    this.rGBButton3 = rGBButton3;
        //    this.rGBButton4 = rGBButton4;
        //    this.rGBButton0 = rGBButton0;
        //    this.wormLength = wormLength;





        //    this.worm1 = new RGBWorm(startRGBLed, -1 * wormLength);
        //    this.rgbOffColor = rgbOffColor;
        //    //this.worm2 = worm2;
        //    //this.worm3 = worm3;
        //    //this.worm4 = worm4;
        //}

        public Strip(
          RGBColor rgbColor,
          RGBColor rgbOffColor,
          int startRGBLed,
          int endRGBLed,
          RGBButton rGBButton0,
          RGBButtonPixel rGBButton1,
          RGBButtonPixel rGBButton2,
          RGBButtonPixel rGBButton3,
          RGBButtonPixel rGBButton4
          )
        {
            this.rgbOffColor = rgbOffColor;
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            this.currentLed = startRGBLed;
            this.rGBButton1 = rGBButton1;
            this.rGBButton2 = rGBButton2;
            this.rGBButton3 = rGBButton3;
            this.rGBButton4 = rGBButton4;
            this.rGBButton0 = rGBButton0;
            this.wormLength = wormLength;

            Console.WriteLine("Init Worms");
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 0));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 3, 1));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 4, 2));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 3));
            Console.WriteLine("End Init Worms");




            //this.worm1 = new RGBWorm(startRGBLed, -1 * wormLength);
            this.rgbOffColor = rgbOffColor;
            //this.worm2 = worm2;
            //this.worm3 = worm3;
            //this.worm4 = worm4;
        }

        public void Move()
        {

            // Turn RGB Light On 
            //RGBWS2811.SetColor(startPixel, RGBColor.Red);
            //startPixel++;
            //if (endPixel >= 0)
            //    RGBWS2811.SetColor(endPixel, RGBColor.Off);
            //endPixel++;
            foreach (var worm in Worms)
            {
                if (worm.startPixel < endRGBLed)
                    worm.startPixel++;
                else
                    worm.startPixel = worm.initendPixel;
                if (worm.endPixel < endRGBLed)
                    worm.endPixel++;
                else
                    worm.endPixel = worm.initendPixel;





                bool canMoveForward = worm.startPixel >= this.startRGBLed;
                bool theTailIsShown = worm.endPixel >= startRGBLed;

                if (canMoveForward)
                    RGBWS2811.SetColor(this.isActive, worm.startPixel, this.rgbColor);
                if (theTailIsShown)
                    RGBWS2811.SetColor(this.isActive, worm.endPixel, this.rgbOffColor);
            }



            //if (worm1.startPixel < endRGBLed)
            //    worm1.startPixel++;
            //else
            //    worm1.startPixel = startRGBLed;
            // Move the Start one and the end one
            //if (worm1.startPixel >= 0)
            //    RGBWS2811.SetColor(this.isActive, worm1.endPixel, this.rgbOffColor);
            //if (worm1.endPixel < endRGBLed)
            //    worm1.endPixel++;
            //else
            //    worm1.endPixel = startRGBLed;
            // Check the RGB Button
        }



        public void NextLed()
        {
            if (!isActive)
                return;
            RGBWS2811.SetColor(isActive, currentLed, rgbColor);

            if (currentLed == startRGBLed)
                this.rGBButton0.TurnColorOn(this.rgbColor);
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
            this.rGBButton0.TurnColorOn(RGBColor.Off);
        }


    }
}
