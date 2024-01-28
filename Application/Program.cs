using Iot.Device.Card.Ultralight;
using Iot.Device.Mfrc522;
using Iot.Device.Rfid;
using Library.GPIOLib;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using NAudio.Codecs;
using NAudio.SoundFont;
using System;
using System.Device.Gpio;
using System.Device.I2c;
using System.Device.Spi;
using System.Text;
using System.Threading;
using Iot.Device.Board;
using Iot.Device.Card.Mifare;
using Iot.Device.Mcp25xxx.Register.ErrorDetection;
using System.Diagnostics;
using Iot.Device.Mcp23xxx;
using Library.Display;
using Library.Enum;
using Library;
using System.Numerics;
using Library.Modbus;
using Iot.Device.Mcp3428;

// Test MCP0 ==== The Main Board 
// Output 
//MCP23Controller test = new MCP23Controller();
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 0, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 1, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 2, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 3, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 4, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 5, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 6, PinMode.Output);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortB, 7, PinMode.Output);

//while (true)
//{
//    //test.Write(MCP23017.MCP2301720, Port.PortB, 0, PinState.High);
//    for (int i = 0; i < 8; i++)
//    {
//        test.Write(MCP23017.MCP2301720, Port.PortB, i, PinState.High);
//        Console.WriteLine($"High {i}");
//        Thread.Sleep(1000);
//        //test.Write(MCP23017.MCP2301720, Port.PortB, i, PinState.Low);
//        //Console.WriteLine($"Low {i}");
//        //Thread.Sleep(6000);
//    }

//    //for (int i = 8; i > 0; i--)
//    //{
//    //    test.Write(MCP23017.MCP2301720, Port.PortB, i, PinState.Low);
//    //    Console.WriteLine("Low");
//    //    Thread.Sleep(1000);
//    //}

//}

//=================== input 
//MCP23Controller test = new MCP23Controller();
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 0, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 1, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 2, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 3, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 4, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 5, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 6, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301720, Port.PortA, 7, PinMode.Input);

//while (true)
//{
//    Console.Write("0b");
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 0));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 1));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 2));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 3));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 4));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 5));
//    Console.Write(test.Read(MCP23017.MCP2301720, Port.PortA, 6));
//    Console.WriteLine(test.Read(MCP23017.MCP2301720, Port.PortA, 7));

//    Thread.Sleep(1000);
//}
//================= Test the IO 
//GPIOController gpio = new GPIOController();
//Console.WriteLine("Turn Them On");
//gpio.Setup(4, PinMode.Output);
//gpio.Setup(17, PinMode.Output);
//gpio.Setup(27, PinMode.Output);
//gpio.Setup(22, PinMode.Output);
//gpio.Setup(23, PinMode.Output);
//gpio.Setup(24, PinMode.Output);

//gpio.Write(4, true);
//gpio.Write(17, true);
//gpio.Write(27, true);
//gpio.Write(22, true);
//gpio.Write(23, true);
//gpio.Write(24, true);
//while (true)
//{
//    Console.WriteLine(".");
//    Thread.Sleep(1000);
//}


//======== Serial Port 




















// Test Mobus 

//var modbus = new ModbusLib();
//modbus.Init(SerialPortMapping.PortMap["SerialTest"]);
//while (true)
//{
//    modbus.WriteSingleRegister(1, 4001, 2);
//    Thread.Sleep(1000);
//}


// mcp23017
//Console.WriteLine("Hello World!");
var displayControl = new DisplayController();
displayControl.Init(SerialPortMapping.PortMap["Serial0"]);

//var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
//var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);

//////Create an instance of MCP23017
//var mcp23017x20 = new Mcp23017(i2cDevicex20);

////// Set the I/O direction for PortA and PortB (0 means output, 1 means input)
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortA);
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortB);

while (true)
{
    ////mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
    displayControl.SendCommand(Displays.first, DisplayCommand.rightArrow);
    Thread.Sleep(1000);
    ////mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
    //displayControl.SendCommand(Displays.first, DisplayCommand.clear);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.second, DisplayCommand.rightArrow);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.second, DisplayCommand.clear);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.third, DisplayCommand.rightArrow);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.third, DisplayCommand.clear);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.fourth, DisplayCommand.rightArrow);
    //Thread.Sleep(1000);
    //displayControl.SendCommand(Displays.fourth, DisplayCommand.clear);
    //Thread.Sleep(1000);
    //mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
    //Task.Delay(1000).Wait();
    //mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
    //Task.Delay(1000).Wait();

}







// Configure I2C connection settings for MCP23017
//var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
//var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);

// Create an instance of MCP23017
//var mcp23017x20 = new Mcp23017(i2cDevicex20);

//// Set the I/O direction for PortA and PortB (0 means output, 1 means input)
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortA);
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortB);
//while (true)
//{
//    mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
//    Task.Delay(1000).Wait();
//    mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
//    Task.Delay(1000).Wait();
//}

