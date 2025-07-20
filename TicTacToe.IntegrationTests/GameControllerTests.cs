using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TicTacToe.IntegrationTests
{
    public class GameControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly HttpClient _client;

        public GameControllerTests(WebApplicationFactory<Program> factory)
        {
            _client = factory.CreateClient();
        }

        [Fact]
        public async Task CreateGame_ReturnsOkAndGameId()
        {
            var request = new
            {
                playerOneId = System.Guid.NewGuid(),
                playerTwoId = System.Guid.NewGuid(),
                size = 3
            };

            var response = await _client.PostAsJsonAsync("api/Game/create", request);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);

            var content = await response.Content.ReadAsStringAsync();
            Assert.Contains("id", content.ToLower());
        }

        [Fact]
        public async Task Move_ReturnsOk_WhenMoveIsValid()
        {
            var createRequest = new
            {
                playerOneId = System.Guid.NewGuid(),
                playerTwoId = System.Guid.NewGuid(),
                size = 3
            };
            var createResponse = await _client.PostAsJsonAsync("api/Game/create", createRequest);
            var game = await createResponse.Content.ReadFromJsonAsync<System.Text.Json.JsonElement>();
            string gameId = game.GetProperty("id").GetString();
            string playerId = createRequest.playerOneId.ToString();

            // Получаем ETag через GET
            var getResponse = await _client.GetAsync($"api/Game/{gameId}");
            getResponse.EnsureSuccessStatusCode();
            var etag = getResponse.Headers.ETag?.Tag;

            var moveRequest = new
            {
                playerId = playerId,
                row = 0,
                column = 0
            };
            var request = new HttpRequestMessage(HttpMethod.Post, $"api/Game/move/{gameId}")
            {
                Content = JsonContent.Create(moveRequest)
            };
            if (etag != null)
                request.Headers.TryAddWithoutValidation("If-Match", etag);

            var moveResponse = await _client.SendAsync(request);
            Assert.Equal(HttpStatusCode.OK, moveResponse.StatusCode);
        }

        [Fact]
        public async Task GetGame_ReturnsOkAndGameState()
        {
            var createRequest = new
            {
                playerOneId = System.Guid.NewGuid(),
                playerTwoId = System.Guid.NewGuid(),
                size = 3
            };
            var createResponse = await _client.PostAsJsonAsync("api/Game/create", createRequest);
            var game = await createResponse.Content.ReadFromJsonAsync<dynamic>();
            string gameId = game.GetProperty("id").GetString();

            var getResponse = await _client.GetAsync($"api/Game/{gameId}");
            Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);

            var content = await getResponse.Content.ReadAsStringAsync();
            Assert.Contains("board", content.ToLower());
        }
    }
}