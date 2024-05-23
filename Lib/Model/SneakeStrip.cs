using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Model
{
    public class SneakeStrip
    {
        public RGBColor rgbColor { get; set; }
        public RGBColor rgbOffColor { get; set; }

        public int startRGBLed { get; set; }
        public int endRGBLed { get; set; }
        public int currentLed { get; set; }
        public bool isActive { get; set; } = true;
        public bool resetLine { get; set; } = false;

        public int stripIndex = -1;
        public int buttonOneWormIndex = -1;

        public int wormLength { get; set; } = 5;
        RGBWorm Worm;
        //public List<RGBWorm> Worms = new List<RGBWorm>();
        public List<RGBButtonPixel> Buttons = new List<RGBButtonPixel>();


        public SneakeStrip(
          RGBColor rgbColor,
          RGBColor rgbOffColor,
          int startRGBLed,
          int endRGBLed,
          List<RGBButtonPixel> Buttons,
          int stripIndex,
          int warmLength
          )
        {
            this.rgbOffColor = rgbOffColor;
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            this.currentLed = startRGBLed;
            Console.WriteLine("Init Worms");
            Worm = new RGBWorm(startRGBLed, endRGBLed, warmLength, 0);
            Console.WriteLine("End Init Worms");
            this.Buttons = Buttons;
            this.stripIndex = stripIndex;
            this.rgbOffColor = rgbOffColor;
        }
        public void UpdateLength(int wormLength)
        {
            Worm.updateLength(wormLength);
        }


        public void Move()
        {

            if (Worm.startPixel < endRGBLed)
                Worm.startPixel++;
            else
                Worm.startPixel = Worm.initendPixel;
            if (Worm.endPixel < endRGBLed)
                Worm.endPixel++;
            else
                Worm.endPixel = Worm.initendPixel;
            bool canMoveForward = Worm.startPixel >= this.startRGBLed;
            bool theTailIsShown = Worm.endPixel >= startRGBLed;

            if (canMoveForward)
                RGBWS2811.SetColor(this.isActive, Worm.startPixel, this.rgbColor);
            if (theTailIsShown)
                RGBWS2811.SetColor(this.isActive, Worm.endPixel, this.rgbOffColor);


            RGBButtonStateAndColor();// RGB Button Control 
        }
        public void MoveTwoPixel()
        {
            Move();
            Move();
        }
        public void MoveThreePixel()
        {
            Move();
            Move();
            Move();
        }
        public void RGBButtonStateAndColor()
        {
            int buttonIndex = 0;

            foreach (var button in Buttons)
            {
                if (button.Button.stripIndex == -1 || button.Button.stripIndex == stripIndex)
                {
                    bool buttonState = false;
                    buttonState = buttonState || ButtonTouchTheWorm(button.Pixel, Worm.endPixel, Worm.startPixel, buttonIndex == 0);

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



        public bool ButtonTouchTheWorm(int buttonPixel, int wormTailPixel, int wormHeadPixel, bool itsStartButton)
        {
            bool inRange = buttonPixel >= wormTailPixel && buttonPixel <= wormHeadPixel;
            bool wormPassTheLastPixelInStrip = (wormTailPixel > wormHeadPixel);
            bool inLastBitsOfTheLine = wormPassTheLastPixelInStrip && (buttonPixel > wormHeadPixel) && itsStartButton;
            return inRange || inLastBitsOfTheLine;
        }

        public void LineReseted()
        {
            resetLine = false;
            currentLed = startRGBLed;
            Worm.reset();
        }
    }
}









//public void NextLed()
//{
//    if (!isActive)
//        return;
//    RGBWS2811.SetColor(isActive, currentLed, rgbColor);

//    if (currentLed == startRGBLed)
//        this.rGBButton0.Button.TurnColorOn(this.rgbColor);
//    if (currentLed < endRGBLed)
//        currentLed++;
//    else
//    {
//        resetLine = true;
//    }

//    if (currentLed - 1 == rGBButton1.Pixel)
//    {
//        Console.WriteLine($"Turn RGBOne {rgbColor}");
//        rGBButton1.Button.Set(true);
//        rGBButton1.Button.TurnColorOn(rgbColor);
//    }
//    else if (currentLed - 2 == rGBButton1.Pixel || currentLed == rGBButton1.Pixel || currentLed + 1 == rGBButton1.Pixel)
//    {
//        rGBButton1.Button.TurnColorOn(RGBColor.Off);

//    }
//    else
//    {
//        rGBButton1.Button.Set(false);
//        rGBButton1.Button.TurnColorOn(RGBColor.Off);
//    }
//    if (currentLed - 1 == rGBButton2.Pixel)
//    {
//        Console.WriteLine($"Turn RGB2 {rgbColor}");
//        rGBButton2.Button.Set(true);
//        rGBButton2.Button.TurnColorOn(rgbColor);
//    }
//    else if (currentLed - 2 == rGBButton2.Pixel || currentLed == rGBButton2.Pixel || currentLed + 1 == rGBButton2.Pixel)

//    {
//        rGBButton2.Button.TurnColorOn(RGBColor.Off);
//    }
//    else
//    {

//        rGBButton2.Button.Set(false);
//        rGBButton2.Button.TurnColorOn(RGBColor.Off);
//    }

//    if (currentLed - 1 == rGBButton3.Pixel)
//    {
//        Console.WriteLine($"Turn RGB3 {rgbColor}");
//        rGBButton3.Button.Set(true);
//        rGBButton3.Button.TurnColorOn(rgbColor);
//    }

//    else if (currentLed - 2 == rGBButton3.Pixel || currentLed == rGBButton3.Pixel || currentLed + 1 == rGBButton3.Pixel)
//    {
//        rGBButton3.Button.TurnColorOn(RGBColor.Off);
//    }
//    else
//    {

//        rGBButton3.Button.Set(false);
//        rGBButton3.Button.TurnColorOn(RGBColor.Off);
//    }

//    if (currentLed - 1 == rGBButton4.Pixel)
//    {
//        Console.WriteLine($"Turn RGB4 {rgbColor}");
//        rGBButton4.Button.Set(true);
//        rGBButton4.Button.TurnColorOn(rgbColor);
//    }
//    else if (currentLed - 2 == rGBButton4.Pixel || currentLed == rGBButton4.Pixel || currentLed + 1 == rGBButton4.Pixel)
//    {
//        rGBButton4.Button.TurnColorOn(RGBColor.Off);
//    }
//    else
//    {
//        rGBButton4.Button.Set(false);
//        rGBButton4.Button.TurnColorOn(RGBColor.Off);
//    }



//}