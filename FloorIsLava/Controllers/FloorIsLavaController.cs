using FloorIsLava.Services;
using Library;
using Library.APIIntegration;
using Library.LocalStorage;
using Library.Model;
using Library.OSControl;
using Library.RGBLib;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FloorIsLava.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FloorIsLavaController : ControllerBase
    {

        private readonly ILogger<FloorIsLavaController> _logger;
        public FloorIsLavaController(ILogger<FloorIsLavaController> logger)
        {
            _logger = logger;
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
            _logger.LogTrace("Received ..");
            VariableControlService.TeamScore = TeamScore;
            VariableControlService.IsOccupied = true;
            VariableControlService.GameStatus = GameStatus.NotStarted;
            LocalStorage.SaveData(VariableControlService.TeamScore, "data.json");

            return Ok();
        }
        [HttpGet("ReturnScore")]
        public IActionResult ReturnScore()
        {
            VariableControlService.TeamScore.Total =
                VariableControlService.TeamScore.FortRoomScore +
                VariableControlService.TeamScore.ShootingRoomScore +
                VariableControlService.TeamScore.DivingRoomScore +
                VariableControlService.TeamScore.DarkRoomScore +
                VariableControlService.TeamScore.FloorIsLavaRoomScore;

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

        [HttpGet("DoorControl")]
        public IActionResult DoorControl(DoorStatus doorStatus)
        {
            VariableControlService.NewDoorStatus = doorStatus;
            return Ok(doorStatus);
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
                Score = VariableControlService.TeamScore.FloorIsLavaRoomScore,
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


        [HttpGet("GetScore")]
        public IActionResult GetScore()
        {
            return Ok(VariableControlService.TeamScore.FloorIsLavaRoomScore);
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
        [HttpGet("Test")]
        public async Task <IActionResult> Test()
        {
            Team newTeam = new Team();
            newTeam.Name = "TestName";
            newTeam.Total = 100;
            var player = new Player();
            player.Id = "12";
            player.FirstName = "Test";
            player.LastName = "Test@";
            player.MobileNumber= "0795282626";
            newTeam.player.Add(player);
            var result =await APIIntegration.GetSignature("https://admin.frenziworld.com/api/make-signature", newTeam);
            await APIIntegration.SendScore("https://admin.frenziworld.com/api/game-score", result.Item1 ,result.Item2);


            return Ok(result);
        }
    }
}
