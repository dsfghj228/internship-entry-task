using System;
using System.Linq;
using TicTacToe.Enums;
using TicTacToe.Models;
using Xunit;

namespace TicTacToe.UnitTests
{
    public class GameTests
    {
        [Fact]
        public void StartGame_ShouldInitializeGameWithCorrectValues()
        {
            var playerOneId = Guid.NewGuid();
            var playerTwoId = Guid.NewGuid();
            int size = 3;

            var game = Game.StartGame(playerOneId, playerTwoId, size);

            Assert.Equal(playerOneId, game.PlayerOneId);
            Assert.Equal(playerTwoId, game.PlayerTwoId);
            Assert.Equal(size, game.Size);
            Assert.Equal(playerOneId, game.CurrentPlayerId);
            Assert.Equal(GameStatus.NotStarted, game.Status);
            Assert.Equal(0, game.StepCount);
            Assert.All(game.Board.SelectMany(row => row), cell => Assert.Equal(Cell.Empty, cell));
        }

        [Fact]
        public void Move_ShouldChangeBoardAndSwitchPlayer()
        {
            var playerOneId = Guid.NewGuid();
            var playerTwoId = Guid.NewGuid();
            int size = 3;
            var game = Game.StartGame(playerOneId, playerTwoId, size);
            var oldCurrentPlayerId = game.CurrentPlayerId;
            
            game.Move(oldCurrentPlayerId, 0,0);

            Assert.NotEqual(game.CurrentPlayerId, oldCurrentPlayerId);
            Assert.NotEqual(Cell.Empty, game.Board[0][0]);
            Assert.Equal(1, game.StepCount);
            Assert.Equal(GameStatus.InProgress, game.Status);
        }
    }
}