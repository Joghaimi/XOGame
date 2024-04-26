from rpi_ws281x import PixelStrip, Color, ws
strip = None


def init_strip():
    global strip  # Use the global keyword to access the global variable
    LED_COUNT = 300         # Number of LED pixels.
    LED_PIN = 10           # GPIO pin connected to the pixels (must support PWM!).
    LED_FREQ_HZ = 800000   # LED signal frequency in hertz (usually 800khz)
    LED_DMA = 10           # DMA channel to use for generating signal (try 10)
    LED_BRIGHTNESS = 255   # Set to 0 for darkest and 255 for brightest
    LED_INVERT = False     # True to invert the signal (when using NPN transistor level shift)
    LED_CHANNEL = 0
    LED_STRIP = ws.SK6812_STRIP_RGBW
    wait_ms = 10
    strip = PixelStrip(LED_COUNT, LED_PIN, LED_FREQ_HZ, LED_DMA, LED_INVERT, LED_BRIGHTNESS, LED_CHANNEL, LED_STRIP)
    strip.begin()
    print("\nLED strip started.")

def set_color(rgbNumber,red, green, blue, white):
    global strip  # Use the global keyword to access the global variable
    strip.setPixelColorRGBW(rgbNumber, red, green, blue, white)
    strip.show()
