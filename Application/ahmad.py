import time
from rpi_ws281x import PixelStrip, Color, ws
# LED strip configuration:
LED_COUNT = 300         # Number of LED pixels.
LED_PIN = 10           # GPIO pin connected to the pixels (must support PWM!).
LED_FREQ_HZ = 800000   # LED signal frequency in hertz (usually 800khz)
LED_DMA = 10           # DMA channel to use for generating signal (try 10)
LED_BRIGHTNESS = 255   # Set to 0 for darkest and 255 for brightest
LED_INVERT = False     # True to invert the signal (when using NPN transistor level shift)
LED_CHANNEL = 0
LED_STRIP = ws.SK6812_STRIP_RGBW
wait_ms=10
# Create NeoPixel object with appropriate configuration.
strip = PixelStrip(LED_COUNT, LED_PIN, LED_FREQ_HZ, LED_DMA, LED_INVERT, LED_BRIGHTNESS, LED_CHANNEL, LED_STRIP)
# Intialize the library (must be called once before other functions).
def init():
    print("Init Strip")
	strip.begin()
def colorWipe(strip, color):
    """Wipe color across display a pixel at a time."""
    for i in range(strip.numPixels()):
        strip.setPixelColor(i, color)
        strip.show()
        #time.sleep(wait_ms / 1000.0)
    
def test():
    try:
        print("Test ..")
        while True:
            # Traveling red
            colorWipe(strip, Color(255, 0, 0, 0)) # Red 

            colorWipe(strip, Color(0, 255, 0, 0)) # Green 

            colorWipe(strip, Color(0, 0, 255, 0)) # Blue

            colorWipe(strip, Color(0, 0, 0, 255)) # White

            colorWipe(strip, Color(255, 255, 255, 255)) # Full white
    except KeyboardInterrupt:
        # Turn off LEDs on exit
        colorWipe(strip, Color(0, 0, 0, 0))
        print("\nLED strip stopped.")


def print_ahmad():
	print("ahamd Joghaimi")