using DivingRoom.Services;
using Library;
using Library.Model;
using Library.RGBLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DivingRoom.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DivingController : ControllerBase
    {
        private readonly ILogger<DivingController> _logger;
        public DivingController(ILogger<DivingController> logger)
        {
            _logger = logger;
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
            return Ok(VariableControlService.TeamScore.DivingRoomScore);
        }
        // Control Game 
        [HttpGet("DoorControl")]
        public IActionResult DoorControl(DoorStatus doorStatus)
        {
            VariableControlService.NewDoorStatus = doorStatus;
            return Ok(doorStatus);
        }

        [HttpGet("RoomColor")]
        public IActionResult SetRoomColor(RGBColor rGB)
        {
            VariableControlService.DefaultColor = rGB;
            return Ok();
        }
        [HttpGet("CurrentTime")]
        public IActionResult CurrentTime()
        {
            var totalTime = VariableControlService.RoomTiming - VariableControlService.CurrentTime;
            totalTime = totalTime / 1000;
            return Ok(totalTime < 0 ? 0 : totalTime);
        }



        [HttpGet("TimeAndStatus")]
        public IActionResult GetTimeAndStatus()
        {
            var totalTime = (VariableControlService.RoomTiming - VariableControlService.CurrentTime) / 1000;
            totalTime = totalTime < 0 ? 0 : totalTime;
            var result = new { Time = totalTime, Status = VariableControlService.GameStatus.ToString() };
            return Ok(result);
        }

        [HttpGet("RoomInfo")]
        public IActionResult RoomInfo()
        {
            var result = new
            {
                TeamName = VariableControlService.TeamScore.Name,
                Score = VariableControlService.TeamScore.FortRoomScore,
                DoorStatus = VariableControlService.CurrentDoorStatus,
                Status = VariableControlService.GameStatus.ToString()
            };
            return Ok(result);
        }
        [HttpGet("RGBColor")]
        public IActionResult RGBColor(RGBColor newColor)
        {
            RGBLight.SetColor(newColor);
            return Ok();
        }

    }
}
