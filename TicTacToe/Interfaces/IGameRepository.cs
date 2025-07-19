using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
    public interface IGameRepository
    {
        Task<Game> GetByIdAsync(Guid id);
        Task<Game> CreateGameAsync(Game game);
        Task<bool> DeleteGameAsync(Guid id);
    }
}

