using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

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


        public RGBButtonPixel rGBButton0 { get; set; }
        public RGBButtonPixel rGBButton1 { get; set; }
        public RGBButtonPixel rGBButton2 { get; set; }
        public RGBButtonPixel rGBButton3 { get; set; }
        public RGBButtonPixel rGBButton4 { get; set; }

        public int stripIndex = -1;


        public int buttonOneWormIndex = -1;

        public int wormLength { get; set; } = 5;
        public List<RGBWorm> Worms = new List<RGBWorm>();
        public List<RGBButtonPixel> Buttons = new List<RGBButtonPixel>();



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
          List<RGBButtonPixel> Buttons,
          int stripIndex
          //RGBButtonPixel rGBButton0,
          //RGBButtonPixel rGBButton1,
          //RGBButtonPixel rGBButton2,
          //RGBButtonPixel rGBButton3,
          //RGBButtonPixel rGBButton4
          )
        {
            this.rgbOffColor = rgbOffColor;
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            this.currentLed = startRGBLed;
            Console.WriteLine("Init Worms");
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 0));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 1));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 2));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 3));
            Console.WriteLine("End Init Worms");
            this.Buttons = Buttons;
            this.stripIndex = stripIndex;
            this.rgbOffColor = rgbOffColor;
        }




        public Strip(
          RGBColor rgbColor,
          RGBColor rgbOffColor,
          int startRGBLed,
          int endRGBLed,
          RGBButtonPixel rGBButton0,
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
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 1));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 2));
            this.Worms.Add(new RGBWorm(startRGBLed, endRGBLed, 5, 3));
            Console.WriteLine("End Init Worms");

            Buttons.Add(rGBButton0);
            Buttons.Add(rGBButton1);
            Buttons.Add(rGBButton2);
            Buttons.Add(rGBButton3);
            Buttons.Add(rGBButton4);


            this.rgbOffColor = rgbOffColor;

        }

        public void UpdateLength(int wormLength)
        {
            foreach (var worm in Worms)
                worm.updateLength(wormLength);
        }


        public void Move()
        {
            //int Index = 0;
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

            // RGB Button Control 
            int buttonIndex = 0;
            foreach (var button in Buttons)
            {
                if (button.Button.stripIndex == -1 || button.Button.stripIndex == stripIndex)
                {
                    bool buttonState = false;
                    foreach (var worm in Worms)
                    {
                        buttonState = buttonState || InRange(button.Pixel, worm.endPixel, worm.startPixel);
                    }

                    if (buttonState && !button.Button.isSet() && !button.Button.clickedForOnce)
                    {
                        button.Button.Set(true);
                        button.Button.TurnColorOn(rgbColor);
                        button.Button.stripIndex = stripIndex;
                    }
                    else if (!buttonState && (button.Button.isSet() || button.Button.clickedForOnce))
                    {
                        button.Button.TurnColorOn(RGBColor.Off);
                        button.Button.Set(false);
                        button.Button.stripIndex = -1;
                        button.Button.clickedForOnce = false;

                    }

                }
                buttonIndex++;





            }



        }




        public bool InRange(int number, int min, int max)
        {
            bool inRange = number >= min && number <= max;

            return inRange;
        }
        public void NextLed()
        {
            if (!isActive)
                return;
            RGBWS2811.SetColor(isActive, currentLed, rgbColor);

            if (currentLed == startRGBLed)
                this.rGBButton0.Button.TurnColorOn(this.rgbColor);
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
            this.rGBButton0.Button.TurnColorOn(RGBColor.Off);
        }


    }
}
