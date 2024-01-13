using Microsoft.AspNetCore.Mvc;

namespace GatheringRoom.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class Teams : ControllerBase
    {
        private readonly ILogger<Teams> _logger;
        public Teams(ILogger<Teams> logger)
        {
            _logger = logger;
        }

        [HttpGet(Name = "getThePlayers")]
        public IActionResult Get()
        {
            return base.Ok(GatheringRoom.Teams.player);
        }
    }
}