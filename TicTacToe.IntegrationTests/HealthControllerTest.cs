using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TicTacToe.IntegrationTests
{
    public class HealthControllerTests : IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly WebApplicationFactory<Program> _factory;

        public HealthControllerTests(WebApplicationFactory<Program> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task HealthEndpoint_ReturnsOk()
        {
            var client = _factory.CreateClient();
            var response = await client.GetAsync("api/Health");
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}