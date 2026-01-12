using GameStore.Api.Dtos;

namespace GameStore.Api.Interfaces
{
    public interface IGenresServices
    {
        Task<List<GenreDto>> GetGenresAsync();
        Task<GenreDto> GetGenreByIdAsync(int genreId);
        Task<List<GameSummaryDto>> GetGamesByGenre(int genreId);
        Task<int> CreateGenreAsync(GenreCreateDto createGenre);
        Task UpdateGenreAsync(int genreId, UpdateGenreDto updateGenre);
        Task DeleteGenreAsync(int genreId);
    }
}
