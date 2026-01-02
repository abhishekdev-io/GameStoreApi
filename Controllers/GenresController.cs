using GameStore.Api.Dtos;
using GameStore.Api.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenresController : ControllerBase
    {
        private readonly IGenresServices genreServices;

        public GenresController(IGenresServices genreServices)
        {
            this.genreServices = genreServices;
        }

        //GET

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GenreDto>>> GetGenres()
        {
            var genres = await genreServices.GetGenresAsync();

            return Ok(genres);
        }

        [HttpGet("{genreId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GenreDto>> GetGenreById(int genreId)
        {
                var genre = await genreServices.GetGenreByIdAsync(genreId);
                return Ok(genre);            
        }

        [HttpGet("{genreId:int}/games")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<IEnumerable<GameSummaryDto>>> GetGamesByGenre(int genreId)
        {
            var games = await genreServices.GetGamesByGenre(genreId);

            return Ok(games);
        }

        //POST

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGenre([FromBody] GenreCreateDto newGenre)
        {
            var createdGenre = await genreServices.CreateGenreAsync(newGenre);

                return CreatedAtAction(nameof(GetGenreById),
                          new { genreId = createdGenre}, null);            
        }

        //PUT

        [HttpPut("{genreId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGenre(int genreId, [FromBody] UpdateGenreDto updateGenre)
        {

            await genreServices.UpdateGenreAsync(genreId, updateGenre);

                return NoContent();
        }

        //DELETE

        [HttpDelete("{genreId:int}")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGenre(int genreId)
        {
                await genreServices.DeleteGenreAsync(genreId);

                return NoContent();
        }
    }
}
