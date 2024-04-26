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
using Library.Media;
using Library.RGBLib;
using Library.PinMapping;
using NAudio.Wave;
using Newtonsoft.Json;
using Library.RFIDLib;
using Library.APIIntegration;
using GatheringRoom.Services;
using Python.Runtime;

Console.WriteLine("Hello, World!");
test();

void test()
{
    Runtime.PythonDLL = "/usr/lib/python3.11/config-3.11-aarch64-linux-gnu/libpython3.11.so";

    //Runtime.PythonDLL = "C:\\Python312\\python312.dll";
    PythonEngine.Initialize();
    using (Py.GIL())
    {
        var python = Py.Import("/home/catchy/XOGame/Application/ahmad");
        var iscallable = python.IsCallable();
        python.InvokeMethod("print_ahmad");
        python.InvokeMethod("print_ahmad");
        python.InvokeMethod("print_ahmad");
        python.InvokeMethod("print_ahmad");
        python.InvokeMethod("print_ahmad");
    }


    //using (Py.GIL()) // Acquire the Global Interpreter Lock (GIL)
    //{
    //    dynamic pyModule = Py.Import("my_python_script.py"); // Import your Python script
    //    int result = pyModule.add(5, 3); // Call a Python function
    //    Console.WriteLine(result); // Print the result
    //}
}





// Initialize a new instance of the wrapper
//var neopixel = new ws281x.Net.Neopixel(ledCount: 42, pin: 10);

//// You can also choose a custom color order
//neopixel = new ws281x.Net.Neopixel(ledCount: 42, pin: 10, stripType: rpi_ws281x.WS2811_STRIP_RBG);

//// Always initialize the wrapper first
//neopixel.Begin();

//// Set color of all LEDs to red
//for (var i = 0; i < neopixel.GetNumberOfPixels(); i++)
//{
//    neopixel.SetPixelColor(i, System.Drawing.Color.Red);
//}

//// Apply changes to the led
//neopixel.Show();

//// Dispose after use
//neopixel.Dispose();
//while (true)
//{

//    //var token = await APIIntegration.AuthorizationAsync(VariableControlService.AuthorizationURL, VariableControlService.UserName, VariableControlService.Password);
//    //Console.WriteLine(token);
//    //if (token != "")
//    //{
//    //    Console.WriteLine("Get User Info");
//    //    //var result = await APIIntegration.ReturnPlayerInformation(VariableControlService.UserInfoURL,token , "84436C18");
//    //    //Console.WriteLine(result);
//    //}
//    var result = await APIIntegration.ReturnPlayerInformation(VariableControlService.UserName, VariableControlService.Password,VariableControlService.UserInfoURL, "84436C18");
//    Console.WriteLine($"result :{result}");
//    Thread.Sleep(5000);
//}




// === RGB Strip
//float hue = 00;
//bool up = true;

//RGBLight.Init((byte)MasterOutputPin.Clk, (byte)MasterOutputPin.Data, 5);
//while (true)
//{
//    for (byte i = 0; i < 10; i++)
//        RGBLight.SetColorHSL(i, hue, 1, (float)0.5);
//    Thread.Sleep(100);
//    if (up)
//        hue += (float)0.025;
//    else
//        hue -= (float)0.025;

//    if (hue >= 1.0 && up)
//        up = false;
//    else if (hue <= 0.0 && !up)
//        up = true;

//}


//Console.WriteLine(  "Started ... ");
//RGBLight.Init(MasterOutputPin.Clk, MasterOutputPin.Data);
////RGBLight.BeginTransition();
//RGBLight.SetColor(RGBColor.Red);
////RGBLight.EndTransition();
//while (true)
//{
//    Thread.Sleep(3000);

//    //Console.WriteLine("Red");
//    //RGBLight.BeginTransition();
//    //RGBLight.SetColor(RGBColor.Red);
//    //RGBLight.EndTransition();

//    //Thread.Sleep(3000);
//    //Console.WriteLine("Green");

