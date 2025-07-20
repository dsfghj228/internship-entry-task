using System.Net;
using Microsoft.AspNetCore.Http;

namespace TicTacToe.Extensions
{
    public abstract class ApiException : Exception
    {
        public HttpStatusCode StatusCode { get; }
        public string Type { get; }
        public string Title { get; }

        protected ApiException(
            HttpStatusCode statusCode,
            string type,
            string title,
            string message) : base(message)
        {
            StatusCode = statusCode;
            Type = type;
            Title = title;
        }
        
        public class GameNotFoundException : ApiException
        {
            public GameNotFoundException(Guid gameId)
                : base(
                    HttpStatusCode.NotFound,
                    "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    "Игра не найдена",
                    $"Игра с таким id {gameId} не найдена")
            {
            }
        }
        
        public class PlayerNotFoundException : ApiException
        {
            public PlayerNotFoundException(Guid playerId)
                : base(
                    HttpStatusCode.NotFound,
                    "https://tools.ietf.org/html/rfc7231#section-6.5.4",
                    "Игрок не найден",
                    $"Игрок с таким id {playerId} не найден")
            {
            }
        }
        
        public class GameAlreadyCompletedException : ApiException
        {
            public GameAlreadyCompletedException(Guid gameId)
                : base(
                    HttpStatusCode.Conflict,
                    "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    "Игра уже закончилась",
                    $"Игра с таким id {gameId} уже закончилась")
            {
            }
        }
        
        public class ValidationException : ApiException
        {
            public IDictionary<string, string[]> Errors { get; }

            public ValidationException(IDictionary<string, string[]> errors)
                : base(
                    HttpStatusCode.UnprocessableEntity,
                    "https://tools.ietf.org/html/rfc4918#section-11.2",
                    "Validation error",
                    "One or more validation errors occurred")
            {
                Errors = errors;
            }
        }
        
        public class InvalidCellException : ApiException
        {
            public InvalidCellException(int row, int column, int size)
                : base(
                    HttpStatusCode.BadRequest,
                    "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                    "Invalid cell",
                    $"Ячейки с индексами ({row}, {column}) не существует. Максимальный индекс: {size * size}")
            {
            }
        }

        public class WrongPlayerTurnException : ApiException
        {
            public WrongPlayerTurnException(Guid currentPlayerId)
                : base(
                    HttpStatusCode.Conflict,
                    "https://tools.ietf.org/html/rfc7231#section-6.5.8",
                    "Wrong player turn",
                    $"Сейчас очередь игрока с ID {currentPlayerId}")
            {
            }
        }
    }
}

