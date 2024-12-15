using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class UoW : IUoW
    {
        private readonly TournamentApiContext _context;
        private readonly ITournamentRepository _tournamentRepository;
        private readonly IGameRepository _gameRepository;

        public ITournamentRepository TournamentRepository => _tournamentRepository;

        public IGameRepository GameRepository => _gameRepository;

        public UoW(TournamentApiContext context, ITournamentRepository tournamentRepository, IGameRepository gameRepository) 
        {
            _context = context;
            _tournamentRepository = tournamentRepository;
            _gameRepository = gameRepository;
        }

        public async Task CompleteAsync()
        {
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error when saving changes", ex);
            }
        }
    }
}
