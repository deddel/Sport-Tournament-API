using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;
using Tournament.Core.Repositories;
using Tournament.Data.Data;

namespace Tournament.Data.Repositories
{
    public class TournamentRepository : ITournamentRepository
    {
        private TournamentApiContext Context { get;}
        public TournamentRepository(TournamentApiContext context)
        {
            Context = context;
        }
        public async Task<IEnumerable<TournamentDetails>> GetAllAsync()
        {
            return await Context.TournamentDetails.ToListAsync();
        }
        public async Task<TournamentDetails?> GetAsync(int id)
        {
            return await Context.TournamentDetails
                .Include(t => t.Games)
                .FirstOrDefaultAsync(t => t.Id.Equals(id));
        }
        public async Task<bool> AnyAsync(int id)
        {
            return await Context.TournamentDetails.AnyAsync(t => t.Id.Equals(id));
        }
        public void Add(TournamentDetails tournament)
        {
            Context.TournamentDetails.Add(tournament);
        }
        public void Remove(TournamentDetails tournament)
        {
            Context.TournamentDetails.Remove(tournament);
        }
        public void Update(TournamentDetails tournament)
        {
            Context.TournamentDetails.Update(tournament);
        }
    }
}
