using FortRoom.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FortRoom.Controllers
{
    //[EnableCors("AllowAll")]
    [Route("api/[controller]")]
    [ApiController]
    public class FortRoomController : ControllerBase
    {
        [HttpGet("IsOccupied")]
        public IActionResult Get()
        {
            return Ok(VariableControlService.IsOccupied);
        }
        [HttpGet("SetAsOccupied")]
        public IActionResult SetAsOccupied(bool IsOccupied)
        {
            VariableControlService.IsOccupied = IsOccupied;
            return Ok(VariableControlService.IsOccupied);
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
            Console.WriteLine("Recived ..");
            VariableControlService.TeamScore = TeamScore;
            return Ok();
        }
    }
}
