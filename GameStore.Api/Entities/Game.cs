using System;

namespace GameStore.Api.Entities;

public class Game
{
    public int Id {get; set;}

    public required string Name {get; set;} 

    public int GenreId { get; set; }

    public Genre Genre { get; set; }

    public decimal Price {get; set;}

    public string Description { get; set; } = string.Empty;

    public DateTime ReleaseDate {get; set;}

}
