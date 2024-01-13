using Library.RFIDLib;
using NAudio.SoundFont;

namespace GatheringRoom.Services
{
    public class RoomSensorServices : IHostedService, IDisposable
    {
        public bool isTheirAreSomeOneInTheRoom = false;
        private CancellationTokenSource _cts;
        private bool PIR1, PIR2, PIR3, PIR4 = false; // PIR Sensor
        public Task StartAsync(CancellationToken cancellationToken)
        {
            // Init the Pin's






            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }
        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                bool isAnyOfRIPSensorActive = PIR1 || PIR2 || PIR3 || PIR4 || isTheirAreSomeOneInTheRoom;
                if (isAnyOfRIPSensorActive && !isTheirAreSomeOneInTheRoom) {
                    // Turn the Light on 

                    // rise a flag 
                    isTheirAreSomeOneInTheRoom = true;
                }






                //await Task.Delay(TimeSpan.FromMilliseconds(500), cancellationToken);
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
