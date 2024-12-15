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
    public class GameService : IGameService
    {
        private readonly IUoW _uow;
        private readonly IMapper _mapper;

        public GameService(IUoW uow, IMapper mapper) 
        {
            _uow = uow;
            _mapper = mapper;
        }
        public async Task<IEnumerable<GameDto>> GetGamesAsync(int tournamentId)
        {
            return _mapper.Map<IEnumerable<GameDto>>(await _uow.GameRepository.GetAllAsync(tournamentId));
        }
        public async Task<GameDto> GetGameAsync(int tournamentId, int id)
        {
            return _mapper.Map<GameDto>(await _uow.GameRepository.GetAsync(tournamentId, id));
        }

    }
}
