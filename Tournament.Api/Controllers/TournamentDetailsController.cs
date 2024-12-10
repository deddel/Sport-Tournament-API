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
        public async Task<ActionResult<IEnumerable<TournamentDto>>> GetTournament()
        {
            var tournaments = await _uow.TournamentRepository.GetAllAsync();
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
                return NotFound();
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

            if (existingTournament == null) return NotFound();

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

        // POST: api/TournamentDetails
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

        // DELETE: api/TournamentDetails/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTournament(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null) return NotFound();

            _uow.TournamentRepository.Remove(tournament);
            await _uow.CompleteAsync();

            return NoContent();
        }

        //private bool TournamentDetailsExists(int id)
        //{
        //    return _context.TournamentDetails.Any(e => e.Id == id);
        //}
    }
}
