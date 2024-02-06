using FortRoom.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FortRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortRoomController : ControllerBase
    {
        [HttpGet(Name = "IsOccupied")]
        public IActionResult Get()
        {
            //return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            //{
            //    Date = DateTime.Now.AddDays(index),
            //    TemperatureC = Random.Shared.Next(-20, 55),
            //    Summary = Summaries[Random.Shared.Next(Summaries.Length)]
            //})
            //.ToArray();

            return Ok("");
        }
        [HttpPost("StartStopGame")]
        public IActionResult StartGame(bool startGame)
        {
            VariableControlService.IsTheGameStarted = startGame;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpPost("ReceiveScore")]
        public IActionResult ReceiveScore(Team TeamScore)
        {
            VariableControlService.TeamScore = TeamScore;
            return Ok();
        }
    }
}
