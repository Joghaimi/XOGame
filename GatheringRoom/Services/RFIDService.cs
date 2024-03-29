﻿using GatheringRoom.Controllers;
using Library.Media;
using Library;
using Library.PinMapping;
using Library.RFIDLib;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;

namespace GatheringRoom.Services
{
    public class RFIDService : IHostedService, IDisposable
    {
        private readonly ILogger<GatheringRoomController> _logger;
        private CancellationTokenSource _cts;
        private RFID _rfidController = new();
        RFIDSerial rfid = new RFIDSerial(SerialPort.Serial);
        int pinReset = 6;
        bool stop = false;
        public RFIDService(ILogger<GatheringRoomController> logger)
        {
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Start RFID Service");
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _rfidController.Init(pinReset);
            Task.Run(() => RunService(_cts.Token));
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
                        VariableControlService.IsTheGameStarted=true;
                        string[] originString = Enumerable.Range(0, playerId.Length / 2).Select(i => playerId.Substring(i * 2, 2)).ToArray(); ;//newPlayerId.Split("-");
                        playerId = "";
                        _logger.LogTrace($"PlayerId Before {playerId}");

                        for (int j = (originString.Length - 1); j >= 0; j--)
                        {
                            playerId += originString[j];
                        }
                        bool isInTeam = VariableControlService.TeamScore.player.Any(item => item.Id == playerId);
                        _logger.LogTrace("New Card Found");
                        _logger.LogTrace($"PlayerId After {playerId}");
                        _logger.LogTrace($"isInTeam {isInTeam}");
                        if ( !isInTeam)
                        {
                            using (HttpClient httpClient = new HttpClient())
                            {
                                string apiUrl = "https://thcyle7652.execute-api.us-east-1.amazonaws.com/default/myservice-dev-hello";
                                string jsonData = "{\"rfid\":\"" + playerId + "\"}";
                                StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                                _logger.LogDebug($"Send Request");
                                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                                _logger.LogDebug($"response.IsSuccessStatusCode {response.IsSuccessStatusCode}");

                                if (response.IsSuccessStatusCode)
                                {
                                    string responseContent = await response.Content.ReadAsStringAsync();
                                    List<Person> people = JsonConvert.DeserializeObject<List<Person>>(responseContent);
                                    if (people?.Count > 0)
                                    {
                                        var player = new Player
                                        {
                                            Id = playerId,
                                            FirstName = people[0].firstname,
                                            LastName = people[0].lastname
                                        };
                                        VariableControlService.TeamScore.player.Add(player);
                                        _logger.LogDebug($"Player {people[0].firstname} {people[0].lastname}");
                                    }
                                    _logger.LogError($"POST request successful {responseContent}. Response: {people[0].firstname} {people[0].lastname}");
                                }
                                else
                                    _logger.LogError($"POST request failed. Status Code: {response.StatusCode}");
                            }
                        }
                        else
                            _logger.LogWarning($"Player is Exist :{isInTeam}");
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
    }
}

class Person
{
    public string firstname { get; set; }
    public string lastname { get; set; }
    public string mobilenumber { get; set; }
    public string email { get; set; }
}

