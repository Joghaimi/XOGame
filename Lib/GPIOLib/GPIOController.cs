using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.GPIOLib
{
    using System.Device.Gpio;
    public class GPIOController
    {
        GpioController _controller;
        public GPIOController(GpioController controller)
        {
            _controller = controller;
        }
        public void Setup(int pin , PinMode mode ) { 
            _controller.OpenPin(pin, mode);
        }
        public void Write(int pin , bool value) {
            if(value)
                _controller.Write(pin, PinValue.High);
            else
                _controller.Write(pin, PinValue.High);

        }
        public bool Read(int pin)
        {
          return _controller.Read(pin)== PinValue.High ?true:false;
        }
    }
}
