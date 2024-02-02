import RPi.GPIO as GPIO
import time

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


# Example usage
if __name__ == "__main__":
    arg1 = globals().get("arg1", None)
    arg2 = globals().get("arg2", None)

    rgb_driver = RGBDriver(clk=20, data=21)
    rgb_driver.begin()
    rgb_driver.set_color(red=100, green=0, blue=0)  # Set color to red
    #time.sleep(2)
    rgb_driver.end()
    #rgb_driver.cleanup()
    print(arg1)
    print(arg2)







