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
        RGBSneak Sneak;
        public List<RGBButtonSneakPixel> Buttons = new List<RGBButtonSneakPixel>();
        private int playerAssignedToTheStrip;
        public bool IsActive = true;

        public SneakStrip(
          RGBColor rgbColor,
          RGBColor rgbOffColor,
          int startRGBLed,
          int endRGBLed,
          List<RGBButtonSneakPixel> Buttons,
          int stripIndex,
          int playerAssignedToTheStrip,
          int warmLength
          )
        {
            this.rgbOffColor = rgbOffColor;
            this.rgbColor = rgbColor;
            this.startRGBLed = startRGBLed;
            this.endRGBLed = endRGBLed;
            Sneak = new RGBSneak(rgbColor, rgbOffColor, startRGBLed, endRGBLed, warmLength);

            this.Buttons = Buttons;
            this.stripIndex = stripIndex;
            this.playerAssignedToTheStrip = playerAssignedToTheStrip;
        }
        public void UpdateLength(int wormLength)
        {
            //return;
            Sneak.UpdateSneakLength(wormLength);
        }

        public void Activate(bool isActive)
        {
            this.isActive = isActive;
        }
        public void Move()
        {
            if (!this.isActive)
                return;
            Sneak.MoveSneakForward();
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
                    bool buttonState = ButtonTouchTheWorm(button.Pixel, Sneak.TailPixel, Sneak.HeadPixel, buttonIndex == 0);

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
            //bool inRange = buttonPixel >= wormTailPixel && buttonPixel <= wormHeadPixel;
            bool inRange = buttonPixel >= wormHeadPixel && buttonPixel < wormHeadPixel + 5;
            bool wormPassTheLastPixelInStrip = (wormTailPixel > wormHeadPixel);
            bool inLastBitsOfTheLine = wormPassTheLastPixelInStrip && (buttonPixel > wormHeadPixel) && itsStartButton;
            return inRange || inLastBitsOfTheLine;
        }

        public void LineReset()
        {
            resetLine = false;
            Sneak.Reset();
        }
    }
}