﻿using Library.RGBLib;
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
        public List<RGBButtonSneakPixel> Buttons = new List<RGBButtonSneakPixel>();
        private int playerAssignedToTheStrip;
        public bool IsActive = true;

        public SneakeStrip(
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
            this.currentLed = startRGBLed;
            Console.WriteLine("Init Worms");
            Worm = new RGBWorm(startRGBLed, endRGBLed, warmLength, 0);
            Console.WriteLine("End Init Worms");
            this.Buttons = Buttons;
            this.stripIndex = stripIndex;
            this.rgbOffColor = rgbOffColor;
            this.playerAssignedToTheStrip = playerAssignedToTheStrip;
        }
        public void UpdateLength(int wormLength)
        {
            Worm.updateLength(wormLength);
        }

        public void Activate(bool isActive)
        {
            this.isActive = isActive;
        }
        public void Move()
        {
            if (!this.isActive)
                return;
            bool headerNotReachTheEndOfTheStrip = Worm.startPixel < endRGBLed;
            bool tailNotReachTheEndOfTheStrip = Worm.endPixel < endRGBLed;

            if (headerNotReachTheEndOfTheStrip)
                Worm.startPixel++;
            else
                Worm.startPixel = Worm.initendPixel;

            if (tailNotReachTheEndOfTheStrip)
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
                    bool buttonState = ButtonTouchTheWorm(button.Pixel, Worm.endPixel, Worm.startPixel, buttonIndex == 0);

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