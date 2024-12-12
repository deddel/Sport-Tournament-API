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
using AutoMapper;
using Tournament.Core.Dto;
using Microsoft.AspNetCore.JsonPatch;

namespace Tournament.Api.Controllers
{
    [Route("api/TournamentDetails")]
    [ApiController]
    public class TournamentDetailsController : ControllerBase
    {
        //private readonly TournamentApiContext _context;
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentDetailsController(IMapper mapper,IUoW uow)
        {
            _uow = uow;
            _mapper = mapper;
        }

        // GET: api/tournamentdetails
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament(bool includeGames)
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync(includeGames);
            var tournamentDtos = _mapper.Map<IEnumerable<TournamentDto>>(tournaments);

            return Ok(tournamentDtos);
        }

        // GET: api/tournamentdetails/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TournamentDto>> GetTournament(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                return NotFound("The tournament does not exist");
            }

            var tournamentDto = _mapper.Map<TournamentDto>(tournament);

            return Ok(tournamentDto);
        }

        // PUT: api/tournamentdetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTournament(int id, TournamentUpdateDto dto)
        {
            if (id != dto.Id) return BadRequest();

            var existingTournament = await _uow.TournamentRepository.GetAsync(id);

            if (existingTournament == null) return NotFound("The tournament does not exist");

            _mapper.Map(dto, existingTournament);

            try
            {
                await _uow.CompleteAsync();
            }

            catch (Exception ex) 
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(_mapper.Map<TournamentDto>(existingTournament));

        }

        // POST: api/tournamentdetails
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<IActionResult> PostTournament(TournamentCreateDto dto)
        {
            var tournament = _mapper.Map<TournamentDetails>(dto);
            _uow.TournamentRepository.Add(tournament);

            try
            {
                await _uow.CompleteAsync();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            var createdTournament = _mapper.Map<TournamentDto>(tournament);
            return CreatedAtAction(nameof(GetTournament), new { id = createdTournament.Id }, createdTournament);
        }

        // DELETE: api/tournamentdetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null) return NotFound();

            _uow.TournamentRepository.Remove(tournament);
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

        // PATCH: api/tournamentdetails/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPatch("{id}")]
        public async Task<IActionResult> PatchTournament(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument)
        {
            if (patchDocument == null) return BadRequest("No patch document");

            var tournamentToPatch = await _uow.TournamentRepository.GetAsync(id);

            if (tournamentToPatch == null) return NotFound("Tournament does not exist");

            var dto = _mapper.Map<TournamentUpdateDto>(tournamentToPatch);

            patchDocument.ApplyTo(dto, ModelState);

            //Validate ModelState for dto after patch
            TryValidateModel(dto);
            if (!ModelState.IsValid) return UnprocessableEntity(ModelState);

            _mapper.Map(dto, tournamentToPatch);

            try
            {
                await _uow.CompleteAsync();
            }

            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }

            return Ok(_mapper.Map<TournamentDto>(tournamentToPatch));
        }
    }
}
