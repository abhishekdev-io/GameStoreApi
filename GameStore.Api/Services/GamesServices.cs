using AutoMapper;
using AutoMapper.QueryableExtensions;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Services
{
    public class GamesServices : IGameServices
    {
        private readonly GameStoreContext dbContext;
        private readonly IMapper mapper;

        public GamesServices(GameStoreContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        //GET
        public async Task<List<GameSummaryDto>> GetGamesAsync()
        {
            return await dbContext.Games
                                  .AsNoTracking()
                                  .Select(g => new GameSummaryDto(
                                      g.Id,
                                      g.Name,
                                      g.Genre.Name,
                                      g.Description,
                                      g.Price,
                                      g.ReleaseDate))
                                  .ToListAsync();
        }

        public async Task<GameDetailsDto> GetGameByIdAsync(int gameId)
        {
            var game = await dbContext.Games
                                      .AsNoTracking()
                                      .Where(g => g.Id == gameId)
                                      .ProjectTo<GameDetailsDto>(mapper.ConfigurationProvider)
                                      .SingleOrDefaultAsync();
            if(game == null)
                throw new KeyNotFoundException($"Game with Id {gameId} was not found.");

            return game;
        }

        //POST

        public async Task<int> CreateGameAsync(CreateGameDto newGame)
        {
            if(newGame == null)
                throw new ArgumentNullException(nameof(newGame), "Game cannot be null.");

            var game = mapper.Map<Game>(newGame);

            await dbContext.Games
                           .AddAsync(game);

            await SaveAsync();

            return game.Id;

        }

        //PUT

        public async Task UpdateGameAsync(int gameId, UpdateGameDto updateGame)
        {

            var existingGame = await dbContext.Games
                                              .FindAsync(gameId);

            if(existingGame == null)
                throw new KeyNotFoundException($"Game with Id {gameId} was not found.");

            mapper.Map(updateGame, existingGame);

            await SaveAsync();

        }


        //DELETE

        public async Task DeleteGameAsync(int gameId)
        {
            var game = await dbContext.Games
                                      .FindAsync(gameId);

            if (game == null)
                throw new KeyNotFoundException($"Game with Id {gameId} was not found.");

            dbContext.Games.Remove(game);

            await SaveAsync();
        }



        //SaveChanges

        private async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }
    }
}
