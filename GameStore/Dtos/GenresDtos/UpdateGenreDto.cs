using System.ComponentModel.DataAnnotations;

namespace GameStore.Dtos.GenresDtos;
    public record UpdateGenreDto
    (
      [Required][StringLength(50)] string Name
    );
    
