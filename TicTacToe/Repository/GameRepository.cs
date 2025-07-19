using Microsoft.EntityFrameworkCore;
using TicTacToe.Data;
using TicTacToe.Interfaces;
using TicTacToe.Models;

namespace TicTacToe.Repository
{
    public class GameRepository : IGameRepository
    {
        private readonly ApplicationDbContext _context;
        public GameRepository(ApplicationDbContext context)
        {
            _context = context;
        }
        
        public async Task<Game> GetByIdAsync(Guid id)
        {
            var game = await _context.Games.Where(g => g.Id == id).FirstOrDefaultAsync();
            return game;
        }

        public async Task<Game> CreateGameAsync(Game game)
        {
            await _context.Games.AddAsync(game);
            await _context.SaveChangesAsync();
            return game;
        }

        public async Task<bool> DeleteGameAsync(Guid id)
        {
            var game = await _context.Games.Where(g => g.Id == id).FirstOrDefaultAsync();
            if (game == null)
            {
                return false;
            }
            _context.Games.Remove(game);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}