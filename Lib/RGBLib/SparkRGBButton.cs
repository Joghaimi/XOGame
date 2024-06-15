using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.RGBLib
{
    public class SparkRGBButton
    {
        public RGBButton _button;
        public List<Spike> SpikeStrips; // To Do 
        public int PixelInAllSpikes;
        public RGBColor color;
        bool hasSpikes = false;
        public SparkRGBButton(RGBButton button, List<Spike> spikeStrips, int pixelInAllSpikes, RGBColor color)
        {
            //, List<Spike> spikeStrips
            _button = button;
            SpikeStrips = spikeStrips;
            PixelInAllSpikes = pixelInAllSpikes;
            this.color = color;
            hasSpikes = true;
        }
        public SparkRGBButton(RGBButton button, int pixelInAllSpikes, RGBColor color)
        {
            //, List<Spike> spikeStrips
            _button = button;
            PixelInAllSpikes = pixelInAllSpikes;
            this.color = color;
        }

        public void Activate(bool activate)
        {
            _button.Set(activate);
            if (activate)
                _button.TurnColorOn(color);
            else
                _button.TurnColorOn(RGBColor.Off);
        }
        public int isPressed()
        {
            if (_button.CurrentStatus())
                return 0;
            if (!_button.isSet())
                return 0;
            if (hasSpikes)
                SuccessEffect();
            Activate(false);
            return 1;
        }


        public void SuccessEffect()
        {
            for (int i = 0; i < PixelInAllSpikes; i++)
            {
                foreach (var spike in SpikeStrips)
                {
                    spike.MoveForward();
                }
                Thread.Sleep(100);
            }
        }
    }
}
