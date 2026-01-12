using System.ComponentModel.DataAnnotations;

namespace GameStore.Api.Dtos;
    
  public record GenreCreateDto
  (
     [Required][StringLength(50)] string Name
  );
