using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private TournamentApiContext _context { get;}
        public TournamentRepository(TournamentApiContext context)
        {
            _context = context;
        }

        public void Add(TournamentDetails tournament)
        {
            _context.TournamentDetails.Add(tournament);
        }

        public async Task<bool> AnyAsync(int id)
        {
            return await _context.TournamentDetails.AnyAsync(t => t.Id == id);
        }

        public Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
            return _context.TournamentDetails.;
        }

        public Task<TournamentDetails> GetAsync(int id)
        {
            throw new NotImplementedException();
        }

        public void Remove(TournamentDetails tournament)
        {
            throw new NotImplementedException();
        }

        public void Update(TournamentDetails tournament)
        {
            throw new NotImplementedException();
        }
    }
}
