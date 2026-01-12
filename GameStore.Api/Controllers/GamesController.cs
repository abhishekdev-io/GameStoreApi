using GameStore.Api.Dtos;
using GameStore.Api.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace GameStore.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GamesController : ControllerBase
    {
        private readonly IGameServices gameServices;

        public GamesController(IGameServices gameServices)
        {
            this.gameServices = gameServices;
        }

        //GET

        [HttpGet]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<ActionResult<IEnumerable<GameSummaryDto>>> GetGames()
        {
            var games = await gameServices.GetGamesAsync();

            return Ok(games);
        }

        [HttpGet("{gameId:int}")]
        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GameDetailsDto>> GetGameById(int gameId)
        {
            var game = await gameServices.GetGameByIdAsync(gameId);
            return Ok(game);
        }  

        //POST

        [HttpPost]
        [Authorize(Policy = "CanCreate")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> CreateGame([FromBody] CreateGameDto newGame)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var createdGameId = await gameServices.CreateGameAsync(newGame);

                return CreatedAtAction(nameof(GetGameById),
                          new {gameId = createdGameId}, null );         
        }

        //PUT

        [HttpPut("{gameId:int}")]
        [Authorize(Policy = "CanUpdate")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateGame(int gameId,[FromBody] UpdateGameDto updateGame)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await gameServices.UpdateGameAsync(gameId, updateGame);

                return NoContent();           
        }

        //DELETE

        [HttpDelete("{gameId:int}")]
        [Authorize(Policy = "CanDelete")]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> DeleteGame(int gameId)
        {           
                await gameServices.DeleteGameAsync(gameId);

                return NoContent();            
        }
    }
}
