using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
//using Tournament.Data.Data;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;

namespace Tournament.Presentation.Controllers
{
    [Route("api/TournamentDetails")]
    [ApiController]
    public class TournamentController : ControllerBase
    {
        private readonly IServiceManager _serviceManager;

        public TournamentController(IServiceManager serviceManager)
        {
            _serviceManager = serviceManager;
        }

        // GET: api/tournamentdetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includeGames)
        {
            var tournamentDtos = await _serviceManager.TournamentService.GetTournamentsAsync(includeGames);

            return Ok(tournamentDtos);
        }

        // GET: api/tournamentdetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournamentDto = await _serviceManager.TournamentService.GetTournamentAsync(id);

            if (tournamentDto == null)
            {
                return NotFound("The tournament does not exist");
            }

            return Ok(tournamentDto);
        }

        // PUT: api/tournamentdetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentUpdateDto dto)
        {
            try
            {
                var updatedTournament = await _serviceManager.TournamentService.PutTournamentAsync(id, dto);
                return Ok(updatedTournament);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }


        }

        // POST: api/tournamentdetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTournament(TournamentCreateDto dto)
        {
            try
            {
                var createdTournament = await _serviceManager.TournamentService.PostTournamentAsync(dto);

                return CreatedAtAction(nameof(GetTournament), new { id = createdTournament.Id }, createdTournament);
            }
            catch (ArgumentException ex) 
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }

        // DELETE: api/tournamentdetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {

            try
            {
                await _serviceManager.TournamentService.DeleteTournamentAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Tournament not found");  // Handle not found
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            
        }

        // PATCH: api/tournamentdetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournament(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument)
        {
            if (patchDocument == null)
                return BadRequest("No patch document");
            try
            {
                var tournamentDto = await _serviceManager.TournamentService.PatchTournamentAsync(id, patchDocument);

                if (!ModelState.IsValid)
                    return UnprocessableEntity(ModelState);

                return Ok(tournamentDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound("Tournament does not exist");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

        }
    }
}
