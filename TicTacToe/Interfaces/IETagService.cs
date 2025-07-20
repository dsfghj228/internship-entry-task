using TicTacToe.Models;

namespace TicTacToe.Interfaces
{
    public interface IETagService
    {
        string GenerateETag(Game game);
    }
}

