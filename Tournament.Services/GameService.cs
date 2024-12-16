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
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);

            if (!tournamentExist) throw new KeyNotFoundException($"Tournament with ID {tournamentId} not found");

            var games = await _uow.GameRepository.GetAllAsync(tournamentId);
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }
        public async Task<GameDto> GetGameAsync(int tournamentId, int id)
        {
            var tournamentExist = await _uow.TournamentRepository.AnyAsync(tournamentId);

            if (!tournamentExist) throw new KeyNotFoundException($"Tournament with ID {tournamentId} not found");

            var game = await _uow.GameRepository.GetAsync(tournamentId, id);

            if (game == null)
            {
                throw new KeyNotFoundException($"Game with id {id} not found");
            }

            return _mapper.Map<GameDto>(game);
        }

        public async Task<IEnumerable<GameDto>> FindGames(string searchString)
        {
            var games = await _uow.GameRepository.FindGamesByTitle(searchString);

            if (games.Count() == 0)
            {
                throw new KeyNotFoundException($"No Games found with the title: {searchString}");
            }
            return _mapper.Map<IEnumerable<GameDto>>(games);
        }

    }
}
