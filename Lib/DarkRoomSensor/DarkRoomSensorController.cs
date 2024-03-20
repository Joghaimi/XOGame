using Iot.Device.BrickPi3.Sensors;
using Library.GPIOLib;
using System;
using System.Collections.Generic;
using System.Device.Gpio;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Library.DarkRoomSensor
{
    public class DarkRoomSensorController
    {
        private DarkRoomSensor _darkRoomSensor;
        public DarkRoomSensorController(DarkRoomSensor darkRoomSensor)
        {
            _darkRoomSensor = darkRoomSensor;
            MCP23Controller.PinModeSetup(darkRoomSensor.Pin, PinMode.Input);
        }

        public (bool status, int score) SensorStatus()
        {
            if (!_darkRoomSensor.isShoot)
            {
                _darkRoomSensor.isShoot=true;
                return (!MCP23Controller.Read(_darkRoomSensor.Pin), _darkRoomSensor.Score);
            }
            return (false, 0);
        }
        public void BlockScoreFor1Sec()
        {

            Task.Run(async () =>
            {
                await Task.Delay(1000);
                _darkRoomSensor.isShoot = false;
            });
        }
    }
}
