using AutoMapper;
using Microsoft.AspNetCore.JsonPatch;
using Service.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class TournamentService: ITournamentService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public TournamentService(IUoW uow, IMapper mapper) 
        {
            _uow = uow;
            _mapper = mapper; 
        }
        public async Task<IEnumerable<TournamentDto>> GetTournamentsAsync(bool includeGames)
        {
            return _mapper.Map<IEnumerable<TournamentDto>>(await _uow.TournamentRepository.GetAllAsync(includeGames));
        }

        public async Task<TournamentDto> GetTournamentAsync(int id)
        {
            TournamentDetails? tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                throw new KeyNotFoundException("Tournament not found");
            }

            return _mapper.Map<TournamentDto>(tournament);

        }

        public async Task<TournamentDto> PutTournamentAsync(int id, TournamentUpdateDto dto)
        {
            if (id != dto.Id)
            {
                throw new ArgumentException("Id mismatch");
            }

            var existingTournament = await _uow.TournamentRepository.GetAsync(id);
            if (existingTournament == null) throw new KeyNotFoundException($"Tournament with ID {id} not found");


            _mapper.Map(dto, existingTournament);

            await _uow.CompleteAsync();

            return _mapper.Map<TournamentDto>(existingTournament);
        }

        public async Task<TournamentDto> PostTournamentAsync(TournamentCreateDto dto)
        {
            var tournament = _mapper.Map<TournamentDetails>(dto);

            _uow.TournamentRepository.Add(tournament);

            await _uow.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournament);
        }

        public async Task DeleteTournamentAsync(int id)
        {
            var tournament = await _uow.TournamentRepository.GetAsync(id);
            if (tournament == null) throw new KeyNotFoundException("Tournament not found");

            _uow.TournamentRepository.Remove(tournament);

            await _uow.CompleteAsync();
        }

        public async Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument)
        {
            var tournamentToPatch = await _uow.TournamentRepository.GetAsync(id);
            if (tournamentToPatch == null) throw new KeyNotFoundException("Tournament does not exist");

            var dto = _mapper.Map<TournamentUpdateDto>(tournamentToPatch);

            patchDocument.ApplyTo(dto);

            _mapper.Map(dto, tournamentToPatch);
            await _uow.CompleteAsync();

            return _mapper.Map<TournamentDto>(tournamentToPatch);
        }
    }
}
