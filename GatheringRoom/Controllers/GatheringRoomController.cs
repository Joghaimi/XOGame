using GatheringRoom.Services;
using Library;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net;
using System.Security.Cryptography.Xml;
using System.Text;

namespace GatheringRoom.Controllers
{
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
            _logger.LogTrace($"Game New Status :{startGame}");
            VariableControlService.IsTheGameStarted = startGame;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpPost("TeamNaming")]
        public IActionResult TeamNaming(string TeamName)
        {
            if (TeamName == "" || TeamName is null)
            {
                _logger.LogWarning($"Team Name cant be Empty or null");
                return BadRequest("Team Name can't be Empty or null");

            }
            _logger.LogTrace($"Team New Name :{TeamName}");
            VariableControlService.TeamScore.Name = TeamName;
            return Ok(VariableControlService.TeamScore.Name);
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
        [HttpGet("getThePlayers")]
        public IActionResult Get()
        {
            _logger.LogTrace("Get Player Team");
            return base.Ok(VariableControlService.TeamScore.player);
        }

        // Control Game 
        [HttpGet("DoorControl")]
        public IActionResult DoorControl(DoorStatus doorStatus)
        {
            VariableControlService.NewDoorStatus = doorStatus;
            return Ok(doorStatus);
        }
    }
}