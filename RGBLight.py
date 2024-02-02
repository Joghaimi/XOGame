import RPi.GPIO as GPIO
import time
import sys

class RGBDriver:
    def __init__(self, clk, data):
        self.clk_pin = clk
        self.data_pin = data
        GPIO.setmode(GPIO.BCM)
        GPIO.setup(self.clk_pin, GPIO.OUT)
        GPIO.setup(self.data_pin, GPIO.OUT)

    def begin(self):
        self.send_32_zero()

    def end(self):
        self.send_32_zero()

    def clk_rise(self):
        GPIO.output(self.clk_pin, GPIO.LOW)
        time.sleep(0.00002)  # Sleep for 20 microseconds
        GPIO.output(self.clk_pin, GPIO.HIGH)
        time.sleep(0.00002)

    def send_32_zero(self):
        for _ in range(32):
            GPIO.output(self.data_pin, GPIO.LOW)
            self.clk_rise()

    def take_anti_code(self, dat):
        tmp = 0

        if (dat & 0x80) == 0:
            tmp |= 0x02

        if (dat & 0x40) == 0:
            tmp |= 0x01

        return tmp

    def dat_send(self, dx):
        for _ in range(32):
            GPIO.output(self.data_pin, GPIO.HIGH if (dx & 0x80000000) != 0 else GPIO.LOW)
            dx <<= 1
            self.clk_rise()

    def set_color(self, red, green, blue):
        dx = 0

        dx |= 0x03 << 30
        dx |= self.take_anti_code(blue) << 28
        dx |= self.take_anti_code(green) << 26
        dx |= self.take_anti_code(red) << 24

        dx |= blue << 16
        dx |= green << 8
        dx |= red

        self.dat_send(dx)

    def cleanup(self):
        GPIO.cleanup()

if __name__ == "__main__":
    if len(sys.argv) != 6:
        print("Usage: python script.py clk_pin data_pin red green blue")
        sys.exit(1)

    clk_pin = int(sys.argv[1])
    data_pin = int(sys.argv[2])
    red = int(sys.argv[3])
    green = int(sys.argv[4])
    blue = int(sys.argv[5])

    rgb_driver = RGBDriver(clk=clk_pin, data=data_pin)
    rgb_driver.begin()
    rgb_driver.set_color(red=red, green=green, blue=blue)
    rgb_driver.end()
    rgb_driver.cleanup()
