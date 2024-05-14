using FortRoom.Services;
using Library;
using Library.LocalStorage;
using Library.Modbus;
using Library.Model;
using Library.OSControl;
using Library.RGBLib;
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
            return Ok(VariableControlService.GameStatus != GameStatus.Empty);
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
            LocalStorage.SaveData(VariableControlService.TeamScore, "data.json");
            _logger.LogInformation("New Team Enter : {0} and Current Game Status is {1}" , VariableControlService.TeamScore.Name , VariableControlService.GameStatus);
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

        [HttpGet("Obstacle")]
        public IActionResult Obstacle(bool status)
        {
            if (status)
                ObstructionLib.Start();
            else
                ObstructionLib.Stop();
            return Ok();
        }

        [HttpGet("RestartService")]
        public IActionResult RestartService()
        {
            OSLib.ResetService("xogame.service");
            return Ok();
        }
        [HttpGet("RetrieveData")]
        public IActionResult RetrieveData()
        {
            var loadedData = LocalStorage.LoadData<Team>("data.json");
            if (loadedData != null)
                VariableControlService.TeamScore = loadedData;

            Console.WriteLine(loadedData);
            return Ok(loadedData);
        }



    }
}
