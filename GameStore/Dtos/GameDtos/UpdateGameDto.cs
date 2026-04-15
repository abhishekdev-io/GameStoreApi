using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos.GameDtos;


public record class UpdateGameDto(    
    [Required][StringLength(50)] string Name, 
    int GenreId, 
    [Range(1,100)] decimal Price,
    DateTime ReleaseDate,
    [StringLength(250)] string? Description
);