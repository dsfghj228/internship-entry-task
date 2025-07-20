using TicTacToe.Models;
using System.Security.Cryptography;
using System.Text;

namespace TicTacToe.Services
{
    public class ETagService
    {
        public ETagService()
        {
            
        }

        public string GenerateETag(Game game)
        {
            var content = $"{game.Id}{game.Status}{game.StepCount}{string.Join("", game.Board.SelectMany(r => r))}";
    
            using var sha256 = SHA256.Create();
            var hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(content));
            return $"\"{Convert.ToBase64String(hashBytes)}\"";
        }
    }
}

