using FakeItEasy;
using FluentAssertions;
using GameStore.Controllers;
using GameStore.Dtos.GameDtos;
using GameStore.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.UnitTest.Controllers
{
    public class GamesControllerTests
    {
        private readonly IGameServices gameServices;

        public GamesControllerTests()
        {
            gameServices = A.Fake<IGameServices>();

        }

        [Fact]
        public async Task GamesController_GetGames_ReturnOk()
        {
            //Arrange
            var gamesList = A.Fake<List<GameSummaryDto>>();

            A.CallTo(() => gameServices.GetGamesAsync()).Returns(Task.FromResult(gamesList));

            var controller = new GamesController(gameServices);

            //Act
            var result = await controller.GetGames();

            //Assert 
            result.Result.Should().NotBeNull();
            result.Result.Should().BeOfType(typeof(OkObjectResult));
            
        }


        
        [Fact]
        public async Task GamesController_CreateGame_ReturnObject()
        {
            //Arrange
            var newGameDto = A.Fake<CreateGameDto>();
            int expectedId = 1;

            A.CallTo(() => gameServices.CreateGameAsync(newGameDto)).Returns(Task.FromResult(expectedId));

            var controller = new GamesController(gameServices);

            //Act
            var result = await controller.CreateGame(newGameDto);

            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType<CreatedAtActionResult>();

            //Check if the ID in the "Address" is actually 1
            var createdResult = result as CreatedAtActionResult;
            createdResult?.RouteValues?["gameId"].Should().Be(expectedId);
        }
    }
}


