using System.ComponentModel.DataAnnotations;
using TicTacToe.Enums;
using TicTacToe.Algorithm;
using TicTacToe.Extensions;

namespace TicTacToe.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        [Required]
        public List<List<Cell>> Board { get; set; }
        public int Size { get; set; }
        public Guid PlayerOneId { get; set; }
        public Guid PlayerTwoId { get; set; }
        public Guid CurrentPlayerId { get; set; }
        public GameStatus Status { get; set; }
        public int StepCount  { get; set; }
        
        private static List<List<Cell>> InitializeBoard(int Size)
        {
            var board = new List<List<Cell>>();
            for (int i = 0; i < Size; i++)
            {
                var row = new List<Cell>();
                for (int j = 0; j < Size; j++)
                {
                    row.Add(Cell.Empty);
                }
                board.Add(row);
            }
            
            return board;
        }

        private static Game InitializeGame(Guid playerOneId, Guid playerTwoId, int size)
        {
            return new Game
            {
                Id = Guid.NewGuid(),
                Board = InitializeBoard(size),
                Size = size,
                PlayerOneId = playerOneId,
                PlayerTwoId = playerTwoId,
                CurrentPlayerId = playerOneId,
                Status = GameStatus.NotStarted,
                StepCount = 0
            };
        }

        public static Game StartGame(Guid playerOneId, Guid playerTwoId, int size)
        {
            return InitializeGame(playerOneId, playerTwoId, size);
        }

        public void Move( Guid playerId, int row, int column)
        {
            if (Status == GameStatus.NotStarted)
            {
                Status = GameStatus.InProgress;
            }
            
            if (Status == GameStatus.Finished)
            {
                throw new ApiException.GameAlreadyCompletedException(Id);
            }

            if (Board[row][column] != Cell.Empty)
            {
                return;
            }

            if (Size < row || column > Size)
            {
                throw new ApiException.InvalidCellException(row, column, Size);
            }

            if (playerId != CurrentPlayerId)
            {
                throw new ApiException.WrongPlayerTurnException(playerId);
            }

            if (PlayerOneId != playerId && PlayerTwoId != playerId)
            {
                throw new ApiException.PlayerNotFoundException(playerId);
            }
            
            
            
            Random random = new Random();

            if (PlayerOneId == playerId)
            {
                if (StepCount % 3 == 0 && random.NextDouble() < 0.1)
                {
                    Board[row][column] = Cell.O;
                }
                else
                {
                    Board[row][column] = Cell.X;
                }
            }else if (PlayerTwoId == playerId)
            {
                if (StepCount % 3 == 0 && random.NextDouble() < 0.1)
                {
                    Board[row][column] = Cell.X;
                }
                else
                {
                    Board[row][column] = Cell.O;
                }
            }
            
            CurrentPlayerId = PlayerOneId == playerId ? PlayerTwoId : PlayerOneId;

            if (GameAlgorithm.VerticalCheck(Board, Size) ||
                GameAlgorithm.HorizontalCheck(Board, Size) ||
                GameAlgorithm.CrossCheck(Board, Size))
            {
                Status = GameStatus.Finished;
            }
            
            if (StepCount == 0)
            {
                Status = GameStatus.InProgress;
            }
            StepCount++;

            if (StepCount >= (Size * Size))
            {
                Status = GameStatus.Finished;
            }
            
            
            
            return;
        }
    }
}