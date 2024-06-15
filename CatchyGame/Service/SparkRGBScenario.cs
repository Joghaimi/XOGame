using Iot.Device.BrickPi3.Sensors;
using Library.GPIOLib;
using Library;
using Library.PinMapping;
using Library.RGBLib;


namespace CatchyGame.Service
{
    public class SparkRGBScenario : IHostedService, IDisposable
    {



        private CancellationTokenSource _cts;
        List<SparkRGBButton> RGBButtonList = new List<SparkRGBButton>();
        public Task StartAsync(CancellationToken cancellationToken)
        {


            MCP23Controller.Init(Room.Fort);


            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR1, RGBButtonPin.RGBG1, RGBButtonPin.RGBB1, RGBButtonPin.RGBPB1), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR2, RGBButtonPin.RGBG2, RGBButtonPin.RGBB2, RGBButtonPin.RGBPB2), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR3, RGBButtonPin.RGBG3, RGBButtonPin.RGBB3, RGBButtonPin.RGBPB3), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR4Extra, RGBButtonPin.RGBG4Extra, RGBButtonPin.RGBB4Extra, RGBButtonPin.RGBPB4Extra), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR5, RGBButtonPin.RGBG5, RGBButtonPin.RGBB5, RGBButtonPin.RGBPB5), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR6, RGBButtonPin.RGBG6, RGBButtonPin.RGBB6, RGBButtonPin.RGBPB6), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR7, RGBButtonPin.RGBG7, RGBButtonPin.RGBB7, RGBButtonPin.RGBPB7), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR8, RGBButtonPin.RGBG8, RGBButtonPin.RGBB8, RGBButtonPin.RGBPB8), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR9, RGBButtonPin.RGBG9, RGBButtonPin.RGBB9, RGBButtonPin.RGBPB9), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR10, RGBButtonPin.RGBG10, RGBButtonPin.RGBB10, RGBButtonPin.RGBPB10), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR11, RGBButtonPin.RGBG11, RGBButtonPin.RGBB11, RGBButtonPin.RGBPB11), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR12Extra, RGBButtonPin.RGBG12Extra, RGBButtonPin.RGBB12Extra, RGBButtonPin.RGBPB12Extra), 5, Library.RGBColor.Green));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR13, RGBButtonPin.RGBG13, RGBButtonPin.RGBB13, RGBButtonPin.RGBPB13), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR14, RGBButtonPin.RGBG14, RGBButtonPin.RGBB14, RGBButtonPin.RGBPB14), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR15, RGBButtonPin.RGBG15, RGBButtonPin.RGBB15, RGBButtonPin.RGBPB15), 5, Library.RGBColor.Blue));
            RGBButtonList.Add(new SparkRGBButton(new RGBButton(RGBButtonPin.RGBR16, RGBButtonPin.RGBG16, RGBButtonPin.RGBB16, RGBButtonPin.RGBPB16), 5, Library.RGBColor.Green));




            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => ControlGame(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task ControlGame(CancellationToken cancellationToken)
        {
            while (true)
            {
                var i = 0;

                foreach (var button in RGBButtonList)
                {
                    Console.WriteLine(i);
                    button.Activate(true);
                    //Thread.Sleep(500);
                    i++;
                }
                //Thread.Sleep(2000);

                //foreach (var button in RGBButtonList)
                //{
                //    Console.WriteLine(i);
                //    button.Activate(false);
                //    //Thread.Sleep(500);
                //    i++;
                //}
                //Thread.Sleep(2000);

                while (true)
                {
                    int b = 0;
                    foreach (var bu in RGBButtonList)
                    {
                        if (bu.isPressed() > 0)
                            Console.WriteLine(i++);
                        b++;
                    }

                }






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
