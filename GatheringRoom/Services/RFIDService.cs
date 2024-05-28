using GatheringRoom.Controllers;
using Library.Media;
using Library;
using Library.PinMapping;
using Library.RFIDLib;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using Library.APIIntegration;
using Library.Model;

namespace GatheringRoom.Services
{
    public class RFIDService : IHostedService, IDisposable
    {
        private readonly ILogger<GatheringRoomController> _logger;
        private CancellationTokenSource _cts, _cts2;
        private RFID _rfidController = new();
        RFIDSerial rfid = new RFIDSerial(SerialPort.Serial);
        int pinReset = 6;
        bool stop = false;
        private string token = "";
        public RFIDService(ILogger<GatheringRoomController> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start RFID Service");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _cts2 = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _rfidController.Init(pinReset);
            Task.Run(() => RunService(_cts.Token));
            //Task.Run(() => RefreshTokenService(_cts2.Token));
            return Task.CompletedTask;
        }

        private async Task RunService(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                string playerId = rfid.GetRFIDUID();
                if (!playerId.Contains("None"))
                {
                    AudioPlayer.PIStartAudio(SoundType.ScanId);
                    if (VariableControlService.TeamScore.player.Count < 5 && !stop)
                    {
                        VariableControlService.IsTheGameStarted = true;
                        string[] originString = Enumerable.Range(0, playerId.Length / 2).Select(i => playerId.Substring(i * 2, 2)).ToArray(); ;//newPlayerId.Split("-");
                        playerId = "";
                        _logger.LogTrace($"PlayerId Before {playerId}");
                        for (int j = (originString.Length - 1); j >= 0; j--)
                        {
                            playerId += originString[j];
                        }
                        _logger.LogTrace("New Card Found");
                        _logger.LogTrace($"PlayerId After {playerId}");
                        var newPlayer = await APIIntegration.ReturnPlayerInformation(VariableControlService.UserName, VariableControlService.Password, VariableControlService.UserInfoURL, playerId);
                        if (newPlayer != null)
                        {
                            bool isInTeam = VariableControlService.TeamScore.player.Any(item => item.Id == newPlayer.Id);
                            _logger.LogTrace($"isInTeam {isInTeam}");
                            if (!isInTeam)
                            {
                                VariableControlService.TeamScore.player.Add(new Player { Id = newPlayer.Id, FirstName = newPlayer.FirstName, LastName = newPlayer.LastName, MobileNumber = newPlayer.MobileNumber });
                                _logger.LogDebug($"Player {newPlayer?.FirstName} {newPlayer.LastName}");
                            }
                            else
                                _logger.LogWarning($"Player is Exist :{isInTeam}");
                        }
                    }
                    else
                        _logger.LogWarning($"Can't Have More Than 5 Member in a team");
                }
                await Task.Delay(TimeSpan.FromMilliseconds(1000), cancellationToken);
            }
        }
        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("RFID Service Stopped");
            _cts.Cancel();
            return Task.CompletedTask;
        }
        public void Dispose()
        {
            _logger.LogInformation("RFID Service Disposed");
            _cts.Dispose();
        }

        private async Task RefreshTokenService(CancellationToken cancellationToken)
        {
            while (true)
            {
                _logger.LogTrace("Start Refresh Token ..");
                token = "";
                while (token == "")
                {
                    token = await APIIntegration.AuthorizationAsync(VariableControlService.AuthorizationURL, VariableControlService.UserName, VariableControlService.Password);
                    Thread.Sleep(5000);
                }
                _logger.LogTrace($"New Token {token}");
                Thread.Sleep(900000);
            }
        }



    }
}

class Person
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string mobilenumber { get; set; }
    public string email { get; set; }
}

