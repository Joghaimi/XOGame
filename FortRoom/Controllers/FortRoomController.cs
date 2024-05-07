using FortRoom.Services;
using Library;
using Library.Model;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FortRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FortRoomController : ControllerBase
    {
        private readonly ILogger<FortRoomController> _logger;
        public FortRoomController(ILogger<FortRoomController> logger)
        {
            _logger = logger;
        }

        [HttpGet("IsOccupied")]
        public IActionResult Get()
        {
            //return Ok(VariableControlService.IsOccupied);
            return Ok(VariableControlService.GameStatus !=GameStatus.Empty);
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
        [HttpGet("RoomStatus")]
        public IActionResult GetRoomStatus()
        {
            return Ok(VariableControlService.GameStatus.ToString());
        }

        [HttpPost("RoomStatus")]
        public IActionResult ReturnRoomStatus(GameStatus gameStatus)
        {
            VariableControlService.GameStatus = gameStatus;
            return Ok(VariableControlService.GameStatus);
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
            VariableControlService.GameStatus = GameStatus.NotStarted;
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
            _logger.LogTrace("The Team Mover To the Next room , Reset this Room");
            VariableControlService.IsTheGameStarted = false;
            VariableControlService.TeamScore.Name = "";
            VariableControlService.TeamScore.player.Clear();
            VariableControlService.EnableGoingToTheNextRoom = true;
            return Ok(VariableControlService.IsTheGameStarted);
        }

        [HttpGet("GetScore")]
        public IActionResult GetScore()
        {
            return Ok(VariableControlService.TeamScore.FortRoomScore);
        }
        [HttpGet("IsGameStarted")]
        public IActionResult IsGameStarted()
        {
            return Ok(VariableControlService.GameStatus == GameStatus.Started);
            //return Ok(VariableControlService.IsTheGameStarted);
        }



        // Control Game 
        [HttpGet("DoorControl")]
        public IActionResult DoorControl(DoorStatus doorStatus)
        {
            VariableControlService.NewDoorStatus = doorStatus;
            return Ok(doorStatus);
        }

        [HttpGet("CurrentTime")]
        public IActionResult CurrentTime()
        {
            var totalTime = VariableControlService.RoomTiming - VariableControlService.CurrentTime;
            totalTime = totalTime / 1000;
            return Ok(totalTime < 0 ? 0 : totalTime);
        }

        [HttpGet("TimeAndStatus")]
        public ActionResult<(int, string)> GetTimeAndStatus()
        {
            var totalTime = (VariableControlService.RoomTiming - VariableControlService.CurrentTime) / 1000;
            totalTime = totalTime < 0 ? 0 : totalTime;
            (int, string) timeAndStatus = (totalTime, VariableControlService.GameStatus.ToString());
            Console.WriteLine(totalTime);
            Console.WriteLine(VariableControlService.GameStatus.ToString());
            Console.WriteLine(timeAndStatus);
            var result = new { Time = totalTime, Status = VariableControlService.GameStatus.ToString() };



            return Ok(result);
        }


    }
}
