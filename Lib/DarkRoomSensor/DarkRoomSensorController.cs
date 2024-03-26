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
        public DarkRoomSensor sensor;
        private bool falseFornow =false;
        public DarkRoomSensorController(DarkRoomSensor sensor)
        {
            this.sensor = sensor;
            MCP23Controller.PinModeSetup(this.sensor.Pin, PinMode.Input);
        }
        public bool stauts()
        {
            if(falseFornow)
                return false;
            if (sensor.isInverted)
                return (!MCP23Controller.Read(sensor.Pin));
            else
                return (MCP23Controller.Read(sensor.Pin));

        }

        public (bool status, int score) SensorStatus()
        {
            if (!sensor.isShoot)
            {
                sensor.isShoot = true;
                if (sensor.isInverted)
                    return (!MCP23Controller.Read(sensor.Pin), sensor.Score);
                else
                    return (MCP23Controller.Read(sensor.Pin), sensor.Score);

            }
            return (false, 0);
        }
        public void BlockScoreFor1Sec()
        {

            Task.Run(async () =>
            {
                falseFornow = true;
                await Task.Delay(1000);
                falseFornow = false;
            });
        }
    }
}
