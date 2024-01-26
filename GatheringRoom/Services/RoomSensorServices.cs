﻿using Library.GPIOLib;
using Library.RFIDLib;
using Microsoft.AspNetCore.Mvc;
using NAudio.SoundFont;
using System.Device.Gpio;

namespace GatheringRoom.Services
{
    public class RoomSensorServices : IHostedService, IDisposable
    {
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        private int PIRPin1 = 26;
        private int PIRPin2 = 19;
        private int PIRPin3 = 13;
        private int PIRPin4 = 6;
        private int LightSwitch = 6;
        private int DoorRelay = 6;
        private GPIOController _controller;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _controller = new GPIOController();
            // Init the Pin's
            _controller.Setup(PIRPin1, PinMode.InputPullDown);
            _controller.Setup(PIRPin3, PinMode.InputPullDown);
            _controller.Setup(PIRPin4, PinMode.InputPullDown);
            _controller.Setup(PIRPin2, PinMode.InputPullDown);
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                PIR1 = _controller.Read(PIRPin1);
                PIR2 = _controller.Read(PIRPin2);
                PIR3 = _controller.Read(PIRPin3);
                PIR4 = _controller.Read(PIRPin4);
                //Console.WriteLine($"PIR Status PIR1:{PIR1} PIR2:{PIR2} PIR3{PIR3} PIR4:{PIR4}");
                bool isAnyOfRIPSensorActive = PIR1 || PIR2 || PIR3 || PIR4 || isTheirAreSomeOneInTheRoom;
                if (isAnyOfRIPSensorActive && !isTheirAreSomeOneInTheRoom)
                {
                    Console.WriteLine("Some One In The Room");
                    // Turn the Light on 
                    Console.WriteLine("Switch Light On"); // To Do

                    // rise a flag 
                    isTheirAreSomeOneInTheRoom = true;
                }

                // IF Enable Going To The Next room 
                if (VariableControlService.EnableGoingToTheNextRoom)
                {
                    Console.WriteLine("open The Door ....");
                    VariableControlService.EnableGoingToTheNextRoom = false;
                    isTheirAreSomeOneInTheRoom = false;
                }


                await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _cts.Dispose();
        }
    }
}
