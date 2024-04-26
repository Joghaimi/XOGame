using Library.GPIOLib;
using Library;
using Library.RGBLib;
using Library.Model;
using Library.PinMapping;

namespace CatchyGame.Service
{
    public class SenarioService : IHostedService, IDisposable
    {

        Strip strip1, strip2, strip3, strip4;
        RGBButton rgb1;
        private CancellationTokenSource _cts, _cts2;
        public Task StartAsync(CancellationToken cancellationToken)
        {

            MCP23Controller.Init(Room.Fort);
            // RGB Pixel Button 
            rgb1 = new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1);

            RGBButtonPixel rGBButton1Pixel = new RGBButtonPixel(20, rgb1);
            RGBButtonPixel rGBButton2Pixel = new RGBButtonPixel(40, new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4));
            RGBButtonPixel rGBButton3Pixel = new RGBButtonPixel(23, new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5));
            RGBButtonPixel rGBButton4Pixel = new RGBButtonPixel(41, new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8));
            var rgb2 = new RGBButton(RGBButtonPin.RGBR4, RGBButtonPin.RGBG4, RGBButtonPin.RGBB4, RGBButtonPin.RGBPB4);
            var rgb3 = new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5);
            var rgb4 = new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8);
            while (true)
            {
                Console.WriteLine("RED");
                rgb1.TurnColorOn(RGBColor.Red);
                //rgb2.TurnColorOn(RGBColor.Red);
                //rgb3.TurnColorOn(RGBColor.Red);
                //rgb4.TurnColorOn(RGBColor.Red);
                Thread.Sleep(2000);
                Console.WriteLine("Green");

                rgb1.TurnColorOn(RGBColor.Green);
                //rgb2.TurnColorOn(RGBColor.Green);
                //rgb4.TurnColorOn(RGBColor.Green);
                //rgb3.TurnColorOn(RGBColor.Green);
                Thread.Sleep(2000);
                Console.WriteLine("Blue");

                rgb1.TurnColorOn(RGBColor.Blue);
                //rgb2.TurnColorOn(RGBColor.Blue);
                //rgb4.TurnColorOn(RGBColor.Blue);
                //rgb3.TurnColorOn(RGBColor.Blue);
                Thread.Sleep(2000);

            }


            //Init Strip
            strip1 = new Strip(RGBColor.Yellow, 0, 89, rGBButton1Pixel, rGBButton2Pixel, rGBButton3Pixel, rGBButton4Pixel);


            // Init the RGBStrip 
            RGBWS2811.Init();
            // Init RGB Button 




            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlRGBLight(_cts.Token));
            //Task.Run(() => ControlRGBButton(_cts2.Token));
            return Task.CompletedTask;
        }

        private async Task ControlRGBLight(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                RGBWS2811.SetColor(strip1.currentLed, strip1.rgbColor);
                RGBWS2811.Commit();
                //strip1.NextLed();
                //if (strip1.rGBButton1.Pixel == strip1.currentLed)
                //{
                //    strip1.rGBButton1.Button.TurnColorOn(RGBColor.Red);
                //}
                //else
                //{
                //    rgb1.TurnColorOn(RGBColor.Blue);
                //}
                //rgb1
                Thread.Sleep(1000);
                Console.Write(".");
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
