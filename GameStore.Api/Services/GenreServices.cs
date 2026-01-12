using AutoMapper;
using AutoMapper.QueryableExtensions;
using GameStore.Api.Data;
using GameStore.Api.Dtos;
using GameStore.Api.Entities;
using GameStore.Api.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.Api.Services
{
    public class GenreServices : IGenresServices
    {
        private readonly GameStoreContext dbContext;
        private readonly IMapper mapper;

        public GenreServices(GameStoreContext dbContext, IMapper mapper)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
        }

        //GET

        public async Task<List<GenreDto>> GetGenresAsync()
        {
            return await dbContext.Genres
                           .AsNoTracking()
                           .ProjectTo<GenreDto>(mapper.ConfigurationProvider)
                           .ToListAsync();
        }

        public async Task<GenreDto> GetGenreByIdAsync(int genreId)
        {
            var genre = await dbContext.Genres
                           .AsNoTracking()
                           .Where(g => g.Id == genreId)
                           .ProjectTo<GenreDto>(mapper.ConfigurationProvider)
                           .FirstOrDefaultAsync();

            if (genre == null)
                throw new KeyNotFoundException($"Genre with Id {genreId} was not found");

            return genre;
        }

        public async Task<List<GameSummaryDto>> GetGamesByGenre(int genreId)
        {
            var genre = await GetGenreByIdAsync(genreId);

            return await dbContext.Games
                                  .Include(g => g.Genre)
                                  .Where(g => g.GenreId == genreId)
                                  .Select(g => new GameSummaryDto(
                                      g.Id,
                                      g.Name,
                                      g.Genre.Name,
                                      g.Description,
                                      g.Price,
                                      g.ReleaseDate))
                                  .ToListAsync();

        }

        //POST

        public async Task<int> CreateGenreAsync(GenreCreateDto newGenre)
        {
            if (newGenre == null)
                throw new ArgumentNullException(nameof(newGenre), "Genre cannot be null.");

            var genre = mapper.Map<Genre>(newGenre);

            await dbContext.Genres
                           .AddAsync(genre);

            await SaveAsync();

            return genre.Id;
        }

        //PUT

        public async Task UpdateGenreAsync(int genreId, UpdateGenreDto updateGenre)
        {

            var existingGenre = await dbContext.Genres
                                               .FindAsync(genreId);
            if (existingGenre == null)
                throw new KeyNotFoundException($"Genre with Id {genreId} was not found");            

            mapper.Map(updateGenre, existingGenre);

            await SaveAsync();
        }

        //DELETE

        public async Task DeleteGenreAsync(int genreId)
        {
            var genre = await dbContext.Genres
                                                .FindAsync(genreId);
            if (genre == null)
                throw new KeyNotFoundException($"Genre with Id {genreId} was not found");

            dbContext.Genres.Remove(genre);

            await SaveAsync();

        }



        //SaveChanges

        private async Task SaveAsync()
        {
            await dbContext.SaveChangesAsync();
        }

    }
}