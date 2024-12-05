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
    public class GameRepository : IGameRepository
    {
        private TournamentApiContext Context;

        public GameRepository(TournamentApiContext context) 
        {
            Context = context;
        }
        public async Task<IEnumerable<Game>> GetAllAsync(int tournamentDetailsId)
        {
            var games = await Context.Games
                .Where(g => g.TournamentDetailsId
                .Equals(tournamentDetailsId))
                .ToListAsync();

            return games;
        }

        public async Task<Game?> GetAsync(int TournamentDetailsId, int gameId)
        {
            var game = await Context.Games
                .Where(g => g.TournamentDetailsId.Equals(TournamentDetailsId) && 
                g.Id.Equals(gameId))
                .FirstOrDefaultAsync();
            
            return game;
        }
        public Task<bool> AnyAsync(int TournamentDetailsId, int gameId)
        {
            throw new NotImplementedException();
        }
        public void Add(Game game)
        {
            Context.Games.Add(game);
        }
        public void Remove(Game game)
        {
            throw new NotImplementedException();
        }

        public void Update(Game game)
        {
            throw new NotImplementedException();
        }
    }
}
