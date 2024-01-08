// See https://aka.ms/new-console-template for more information
using Library.Media;
// TO DO 
// --- Voice Test 
Console.WriteLine("Hello, World!");
var soundPath = "C:\\Users\\Ahmad\\Desktop\\beep-02.wav";
var soundLibrary = new AudioPlayer();
soundLibrary.StartAudio(soundPath);
System.Threading.Thread.Sleep(1000);
soundLibrary.StopAudio();

// Testing 
// Dependency injection 






