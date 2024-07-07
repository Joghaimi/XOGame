using CatchyGame.Data;
using CatchyGame.Repository;
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
        public ApplicationDbContext DbContext;
        public readonly IScoreRepository _scoreRepo;
        public CatchyController(ApplicationDbContext _dbContext, IScoreRepository scoreRepo)
        {
            DbContext = _dbContext;
            _scoreRepo = scoreRepo;
        }
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
            return Ok(VariableControlService.topScore);
        }
        [HttpPost("ResetTopScore")]
        public IActionResult ResetTopScore(TopScore topScore)
        {
            VariableControlService.topScore = topScore;
            LocalStorage.SaveData(VariableControlService.topScore, "data.json");
            return Ok(VariableControlService.topScore);
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
        [HttpPost("GameMode")]
        public IActionResult GameMode(GameMode mode)
        {
            Console.WriteLine(mode);
            VariableControlService.GameMode = mode;
            if (VariableControlService.GameMode == Library.GameMode.inWar)
                VariableControlService.GameTiming = 180000;
            else
                VariableControlService.GameTiming = 120000;

            return Ok();
        }
        [HttpPost("Test SaveScore")]
        public IActionResult SaveScoreTest() {

            var score = new Score();
            //score.Id = 1; 
            score.TimeStamp =  DateTime.Now.AddDays(-7);
            score.TeamName = "Test 3";
            score.TeamScore = 30;
            var score2 = new Score();
            //score.Id = 1; 
            score2.TimeStamp = DateTime.Now.AddDays(-30);
            score2.TeamName = "Test 5";
            score2.TeamScore = 50;
            _scoreRepo.SaveScore(score2);
            return Ok(_scoreRepo.SaveScore(score));
        }
        [HttpGet("GetSaveScoreTest")]
        public IActionResult GetSaveScoreTest()
        {
            var scores = DbContext.Score.ToList();
            return Ok( scores );
        }
        [HttpGet("GetTodayTopScore")]
        public IActionResult GetTodayTopScore()
        {
            return Ok(_scoreRepo.GetTopScoreThisDay());
        }
        [HttpGet("GetThisWeekTopScore")]
        public IActionResult GetThisWeekTopScore()
        {
            return Ok(_scoreRepo.GetTopScoreThisWeek());
        }
        [HttpGet("GetThisMonthTopScore")]
        public IActionResult GetThisMonthTopScore()
        {
            return Ok(_scoreRepo.GetTopScoreThisMonth());
        }

    }
}
