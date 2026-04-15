namespace GameStore.Dtos.GameDtos;

public record class GameDetailsDto(
    int Id,
    string Name, 
    int GenreId, 
    decimal Price,
    string Description,
    DateTime ReleaseDate
    );
