using CatchyGame.Service;
using Library;
using Library.LocalStorage;
using Library.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CatchyGame.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CatchyController : ControllerBase
    {
        [HttpGet("RoomStatus")]
        public IActionResult GetRoomStatus()
        {
            return Ok(VariableControlService.GameStatus.ToString());
        }
        [HttpGet("GetScore")]
        public IActionResult GetScore()
        {
            return Ok(VariableControlService.Team);
        }

        [HttpGet("TopScore")]
        public IActionResult TopScore()
        {
            return Ok(VariableControlService.TopScore);
        }
        [HttpPost("ResetTopScore")]
        public IActionResult ResetTopScore(int newTopScore)
        {
            VariableControlService.TopScore = newTopScore;

            LocalStorage.SaveData($"{VariableControlService.TopScoreTeam} {VariableControlService.TopScore}", "data.json");
            //LocalStorage.SaveData(VariableControlService.TopScore, "data.json");
            return Ok($"{VariableControlService.TopScoreTeam} {VariableControlService.TopScore}");
        }

        [HttpGet("CurrentTime")]
        public IActionResult CurrentTime()
        {
            var totalTime = VariableControlService.GameTiming - VariableControlService.CurrentTime;
            totalTime = totalTime / 1000;
            return Ok(totalTime < 0 ? 0 : totalTime);
        }

        [HttpPost("RoomStatus")]
        public IActionResult ReturnRoomStatus(GameStatus gameStatus)
        {
            VariableControlService.GameStatus = gameStatus;
            return Ok(VariableControlService.GameStatus);
        }
        [HttpPost("ReceiveScore")]
        public IActionResult ReceiveScore(CatchyTeam TeamScore)
        {
            VariableControlService.Team = TeamScore;
            VariableControlService.GameStatus = GameStatus.Started;
            return Ok();
        }
    }
}
