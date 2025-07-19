using Microsoft.AspNetCore.Mvc;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HealthController : ControllerBase
    {
        public HealthController()
        {
            
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok();
        }
    }
}
