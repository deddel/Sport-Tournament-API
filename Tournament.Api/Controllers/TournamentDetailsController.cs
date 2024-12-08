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
    [Route("api/TournamentDetails")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uow;

        public TournamentDetailsController(IUoW uow)
        {
            _uow = uow;
        }

        // GET: api/TournamentDetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDetails>>> GetTournamentDetails()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
            return Ok(tournaments);
        }

        // GET: api/TournamentDetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDetails>> GetTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);

            if (tournamentDetails == null)
            {
                return NotFound();
            }

            return Ok(tournamentDetails);
        }

        // PUT: api/TournamentDetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournamentDetails(int id, TournamentDetails tournamentDetails)
        {
            if (id != tournamentDetails.Id) return BadRequest();

            var existingTournamentDetails = await _uow.TournamentRepository.GetAsync(id);

            if (existingTournamentDetails == null) return NotFound();

            //Update the existingTournament
            //existingTournamentDetails.Title = tournamentDetails.Title;
            //existingTournamentDetails.StartDate = tournamentDetails.StartDate;

            _uow.TournamentRepository.Update(tournamentDetails);

            try
            {
                await _uow.CompleteAsync();
            }

            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return NoContent();

        }

        // POST: api/TournamentDetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTournamentDetails(TournamentDetails tournamentDetails)
        {
            _uow.TournamentRepository.Add(tournamentDetails);
            await _uow.CompleteAsync();

            return CreatedAtAction(nameof(GetTournamentDetails), new { id = tournamentDetails.Id }, tournamentDetails);
        }

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournamentDetails(int id)
        {
            var tournamentDetails = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentDetails == null) return NotFound();

            _uow.TournamentRepository.Remove(tournamentDetails);
            await _uow.CompleteAsync();

            return NoContent();
        }

        //private bool TournamentDetailsExists(int id)
        //{
        //    return _context.TournamentDetails.Any(e => e.Id == id);
        //}
    }
}
