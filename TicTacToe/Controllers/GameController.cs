using Microsoft.AspNetCore.Mvc;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static Game _currentGame;
        private readonly IConfiguration _config;

        public GameController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost("create")]
        public IActionResult CreateGame(Guid playerOneId, Guid playerTwoId)
        {
            try
            {
                var size = _config.GetValue<int>("AppSettings:Size");
                _currentGame = Game.StartGame(playerOneId, playerTwoId, size);
            
                return Ok(_currentGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet]
        public IActionResult GetGame()
        {
            return Ok(_currentGame);
        }

        [HttpPost("move")]
        public IActionResult Move(int row, int column)
        {
            var curPlayer = _currentGame.CurrentPlayerId;
            _currentGame = Game.Move(_currentGame, curPlayer, row, column);
            return Ok(_currentGame);
            
        }

        [HttpDelete]
        public IActionResult DeleteGame()
        {
            _currentGame = null;
            return Ok();
        }
        
    }
}

