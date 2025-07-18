using System.ComponentModel.DataAnnotations;
using TicTacToe.Enums;

namespace TicTacToe.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        [Required]
        public Board Board { get; set; }
        public Guid PlayerOneId { get; set; }
        public Guid PlayerTwoId { get; set; }
        public GameStatus Status { get; set; }
        public int StepCount  { get; set; }
        
        private Board InitializeBoard(int Size)
        {
            var board = new Board();
            board.Cells = new List<List<Cell>>();
            for (int i = 0; i < Size; i++)
            {
                var row = new List<Cell>();
                for (int j = 0; j < Size; j++)
                {
                    row.Add(Cell.Empty);
                }
                board.Cells.Add(row);
            }
            
            return board;
        }

        private Game InitializeGame(Guid PlayerOneId, Guid PlayerTwoId)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                Board = InitializeBoard(5),
                PlayerOneId = PlayerOneId,
                PlayerTwoId = PlayerTwoId,
                Status = GameStatus.NotStarted,
                StepCount = 0
            };
        }

        public Game StartGame(Guid PlayerOneId, Guid PlayerTwoId)
        {
            return InitializeGame(PlayerOneId, PlayerTwoId);
        }
    }
}