using Microsoft.AspNetCore.Mvc;
using TicTacToe.Extensions;
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
            
            var size = _config.GetValue<int>("AppSettings:Size");
            var gameForCreate = Game.StartGame(playerOneId, playerTwoId, size);
            var newGame = await _gameRepository.CreateGameAsync(gameForCreate);
                
            var etag = _tagService.GenerateETag(newGame);
    
            Response.Headers.ETag = etag;
            return Ok(newGame);
        }

        [HttpGet("{gameId}")]
        public async Task<IActionResult> GetGame(Guid gameId)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new ApiException.GameNotFoundException(gameId);
            }
                
            var etag = _tagService.GenerateETag(game);
    
            Response.Headers.ETag = etag;
                
            return Ok(game);
        }

        [HttpPost("move/{gameId}")]
        public async Task<IActionResult> Move(Guid gameId, int row, int column, [FromHeader(Name = "If-Match")] string ifMatchHeader)
        {
            var game = await _gameRepository.GetByIdAsync(gameId);
            if (game == null)
            {
                throw new ApiException.GameNotFoundException(gameId);
            }
            
            if (row >= game.Size || column >= game.Size)
            {
                throw new ApiException.InvalidCellException(row, column, game.Size);
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

        [HttpDelete("{gameId}")]
        public async Task<IActionResult> DeleteGame(Guid gameId)
        {
            var res = await _gameRepository.DeleteGameAsync(gameId);
            if (!res)
            {
                throw new ApiException.GameNotFoundException(gameId);
            }
            return Ok();
        }
        
    }
}

