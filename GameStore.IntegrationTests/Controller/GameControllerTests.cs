using FluentAssertions;
using GameStore.Dtos.GameDtos;
using GameStore.IntegrationTests.WebAppFactory;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;


namespace GameStore.IntegrationTests.Controller
{
    //IClassFixture<> is an interface which says,Share that one instance with all the tests, and only destroy it when all tests are finished.
    public class GameControllerTests : IClassFixture<CustomWebApplicationFactory>
    {
        //HttpClient is a built-in .NET class used for sending HTTP requests
        private readonly HttpClient _client;

        public GameControllerTests(CustomWebApplicationFactory _factory)
        {
            _client = _factory.CreateClient();
        }

        [Fact]
        public async Task GamesController_GetGames_ReturnOk()
        {
            // Arrange
            //The CustomWebApplicationFactory has already spun up our entire application in memory.

            string endpointRoute = "api/games";

            // Act
            // We send a real HTTP GET request to the in-memory test server
            var response = await _client.GetAsync(endpointRoute);

            // Assert 
            // 1. Verify the HTTP Status Code is 200 OK
            response.StatusCode.Should().Be(HttpStatusCode.OK);

            var content = await response.Content.ReadAsStringAsync();

            var games = JsonSerializer.Deserialize<List<GameSummaryDto>>(
            content,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true }
            );

            games.Should().NotBeNullOrEmpty();
        }

        [Fact]
        public async Task GamesController_CreateGames_ReturnAction()
        {
            //Arrange
            CreateGameDto request = new("Test",1,23.29M,DateTime.UtcNow,"Test1");

            //Act
            var response = await _client.PostAsJsonAsync("/api/games",request);

            //Assert
            // 1. Verify the status code is specifically 201 Created
            response.StatusCode.Should().Be(HttpStatusCode.Created);

            // 2. Verify that CreatedAtAction successfully attached the Location header
            response.Headers.Location.Should().NotBeNull();


        }
    }
}