//    //RGBLight.SetColor(RGBColor.Green);
//    //Thread.Sleep(3000);
//    //Console.WriteLine("Blue");
//    //RGBLight.SetColor(RGBColor.Blue);
//    //Thread.Sleep(3000);
//    //RGBLight.SetColor(RGBColor.Off);
//    //Thread.Sleep(3000);
//}


// === Test check sum 
//PlayAudio(0x8);
//Console.WriteLine(((byte)8 + 0xB3).ToString("X2"));

//void PlayAudio(int trackNumber)
//{
//    byte[] command = { 0xAA, 0x07, 0x02, 0x00, (byte)trackNumber, (byte)(trackNumber + 0xB3) };
//    Console.WriteLine((byte)(trackNumber + 0xB3));
//}
//byte[] command = { 0xAA, 0x04, 0x00, 0xAE };
//Console.WriteLine(CalculateChecksum(command));



//byte CalculateChecksum(byte[] data)
//{


//    // Calculate the checksum which forms the end byte
//    byte MP3_CHECKSUM = data + command + requestLength;

//    for (uint8_t x = 0; x < requestLength; x++)
//    {
//        MP3_CHECKSUM += (uint8_t)requestBuffer[x];
//    }

//    int sum = 0;

//    // Exclude the start byte (0xAA) and end byte (0xBB)
//    for (int i = 1; i < data.Length - 1; i++)
//    {
//        sum += data[i];
//    }

//    // Take the least significant 8 bits
//    byte checksum = (byte)(sum & 0xFF);

//    return checksum;
//}
// ==== 


//MCP23Pin rGBPinMapping = new MCP23Pin
//{
//    PinNumber = 0, // Set the PinNumber property
//    Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
//    port = Port.PortB
//};

//MCP23Pin gGBPinMapping = new MCP23Pin
//{
//    PinNumber = 1, // Set the PinNumber property
//    Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
//    port = Port.PortB
//};
//MCP23Pin bGBPinMapping = new MCP23Pin
//{
//    PinNumber = 2, // Set the PinNumber property
//    Chip = MCP23017.MCP2301726, // Set the Chip property with the MCP23017 instance
//    port = Port.PortB
//};
//MCP23Pin ButtonGBPinMapping = new MCP23Pin
//{
//    PinNumber = 4, // Set the PinNumber property
//    Chip = MCP23017.MCP2301727, // Set the Chip property with the MCP23017 instance
//    port = Port.PortB
//};


//new RGBPinMapping(0, MCP23017.MCP2301726, Port.PortB);
//RGBPinMapping gGBPinMapping = new RGBPinMapping(1, MCP23017.MCP2301726, Port.PortB);
//RGBPinMapping bGBPinMapping = new RGBPinMapping(2, MCP23017.MCP2301726, Port.PortB);
//RGBPinMapping ButtonGBPinMapping = new RGBPinMapping(4, MCP23017.MCP2301727, Port.PortB);


//RGBButton x = new RGBButton(rGBPinMapping, gGBPinMapping, bGBPinMapping, ButtonGBPinMapping);
//RGBButton y = new RGBButton(rGBPinMapping, gGBPinMapping, bGBPinMapping, ButtonGBPinMapping);


//while (true)
//{

//    Console.WriteLine(x.CurrentStatus());
//    x.TurnColorOn(RGBColor.Red);
//    Thread.Sleep(1000);
//    Console.WriteLine(y.CurrentStatus());
//    y.TurnColorOn(RGBColor.Red);
//    Thread.Sleep(1000);

//    Console.WriteLine("RED Blue");
//    x.TurnColorOn(RGBColor.Blue);
//    Thread.Sleep(2000);

//    Console.WriteLine("RED Green");
//    x.TurnColorOn(RGBColor.Green);
//    Thread.Sleep(2000);

//}





// ====== U11 

//=================== input 
//MCP23Controller test = new MCP23Controller(true);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 0, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 1, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 2, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 3, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 4, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 5, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 6, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortA, 7, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 0, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 1, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 2, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 3, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 4, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 5, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 6, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301721, Port.PortB, 7, PinMode.Input);

