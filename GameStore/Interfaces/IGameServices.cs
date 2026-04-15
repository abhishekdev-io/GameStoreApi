using GameStore.Dtos.GameDtos;

namespace GameStore.Interfaces
{
    public interface IGameServices
    {
        Task<List<GameSummaryDto>> GetGamesAsync();
        Task<GameDetailsDto> GetGameByIdAsync(int gameId);
        Task<int> CreateGameAsync(CreateGameDto newGame);
        Task UpdateGameAsync(int gameId, UpdateGameDto updateGame);
        Task DeleteGameAsync(int gameId);

    }
}