using AutoMapper;
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
        public async Task<IEnumerable<TournamentDto>> GetTournamentsAsync(bool includeEmployees)
        {
            return _mapper.Map<IEnumerable<TournamentDto>>(await _uow.TournamentRepository.GetAllAsync(includeEmployees));
        }

        public async Task<TournamentDto> GetTournamentAsync(int id)
        {
            TournamentDetails? tournament = await _uow.TournamentRepository.GetAsync(id);

            if (tournament == null)
            {
                //ToDo: Fix later
            }

            return _mapper.Map<TournamentDto>(tournament);

        }

    }
}
