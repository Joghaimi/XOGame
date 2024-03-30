using Library.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShootingRoom.Services;

namespace ShootingRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ShootingController : ControllerBase
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
            // To Be Removed and added to  Send Score To the Next Room ..
            if (IsOccupied)
            {
                VariableControlService.IsTheGameFinished = false;
            }
            return Ok(VariableControlService.IsOccupied);
        }

        [HttpPost("StartStopGame")]
        public IActionResult StartGame(bool startGame)
        {
            VariableControlService.IsTheGameStarted = startGame;
            VariableControlService.IsTheGameFinished = !startGame;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpPost("ReceiveScore")]
        public IActionResult ReceiveScore(Team TeamScore)
        {
            Console.WriteLine("Recived ..");
            VariableControlService.TeamScore = TeamScore;
            VariableControlService.IsOccupied = true;
            return Ok();
        }
        [HttpGet("ReturnScore")]
        public IActionResult ReturnScore()
        {
            return Ok(VariableControlService.TeamScore);
        }
        [HttpGet("GoToTheNextRoom")]
        public async Task<IActionResult> NextRoom()
        {
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.TeamScore.Name = "";
            VariableControlService.TeamScore.player.Clear();
            VariableControlService.EnableGoingToTheNextRoom = true;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpGet("GetScore")]
        public IActionResult GetScore()
        {
            return Ok(VariableControlService.TeamScore.ShootingRoomScore);
        }
        [HttpGet("GetRoundNumber")]
        public IActionResult GetRoundNumber()
        {
            var returnArray = new List<int>(){ (int)VariableControlService.GameRound, VariableControlService.LevelScore };
            return Ok(returnArray);
        }
    }
}
