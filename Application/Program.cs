using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Device.Gpio;
using System.Threading;

Console.WriteLine("Blinking LED. Press Ctrl+C to end.");
int pin = 17;
using var controller = new GpioController();
controller.OpenPin(pin, PinMode.Output);
bool ledOn = true;
while (true)
{
    controller.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
    Thread.Sleep(1000);
    ledOn = !ledOn;
}




// === Test Led 




//using IHost host = CreateHostBuilder(args).Build();

//static IHostBuilder CreateHostBuilder(string[] args)
//{
//    return Host.CreateDefaultBuilder(args)
//        .ConfigureServices((_, services) => { 
//        services.ِAddSingleton<>();
//    });
//}













// TO DO 
// --- Voice Test 
//Console.WriteLine("Hello, World!");
//var soundPath = "C:\\Users\\Ahmad\\Desktop\\beep-02.wav";
//var soundLibrary = new AudioPlayer();
//soundLibrary.StartAudio(Library.SoundType.Beeb);
//System.Threading.Thread.Sleep(1000);
//soundLibrary.StopAudio();

// Digital Output 
//GPIOController ledController = new GPIOController();


// Testing 
// Dependency injection 






