using System.ComponentModel.DataAnnotations;
using TicTacToe.Enums;
using TicTacToe.Algorithm;

namespace TicTacToe.Models
{
    public class Game
    {
        public Guid Id { get; set; }
        [Required]
        public Board Board { get; set; }
        public int Size { get; set; }
        public Guid PlayerOneId { get; set; }
        public Guid PlayerTwoId { get; set; }
        public Guid CurrentPlayerId { get; set; }
        public GameStatus Status { get; set; }
        public int StepCount  { get; set; }
        
        private static Board InitializeBoard(int Size)
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

        public static Game Move(Game game, Guid playerId, int row, int column)
        {
            if (game.Status == GameStatus.NotStarted)
            {
                game.Status = GameStatus.InProgress;
            }
            
            if (game.Status == GameStatus.Finished)
            {
                throw new InvalidOperationException("Игра уже закончилась");
            }

            if (game.Board.Cells[row][column] != Cell.Empty)
            {
                throw new InvalidOperationException("Клетка уже занята");
            }

            if (game.Size < row || column > game.Size)
            {
                throw new InvalidOperationException("Ячейки с таким индексом не существует");
            }

            if (playerId != game.CurrentPlayerId)
            {
                throw new InvalidOperationException("Сейчас очередь другого игрока");
            }

            if (game.PlayerOneId != playerId && game.PlayerTwoId != playerId)
            {
                throw new InvalidOperationException("В данной игре нет игроков с таким Id");
            }
            
            
            
            Random random = new Random();

            if (game.PlayerOneId == playerId)
            {
                if (game.StepCount % 3 == 0 && random.NextDouble() < 0.1)
                {
                    game.Board.Cells[row][column] = Cell.O;
                }
                else
                {
                    game.Board.Cells[row][column] = Cell.X;
                }
            }else if (game.PlayerTwoId == playerId)
            {
                if (game.StepCount % 3 == 0 && random.NextDouble() < 0.1)
                {
                    game.Board.Cells[row][column] = Cell.X;
                }
                else
                {
                    game.Board.Cells[row][column] = Cell.O;
                }
            }
            
            game.CurrentPlayerId = game.PlayerOneId == playerId ? game.PlayerTwoId : game.PlayerOneId;

            if (GameAlgorithm.VerticalCheck(game.Board, game.Size) ||
                GameAlgorithm.HorizontalCheck(game.Board, game.Size) ||
                GameAlgorithm.CrossCheck(game.Board, game.Size))
            {
                game.Status = GameStatus.Finished;
            }
            
            if (game.StepCount == 0)
            {
                game.Status = GameStatus.InProgress;
            }
            game.StepCount++;

            if (game.StepCount >= (game.Size * game.Size))
            {
                game.Status = GameStatus.Finished;
            }
            
            
            
            return game;
        }
    }
}