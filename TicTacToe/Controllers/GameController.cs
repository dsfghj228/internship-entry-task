using Microsoft.AspNetCore.Mvc;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static Game _currentGame;
        private readonly IConfiguration _config;
        private readonly IGameRepository _gameRepository;

        public GameController(IConfiguration config, IGameRepository gameRepository)
        {
            _config = config;
            _gameRepository = gameRepository;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGame(Guid playerOneId, Guid playerTwoId)
        {
            try
            {
                var size = _config.GetValue<int>("AppSettings:Size");
                var gameForCreate = Game.StartGame(playerOneId, playerTwoId, size);
                var newGame = await _gameRepository.CreateGameAsync(gameForCreate);
                return Ok(newGame);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Internal server error: " + ex.Message);
            }
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetGame(Guid gameId)
        {
            try
            {
                var game = await _gameRepository.GetByIdAsync(gameId);
                if (game == null)
                {
                    return NotFound();
                }
                
                return Ok(game);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error: " + e.Message);
            }
        }

        [HttpPost("move")]
        public IActionResult Move(int row, int column)
        {
            var curPlayer = _currentGame.CurrentPlayerId;
            _currentGame = Game.Move(_currentGame, curPlayer, row, column);
            return Ok(_currentGame);
            
        }

        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGame(Guid gameId)
        {
            try
            {
                var res = await _gameRepository.DeleteGameAsync(gameId);
                if (!res)
                {
                    return NotFound();
                }
                return Ok();
                
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error: " + e.Message);
            }
        }
        
    }
}

