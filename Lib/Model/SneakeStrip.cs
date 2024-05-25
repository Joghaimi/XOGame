using Library.RGBLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Library.Model
{
    public class SneakStrip
    {
        public RGBColor rgbColor { get; set; }
        public RGBColor rgbOffColor { get; set; }

        public int startRGBLed { get; set; }
        public int endRGBLed { get; set; }
        public bool isActive { get; set; } = true;
        public bool resetLine { get; set; } = false;

        public int stripIndex = -1;

        public int wormLength { get; set; } = 5;
        public List<RGBSneak> Sneaks = new List<RGBSneak>();
        public List<RGBButtonSneakPixel> Buttons = new List<RGBButtonSneakPixel>();
        private int playerAssignedToTheStrip;
        public bool IsActive = true;
        public StripType StripType { get; set; }

        public SneakStrip(
          RGBColor rgbColor,
          RGBColor rgbOffColor,
          int startRGBLed,
          int endRGBLed,
          List<RGBButtonSneakPixel> Buttons,
          int stripIndex,
          int playerAssignedToTheStrip,
          int warmLength,
          StripType type = StripType.player
          )
        {
            this.StripType = type;
            this.rgbOffColor = rgbOffColor;
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            Sneaks.Add(new RGBSneak(rgbColor, rgbOffColor, startRGBLed, endRGBLed, warmLength, 0));
            if (type == StripType.Extra)
            {
                Sneaks.Add(new RGBSneak(rgbColor, rgbOffColor, startRGBLed, endRGBLed, warmLength, 1));
                Sneaks.Add(new RGBSneak(rgbColor, rgbOffColor, startRGBLed, endRGBLed, warmLength, 2));
                Sneaks.Add(new RGBSneak(rgbColor, rgbOffColor, startRGBLed, endRGBLed, warmLength, 3));
            }

            this.Buttons = Buttons;
            this.stripIndex = stripIndex;
            this.playerAssignedToTheStrip = playerAssignedToTheStrip;
        }
        // Only Update Length For Player Sneak
        public void UpdateLength(int wormLength)
        {
            Sneaks[0].UpdateSneakLength(wormLength);
        }

        public void Activate(bool isActive)
        {
            this.isActive = isActive;
        }
        public void Move()
        {
            if (!this.isActive)
                return;
            foreach (var sneak in Sneaks)
            {
                sneak.MoveSneakForward();
            }
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
                    foreach (var sneak in Sneaks)
                    {
                        buttonState = buttonState || ButtonTouchTheWorm(button.Pixel, sneak.TailPixel, sneak.HeadPixel, buttonIndex == 0);
                    }
                    //bool buttonState = ButtonTouchTheWorm(button.Pixel, Sneak.TailPixel, Sneak.HeadPixel, buttonIndex == 0);

                    if (buttonState && !button.Button.isSet() && !button.Button.clickedForOnce)
                    {
                        button.Button.Set(true, playerAssignedToTheStrip);
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
            bool ButtonTouchTheHead = buttonPixel == wormHeadPixel || buttonPixel == wormHeadPixel - 1 || buttonPixel == wormHeadPixel - 2 || buttonPixel == wormHeadPixel - 3 || buttonPixel == wormHeadPixel - 4;


            bool inRange = ButtonTouchTheHead;//buttonPixel >= wormTailPixel && buttonPixel <= wormHeadPixel;
            bool wormPassTheLastPixelInStrip = (wormTailPixel > wormHeadPixel);
            bool inLastBitsOfTheLine = wormPassTheLastPixelInStrip && (buttonPixel > wormHeadPixel) && itsStartButton;
            return inRange || inLastBitsOfTheLine;
        }

        public void LineReset()
        {
            resetLine = false;
            foreach (var sneak in Sneaks)
                sneak.Reset();
        }
    }
}