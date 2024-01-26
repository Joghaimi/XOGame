using GatheringRoom.Services;
using Microsoft.AspNetCore.Mvc;

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
            VariableControlService.IsTheGameStarted = startGame;
            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpPost("TeamNaming")]
        public IActionResult TeamNaming(string TeamName)
        {
            GatheringRoom.Teams.Name = TeamName;
            return Ok(GatheringRoom.Teams.Name);
        }
        [HttpPost("GoToTheNextRoom")]
        public IActionResult NextRoom()
        {
            // Send Command To The Next Room // TO DO 

            // Empty the Variable 
            VariableControlService.IsTheGameStarted = false;
            GatheringRoom.Teams.Name = "";
            GatheringRoom.Teams.player = new List<string>();

            return Ok(VariableControlService.IsTheGameStarted);
        }
        [HttpGet(Name = "getThePlayers")]
        public IActionResult Get()
        {
            return base.Ok(GatheringRoom.Teams.player);
        }



    }
}