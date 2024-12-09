using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Api.Controllers
{
    [Route("api/tournamentdetails/{tournamentId}/games")]
    [ApiController]
    public class GamesController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uow;

        public GamesController(IUoW uow)
        {
            _uow = uow;
        }

        // GET: api/tournamentdetails/5/games
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Game>>> GetGames(int tournamentId)
        {
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);

            if (!tournamentExist) return NotFound("The tournament does not exist");

            var games = await _uow.GameRepository.GetAllAsync(tournamentId);

            return Ok(games);
        }

        // GET: api/tournamentdetails/5/games/10
        [HttpGet("{id}")]
        public async Task<ActionResult<Game>> GetGame(int tournamentId ,int id)
        {
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);

            if (!tournamentExist) return NotFound("The tournament does not exist");

            var game = await _uow.GameRepository.GetAsync(tournamentId, id);

            if (game == null)
            {
                return NotFound();
            }

            return Ok(game);
        }

        // PUT: api/tournamentdetails/5/games/10
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutGame(int tournamentId ,int id, Game game)
        {
            // Check if the game object is being properly bound
            if (game == null)
            {
                return BadRequest("Game object is null.");
            }

            // Check if model is valid
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest($"Model is invalid: {errors}");
            }

            //Check if the tournament exist
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);
            if (!tournamentExist) return NotFound("The tournament does not exist");

            //Check if the game exists
            var gameExist = await _uow.GameRepository.AnyAsync(tournamentId, id);
            if (!gameExist) return NotFound("The game does not exist");

            //Check that game ID match
            //Todo game.Id=0. tournamentDetailsID=0
            //if (id != game.Id) return BadRequest();
            
            _uow.GameRepository.Update(game);

            try
            {
                await _uow.CompleteAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(game);
        }

        // POST: api/tournamentdetails/5/games
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Game>> PostGame(int tournamentId, Game game)
        {
            // Check if the game object is being properly bound
            if (game == null)
            {
                return BadRequest("Game object is null.");
            }

            // Check if model is valid
            if (!ModelState.IsValid)
            {
                var errors = string.Join(", ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
                return BadRequest($"Model is invalid: {errors}");
            }

            //Check if the tournament exist
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);
            if (!tournamentExist) return NotFound("The tournament does not exist");

            //Associate the game with the tournament
            game.TournamentDetailsId = tournamentId;

            //Add the game to the database
            _uow.GameRepository.Add(game);
            await _uow.CompleteAsync();

            return CreatedAtAction(nameof(GetGame), new { tournamentId, id = game.Id }, game);
            //return StatusCode(201, game);
        }

        // DELETE: api/TournamentDetails/5/Games/10
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteGame(int tournamentId, int id)
        {
            var tournamentExists = await _uow.TournamentRepository.AnyAsync(tournamentId);

            if (!tournamentExists) return NotFound();

            var game = await _uow.GameRepository.GetAsync(tournamentId, id);
            if (game == null) return NotFound();

            _uow.GameRepository.Remove(game);
            await _uow.CompleteAsync();

            return NoContent();
        }

        //private bool GameExists(int id)
        //{
        //    return _context.Game.Any(e => e.Id == id);
        //}
    }
}
