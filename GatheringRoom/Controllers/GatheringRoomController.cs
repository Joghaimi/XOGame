using GatheringRoom.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Security.Cryptography.Xml;
using System.Text;

namespace GatheringRoom.Controllers
{
    //[EnableCors("AllowAll")]
    [ApiController]
    [Route("[controller]")]
    public class GatheringRoomController : ControllerBase
    {
        private readonly ILogger<GatheringRoomController> _logger;
        public GatheringRoomController(ILogger<GatheringRoomController> logger)
        {
            _logger = logger;
        }

        [HttpPost("StartStopGame")]
        public IActionResult StartGame(bool startGame)
        {
            VariableControlService.IsTheGameStarted = startGame;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpPost("TeamNaming")]
        public IActionResult TeamNaming(string TeamName)
        {
            VariableControlService.TeamScore.Name = TeamName;
            return Ok(VariableControlService.TeamScore.Name);
        }
        [HttpGet("GoToTheNextRoom")]
        public async Task<IActionResult> NextRoom()
        {
            Console.WriteLine("Request Next Room");

            string jsonData = JsonConvert.SerializeObject(VariableControlService.TeamScore);
            using var client = new HttpClient();
            var request = new HttpRequestMessage
            {
                Method = HttpMethod.Post,
                RequestUri = new Uri(VariableControlService.NextRoomURL),
                Content = new StringContent(jsonData, Encoding.UTF8, "application/json")
            };
            Console.WriteLine("Request Next Room Sent .. ");

            HttpResponseMessage response = await client.SendAsync(request);
            if (response.IsSuccessStatusCode)
            {
                // Read the response content as a string
                string responseContent = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseContent);
            }
            else
            {
                Console.WriteLine($"POST request failed. Status Code: {response.StatusCode}");
            }

            // Empty the Variable 
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.TeamScore.Name = "";
            VariableControlService.TeamScore.player.Clear();
            VariableControlService.EnableGoingToTheNextRoom = true;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpGet("getThePlayers")]
        public IActionResult Get()
        {
            Console.WriteLine("Ahmad SAid ...");
            return base.Ok(VariableControlService.TeamScore.player);
        }



    }
}