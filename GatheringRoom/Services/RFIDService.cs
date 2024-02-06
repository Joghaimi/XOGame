using Library.RFIDLib;
using Newtonsoft.Json;
using System;

namespace GatheringRoom.Services
{
    public class RFIDService : IHostedService, IDisposable
    {
        private CancellationTokenSource _cts;
        private RFID _rfidController = new();
        int pinReset = 6;
        bool stop = false;
        public Task StartAsync(CancellationToken cancellationToken)
        {
            _cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _rfidController.Init(pinReset);
            Task.Run(() => RunService(_cts.Token));
            return Task.CompletedTask;
        }

        private async Task RunService(CancellationToken cancellationToken)
        {



            while (!cancellationToken.IsCancellationRequested)
            {
                if (true)
                {
                    //VariableControlService.IsTheGameStarted
                    //Console.WriteLine("Game Started");
                    // Your service logic goes here
                    // Check The RFID 
                    if (_rfidController.CheckCardExisting() && VariableControlService.TeamScore.player.Count < 5 && !stop)
                    {
                        // Read Card Info .. 
                        Console.WriteLine("Card Found .. ");
                        string newPlayerId = _rfidController.ReadCardInfo();
                        if (!string.IsNullOrEmpty(newPlayerId) && !VariableControlService.TeamScore.player.Any(item => item.Id == newPlayerId))
                        {
                            // Refit .. 
                            using (HttpClient httpClient = new HttpClient())
                            {
                                string apiUrl = "https://thcyle7652.execute-api.us-east-1.amazonaws.com/default/myservice-dev-hello";
                                string jsonData = "{\"rfid\":" + newPlayerId + "}";
                                StringContent content = new StringContent(jsonData, System.Text.Encoding.UTF8, "application/json");
                                HttpResponseMessage response = await httpClient.PostAsync(apiUrl, content);
                                if (response.IsSuccessStatusCode)
                                {
                                    // Read the response content as a string
                                    string responseContent = await response.Content.ReadAsStringAsync();
                                    List<Person> people = JsonConvert.DeserializeObject<List<Person>>(responseContent);
                                    if (people.Count > 0)
                                    {
                                        var player = new Player
                                        {
                                            Id = newPlayerId,
                                            FirstName = people[0].firstname,
                                            SecoundName = people[0].lastname
                                        };
                                        VariableControlService.TeamScore.player.Add(player);

                                    }
                                    Console.WriteLine($"POST request successful. Response: {people[0].firstname} {people[0].lastname}");
                                    Console.WriteLine(responseContent);
                                }
                                else
                                {
                                    Console.WriteLine($"POST request failed. Status Code: {response.StatusCode}");
                                }
                            }







                        }
                    }
                    else
                    {
                        //TestTest++;
                        //if (TestTest > 20)
                        //{
                        //    if(Teams.player.Count < 5)
                        //    Teams.player.Add($"Ahmad {TestTest}");
                        //    TestTest = 0;
                        //}
                    }
                }
                else
                {

                    Console.WriteLine("Waitting .. ");
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

class Person
{
    public string firstname { get; set; }
    public string lastname { get; set; }
}

