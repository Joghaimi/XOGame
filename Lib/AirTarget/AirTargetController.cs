using Iot.Device.Mcp23xxx;
using Iot.Device.Mcp3428;
using Iot.Device.Nmea0183.Ais;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.AirTarget
{
    public class AirTargetController
    {
        // input infra Red 
        private MCP23017 _InputIc;
        private int _InputPin;
        private Port _InputPort;
        private MCP23017 _OutputIc;
        private Port _OutputPort;
        private int _OutputPin;
        public static MCP23Controller _MCP23Controller = new MCP23Controller();
        public static bool IsMCP23ControllerInit = false;
        public static bool IsSelected = false;

        public AirTargetController(MCP23017 inputIc, int inputPin, Port inputPort, MCP23017 outputIc, Port outputPort, int outputPin)
        {
            _InputIc = inputIc;
            _InputPin = inputPin;
            _InputPort = inputPort;
            _OutputIc = outputIc;
            _OutputPort = outputPort;
            _OutputPin = outputPin;
            if (!IsMCP23ControllerInit)
            {
                _MCP23Controller = new MCP23Controller();
                IsMCP23ControllerInit = true;
            }

            // Set Pinout
            _MCP23Controller.PinModeSetup(_InputIc, _InputPort, _InputPin, PinMode.Input);
            _MCP23Controller.PinModeSetup(_OutputIc, _OutputPort, _OutputPin, PinMode.Output);
        }
        public void Select(bool Selected)
        {
            IsSelected = Selected;
            if (IsSelected)
            {
                _MCP23Controller.Write(_OutputIc, _OutputPort, _OutputPin, PinState.High);
            }
            else
            {
                _MCP23Controller.Write(_OutputIc, _OutputPort, _OutputPin, PinState.Low);
            }
        }
        public bool Status()
        {
            return _MCP23Controller.Read(_InputIc, _InputPort, _InputPin); 
        }

    }
}