//// U12
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 0, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 1, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 2, PinMode.Input);
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 3, PinMode.Input);

//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 4, PinMode.Output); LED #12
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 5, PinMode.Output); LED #11 
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 6, PinMode.Output); LED #10
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortA, 7, PinMode.Output); LED #09
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 0, PinMode.Output); Output 8
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 1, PinMode.Output); Output 7
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 2, PinMode.Output); Output 6
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 3, PinMode.Output); Output 5
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 4, PinMode.Output); Output 4
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 5, PinMode.Output); Output 3
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 6, PinMode.Output); Output 2
//test.PinModeSetup(MCP23017.MCP2301722, Port.PortB, 7, PinMode.Output); Output 0

//// U29  ---> 23
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 0, PinMode.Output); Output 20
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 1, PinMode.Output); Output 19
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 2, PinMode.Output); Output 18
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 3, PinMode.Output); Output 17
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 4, PinMode.Output); Output 16
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 5, PinMode.Output); Output 15
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 6, PinMode.Output); Output 14
//test.PinModeSetup(MCP23017.MCP2301723, Port.PortB, 7, PinMode.Output); Output 13
//// U22 
//while (true)
//{
//    for (int i = 0; i < 8; i++)
//    {
//        Console.WriteLine($"LED #{20 - i} High");
//        test.Write(MCP23017.MCP2301723, Port.PortB, i, PinState.High);
//        Thread.Sleep(2000);
//        Console.WriteLine($"LED #{20 - i} Low");
//        test.Write(MCP23017.MCP2301723, Port.PortB, i, PinState.Low);
//        Thread.Sleep(2000);
//    }
//}

// ==== U12
//while (true)
//{
//    //for (int i = 4; i < 8; i++)
//    //{
//    //Console.WriteLine($"LED #12 High");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 4, PinState.High);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #12 Low");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 4, PinState.Low);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #11 High");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 5, PinState.High);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #11 Low");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 5, PinState.Low);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #10 High");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 6, PinState.High);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #10 Low");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 6, PinState.Low);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #9 High");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 7, PinState.High);
//    //Thread.Sleep(2000);
//    //Console.WriteLine($"LED #9 Low");
//    //test.Write(MCP23017.MCP2301722, Port.PortA, 7, PinState.Low);
//    //Thread.Sleep(2000);

//    ////}
//    //for (int i = 0; i < 8; i++)
//    //{
//    //    Console.WriteLine($"LED #{8 - i} High");
//    //    test.Write(MCP23017.MCP2301722, Port.PortB, i, PinState.High);
//    //    Thread.Sleep(2000);
//    //    Console.WriteLine($"LED #{8 - i} Low");
//    //    test.Write(MCP23017.MCP2301722, Port.PortB, i, PinState.Low);
//    //    Thread.Sleep(2000);
//    //}
//    Console.WriteLine("HIGH");


//}



//test.Write(MCP23017.MCP2301722, Port.PortA, 4, PinState.High);
//test.Write(MCP23017.MCP2301722, Port.PortA, 5, PinState.High);
//test.Write(MCP23017.MCP2301722, Port.PortA, 6, PinState.High);
//test.Write(MCP23017.MCP2301722, Port.PortA, 7, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortA, 8, PinState.High);

//test.Write(MCP23017.MCP2301722,2Port.PortB, 0, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 1, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 2, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 3, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 4, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 5, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 6, PinState.High);
//test.Write(MCP23017.MCP2301722,2Port.PortB, 7, PinState.High);





//while (true)
//{
//    Console.Write("Port A : 0b");
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 0) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 1) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 2) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 3) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 4) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 5) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortA, 6) ? "1" : 0);
//    Console.WriteLine(test.Read(MCP23017.MCP2301721, Port.PortA, 7) ? "1" : 0);
//    Thread.Sleep(100);

