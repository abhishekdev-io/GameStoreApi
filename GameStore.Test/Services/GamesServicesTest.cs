using AutoMapper;
using GameStore.Data;
using GameStore.Dtos.GameDtos;
using GameStore.Entities;
using GameStore.Mapper;
using GameStore.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;

namespace GameStore.UnitTest.Services
{
    public class GamesServicesTest
    {
        private async Task<GameStoreContext> GetDatabaseContext()
        {
            var options = new DbContextOptionsBuilder<GameStoreContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            var databaseContext = new GameStoreContext(options);
            databaseContext.Database.EnsureCreated();
            //seeding data
            if (await databaseContext.Games.CountAsync() <= 0)
            {
                for (int i = 1; i <= 5; i++)
                {
                    databaseContext.Games.Add(
                        new Game
                        {
                            Id = i,
                            Name = $"Game{i}",
                            GenreId = i,
                            Description = "Description",
                            ReleaseDate = new DateTime(2016, 2, 16),
                            Price = 59.99M
                        });
                    databaseContext.Genres.Add(
                        new Genre
                        { Id = i, Name = $"Genre{i}" });
                    
                }
                await databaseContext.SaveChangesAsync();
            }
            return databaseContext;
        }

        private IMapper GetMapper()
        {
            var configuration = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }, NullLoggerFactory.Instance);

            return configuration.CreateMapper();
        }

        [Fact]
        public async Task GetGameByIdAsync_ReturnsGameDetailsDto_WhenGameExists()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var mapper = GetMapper();
            var service = new GamesServices(dbContext, mapper);

            int gameId = 1;

            // Act
            var result = await service.GetGameByIdAsync(gameId);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(gameId, result.Id);
            Assert.Equal("Street Fighter V", result.Name);
            Assert.Equal("Classic fighting game", result.Description);
        }

        [Fact]
        public async Task GetGameByIdAsync_ThrowsKeyNotFoundException_WhenGameDoesNotExist()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var mapper = GetMapper();
            var service = new GamesServices(dbContext, mapper);

            int gameId = 999;

            // Act & Assert
            var exception = await Assert.ThrowsAsync<KeyNotFoundException>(() => service.GetGameByIdAsync(gameId));
            Assert.Equal($"Game with Id {gameId} was not found.", exception.Message);
        }

        [Fact]
        public async Task CreateGameAsync_ThrowsArgumentNullException_WhenNewGameIsNull()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var mapper = GetMapper();
            var service = new GamesServices(dbContext, mapper);

            CreateGameDto? newGame = null;

            // Act & Assert
            #pragma warning disable CS8604 // Possible null reference argument.
            var exception = await Assert.ThrowsAsync<ArgumentNullException>(() => service.CreateGameAsync(newGame));


            // Using Assert.Contains here because ArgumentNullException appends the parameter name to the message
            Assert.Contains("Game cannot be null.", exception.Message);
        }

        [Fact]
        public async Task CreateGameAsync_ReturnsGameId_WhenGameIsSuccessfullyCreated()
        {
            // Arrange
            var dbContext = await GetDatabaseContext();
            var mapper = GetMapper();
            var service = new GamesServices(dbContext, mapper);

            // Create a dummy DTO (adjust properties to match your actual CreateGameDto)
            var newGameDto = new CreateGameDto 
            (
                "Test",
                1,
                19M,
                DateTime.UtcNow,
                "Test"

            );

            // Act
            var newGameId = await service.CreateGameAsync(newGameDto);

            // Assert

            // 1. Prove the method returned a valid ID (assuming DB generates IDs greater than 0)
            Assert.True(newGameId > 0);

            // 2. Prove the game was actually saved to the database correctly
            var savedGame = await dbContext.Games.FindAsync(newGameId);

            Assert.NotNull(savedGame);
            Assert.Equal(newGameDto.Name, savedGame.Name);
        }
    }
}