// RFID Test
//GpioController gpioController = new GpioController();
//int pinReset = 25;

//SpiConnectionSettings connection = new(0, 0);
//connection.ClockFrequency = 10_000_000;
//SpiDevice spi = SpiDevice.Create(connection);
//MfRc522 mfrc522 = new(spi, pinReset, gpioController, false);
//byte[] buffer = new byte[2];
//bool res;
//Data106kbpsTypeA card;
//do
//{
//    res = mfrc522.ListenToCardIso14443TypeA(out card, TimeSpan.FromSeconds(2));
//    if (res)
//    {
//        Console.WriteLine("card");
//    }
//    else
//    {
//        Console.WriteLine("No card.");
//        Console.WriteLine(mfrc522.IsCardPresent(buffer, false));
//    }
//    Thread.Sleep(res ? 0 : 1000);
//}
//while (!res);
//if (UltralightCard.IsUltralightCard(card.Atqa, card.Sak))
//{
//    Debug.WriteLine("Ultralight card detected, running various tests.");
//    //ProcessUltralight();
//}
//else
//{
//    Debug.WriteLine("Mifare card detected, dumping the memory.");
//    ProcessMifare();
//}
//void ProcessMifare()
//{
//    var mifare = new MifareCard(mfrc522!, 0);
//    mifare.SerialNumber = card.NfcId;

//    mifare.Capacity = MifareCardCapacity.Mifare1K;
//    mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
//    mifare.KeyB = MifareCard.DefaultKeyB.ToArray();
//    int ret;

//    for (byte block = 0; block < 64; block++)
//    {
//        mifare.BlockNumber = block;
//        mifare.Command = MifareCardCommand.AuthenticationB;
//        ret = mifare.RunMifareCardCommand();
//        if (ret < 0)
//        {
//            // If you have an authentication error, you have to deselect and reselect the card again and retry
//            // Those next lines shows how to try to authenticate with other known default keys
//            mifare.ReselectCard();
//            // Try the other key
//            mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
//            mifare.Command = MifareCardCommand.AuthenticationA;
//            ret = mifare.RunMifareCardCommand();
//            if (ret < 0)
//            {
//                mifare.ReselectCard();
//                mifare.KeyA = MifareCard.DefaultBlocksNdefKeyA.ToArray();
//                mifare.Command = MifareCardCommand.AuthenticationA;
//                ret = mifare.RunMifareCardCommand();
//                if (ret < 0)
//                {
//                    mifare.ReselectCard();
//                    mifare.KeyA = MifareCard.DefaultFirstBlockNdefKeyA.ToArray();
//                    mifare.Command = MifareCardCommand.AuthenticationA;
//                    ret = mifare.RunMifareCardCommand();
//                    if (ret < 0)
//                    {
//                        mifare.ReselectCard();
//                        Debug.WriteLine($"Error reading bloc: {block}");
//                    }
//                }
//            }
//        }

//        if (ret >= 0)
//        {
//            mifare.BlockNumber = block;
//            mifare.Command = MifareCardCommand.Read16Bytes;
//            ret = mifare.RunMifareCardCommand();
//            if (ret >= 0)
//            {
//                if (mifare.Data is object)
//                {
//                    Debug.WriteLine($"Bloc: {block}, Data: {BitConverter.ToString(mifare.Data)}");
//                }
//            }
//            else
//            {
//                mifare.ReselectCard();
//                Debug.WriteLine($"Error reading bloc: {block}");
//            }

//            if (block % 4 == 3)
//            {
//                if (mifare.Data != null)
//                {
//                    // Check what are the permissions
//                    for (byte j = 3; j > 0; j--)
//                    {
//                        var access = mifare.BlockAccess((byte)(block - j), mifare.Data);
//                        Debug.WriteLine($"Bloc: {block - j}, Access: {access}");
//                    }

//                    var sector = mifare.SectorTailerAccess(block, mifare.Data);
//                    Debug.WriteLine($"Bloc: {block}, Access: {sector}");
//                }
//                else
//                {
//                    Debug.WriteLine("Can't check any sector bloc");
//                }
//            }
//        }
//        else
//        {
//            Debug.WriteLine($"Authentication error");
//        }
//    }
//}




// === Test Led Work

//Console.WriteLine("Blinking LED. Press Ctrl+C to end.");
//int pin = 27;
//using var controller = new GpioController();
//controller.OpenPin(pin, PinMode.Input);
//var mode = controller.GetPinMode(pin);
//bool ledOn = true;
//Console.WriteLine($"Mode {mode}");
//while (true)
//{
//    var value = controller.Read(pin);
//    Console.WriteLine($"status {value}");



//    //controller.Write(pin, ((ledOn) ? PinValue.High : PinValue.Low));
//    Thread.Sleep(1000);
//    ;
//}
//GPIOController gPIO =new GPIOController();
//gPIO.

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