//    Console.Write("Port B : 0b");
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 0) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 1) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 2) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 3) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 4) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 5) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301721, Port.PortB, 6) ? "1" : 0);
//    Console.WriteLine(test.Read(MCP23017.MCP2301721, Port.PortB, 7) ? "1" : 0);
//    Thread.Sleep(100);
//    Console.Write("0x22 : 0b");
//    Console.Write(test.Read(MCP23017.MCP2301722, Port.PortA, 0) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301722, Port.PortA, 1) ? "1" : 0);
//    Console.Write(test.Read(MCP23017.MCP2301722, Port.PortA, 2) ? "1" : 0);
//    Console.WriteLine(test.Read(MCP23017.MCP2301722, Port.PortA, 3) ? "1" : 0);
//    Console.WriteLine("================");
//    Thread.Sleep(1000);

//}


// ========================== U12












// ===== Sound 
//JQ8400AudioModule.init(SerialPortMapping.PortMap["Serial2"]);
//while (true)
//{
//    JQ8400AudioModule.PlayAudio((int)SoundType.Start);
//    Thread.Sleep(1000);
//}


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

//        var modbus = new ModbusLib();
//        modbus.Init(SerialPortMapping.PortMap["Serial0"]);
//while (true)
//{
//    //MotorStatus
//    //ModbusAddres
//    Console.WriteLine("Start Motor Forward");
//    modbus.WriteSingleRegister(1, (int) ModbusAddress.startStop, (int) MotorStatus.Run);
//        modbus.WriteSingleRegister(1, (int) ModbusAddress.Speed, (int) MotorSpeed.Medium);
//        Thread.Sleep(5000);
//    Console.WriteLine("Start Motor Reverse");
//    modbus.WriteSingleRegister(1, (int) ModbusAddress.startStop, (int) MotorStatus.Reverse);
//        modbus.WriteSingleRegister(1, (int) ModbusAddress.Speed, (int) MotorSpeed.Slow);
//        Thread.Sleep(5000);

//}



















// Test Mobus 

//var modbus = new ModbusLib();
//modbus.Init(SerialPortMapping.PortMap["Serial0"]);
//while (true)
//{
//    //MotorStatus
//    //ModbusAddres
//    Console.WriteLine("Start Motor Forward");
//    modbus.WriteSingleRegister(1, (int)ModbusAddress.startStop, (int)MotorStatus.Run);
//    modbus.WriteSingleRegister(1, (int)ModbusAddress.Speed, (int)MotorSpeed.Medium);
//    Thread.Sleep(5000);
//    Console.WriteLine("Start Motor Reverse");
//    modbus.WriteSingleRegister(1, (int)ModbusAddress.startStop, (int)MotorStatus.Reverse);
//    modbus.WriteSingleRegister(1, (int)ModbusAddress.Speed, (int)MotorSpeed.Slow);
//    Thread.Sleep(5000);

//}


// mcp23017
//Console.WriteLine("Hello World!");
//var displayControl = new DisplayController();
//displayControl.Init(SerialPortMapping.PortMap["Serial0"]);

//var connectionSettingsx20 = new I2cConnectionSettings(1, 0x20);
//var i2cDevicex20 = I2cDevice.Create(connectionSettingsx20);

//////Create an instance of MCP23017
//var mcp23017x20 = new Mcp23017(i2cDevicex20);

////// Set the I/O direction for PortA and PortB (0 means output, 1 means input)
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortA);
//mcp23017x20.WriteByte(Register.IODIR, 0b0000_0000, Port.PortB);

//while (true)
//{
//    ////mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
//    //displayControl.SendCommand(Displays.first, DisplayCommand.rightArrow);
//    //Thread.Sleep(1000);
//    ////mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
//    //displayControl.SendCommand(Displays.first, DisplayCommand.clear);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.second, DisplayCommand.rightArrow);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.second, DisplayCommand.clear);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.third, DisplayCommand.rightArrow);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.third, DisplayCommand.clear);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.fourth, DisplayCommand.rightArrow);
//    //Thread.Sleep(1000);
//    //displayControl.SendCommand(Displays.fourth, DisplayCommand.clear);
//    //Thread.Sleep(1000);
//    //mcp23017x20.WriteByte(Register.GPIO, 0b0000_0011, Port.PortA);
//    //Task.Delay(1000).Wait();
//    //mcp23017x20.WriteByte(Register.GPIO, 0b0000_0000, Port.PortA);
//    //Task.Delay(1000).Wait();

