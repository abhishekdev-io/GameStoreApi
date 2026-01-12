namespace GameStore.Api.Dtos;

public record class GameSummaryDto(
    int Id,
    string Name, 
    string Genre,
    string Description,
    decimal Price,
    DateTime ReleaseDate
    );
