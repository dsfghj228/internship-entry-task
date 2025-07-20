using Microsoft.AspNetCore.Mvc;
using TicTacToe.Interfaces;
using TicTacToe.Models;
using TicTacToe.Services;

namespace TicTacToe.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private static Game _currentGame;
        private readonly IConfiguration _config;
        private readonly IGameRepository _gameRepository;
        private readonly ETagService  _tagService;

        public GameController(IConfiguration config, IGameRepository gameRepository, ETagService tagService)
        {
            _config = config;
            _gameRepository = gameRepository;
            _tagService = tagService;
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateGame(Guid playerOneId, Guid playerTwoId)
        {
            try
            {
                var size = _config.GetValue<int>("AppSettings:Size");
                var gameForCreate = Game.StartGame(playerOneId, playerTwoId, size);
                var newGame = await _gameRepository.CreateGameAsync(gameForCreate);
                
                var etag = _tagService.GenerateETag(newGame);
    
                Response.Headers.ETag = etag;
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
                
                var etag = _tagService.GenerateETag(game);
    
                Response.Headers.ETag = etag;
                
                return Ok(game);
            }
            catch (Exception e)
            {
                return StatusCode(500, "Internal server error: " + e.Message);
            }
        }

        [HttpPost("move/{gameId}")]
        public async Task<IActionResult> Move(Guid gameId, int row, int column, [FromHeader(Name = "If-Match")] string ifMatchHeader)
        {
            try
            {
                // var curPlayer = _currentGame.CurrentPlayerId;
                // _currentGame = Game.Move(_currentGame, curPlayer, row, column);
                // return Ok(_currentGame);

                var game = await _gameRepository.GetByIdAsync(gameId);
                if (game == null)
                {
                    return NotFound();
                }
                
                var currentEtag = _tagService.GenerateETag(game);

                if (!string.IsNullOrEmpty(ifMatchHeader) && ifMatchHeader != currentEtag)
                {
                    return Ok(game);
                }

                var curPlayer = game.CurrentPlayerId;
                var updatedGame = Game.Move(game, curPlayer, row, column);
                
                var newEtag = _tagService.GenerateETag(updatedGame);
                
                await _gameRepository.MoveAsync(updatedGame);
                
                Response.Headers.ETag = newEtag;
                return Ok(updatedGame);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
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