//}







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
class ProgramEntry
{
    static void Main(string[] args)
    {
        //Console.WriteLine("Init Rfid ..");
        //RFIDSerial rfid = new RFIDSerial(SerialPort.Serial);
        //while (true)
        //{
        //    Console.WriteLine(rfid.GetRFIDUID());
        //    Thread.Sleep(1000);
        //}
        //GpioController gpioController = new GpioController();
        //int pinReset = 6;

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

        //    //for (byte block = 0; block < 64; block++)
        //    //{
        //    //    mifare.BlockNumber = block;
        //    //    mifare.Command = MifareCardCommand.AuthenticationB;
        //    //    ret = mifare.RunMifareCardCommand();
        //    //    if (ret < 0)
        //    //    {
        //    //        // If you have an authentication error, you have to deselect and reselect the card again and retry
        //    //        // Those next lines shows how to try to authenticate with other known default keys
        //    //        mifare.ReselectCard();
        //    //        // Try the other key
        //    //        mifare.KeyA = MifareCard.DefaultKeyA.ToArray();
        //    //        mifare.Command = MifareCardCommand.AuthenticationA;
        //    //        ret = mifare.RunMifareCardCommand();
        //    //        if (ret < 0)
        //    //        {
        //    //            mifare.ReselectCard();
        //    //            mifare.KeyA = MifareCard.DefaultBlocksNdefKeyA.ToArray();
        //    //            mifare.Command = MifareCardCommand.AuthenticationA;
        //    //            ret = mifare.RunMifareCardCommand();
        //    //            if (ret < 0)
        //    //            {
        //    //                mifare.ReselectCard();
        //    //                mifare.KeyA = MifareCard.DefaultFirstBlockNdefKeyA.ToArray();
        //    //                mifare.Command = MifareCardCommand.AuthenticationA;
        //    //                ret = mifare.RunMifareCardCommand();
        //    //                if (ret < 0)
        //    //                {
        //    //                    mifare.ReselectCard();
        //    //                    Debug.WriteLine($"Error reading bloc: {block}");
        //    //                }
        //    //            }
        //    //        }
        //    //    }

        //    //    if (ret >= 0)
        //    //    {
        //    //        mifare.BlockNumber = block;
        //    //        mifare.Command = MifareCardCommand.Read16Bytes;
        //    //        ret = mifare.RunMifareCardCommand();
        //    //        if (ret >= 0)
        //    //        {
        //    //            if (mifare.Data is object)
        //    //            {
        //    //                Debug.WriteLine($"Bloc: {block}, Data: {BitConverter.ToString(mifare.Data)}");
        //    //            }
        //    //        }
        //    //        else
        //    //        {
        //    //            mifare.ReselectCard();
        //    //            Debug.WriteLine($"Error reading bloc: {block}");
        //    //        }

        //    //        if (block % 4 == 3)
        //    //        {
        //    //            if (mifare.Data != null)
        //    //            {
        //    //                // Check what are the permissions
        //    //                for (byte j = 3; j > 0; j--)
        //    //                {
        //    //                    var access = mifare.BlockAccess((byte)(block - j), mifare.Data);
        //    //                    Debug.WriteLine($"Bloc: {block - j}, Access: {access}");
        //    //                }

        //    //                var sector = mifare.SectorTailerAccess(block, mifare.Data);
        //    //                Debug.WriteLine($"Bloc: {block}, Access: {sector}");
        //    //            }
        //    //            else
        //    //            {
        //    //                Debug.WriteLine("Can't check any sector bloc");
        //    //            }
        //    //        }
        //    //    }
        //    //    else
        //    //    {
        //    //        Debug.WriteLine($"Authentication error");
        //    //    }
        //    //}
        //}

    }
}

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






