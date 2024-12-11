using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
        public async Task<IEnumerable<Game>> FindGamesByTitle(string searchString)
        {
            if (string.IsNullOrEmpty(searchString))
            {
                // Return an empty collection if the search string is null or empty.
                return Enumerable.Empty<Game>();  
            }

            var games = await Context.Games
                .Where(g => g.Title != null && g.Title.Equals(searchString))
                .ToListAsync();
            return games;
        }


        public async Task<Game?> GetAsync(int tournamentDetailsId, int gameId)
        {
            var game = await Context.Games
                .Where(g => g.TournamentDetailsId.Equals(tournamentDetailsId) && 
                g.Id.Equals(gameId))
                .FirstOrDefaultAsync();
            
            return game;
        }
        public async Task<bool> AnyAsync(int tournamentDetailsId, int gameId)
        {
            return await Context.Games
                .AnyAsync(g => g.TournamentDetailsId.Equals(tournamentDetailsId) &&
                g.Id.Equals(gameId));
        }
        public void Add(Game game)
        {
            Context.Games.Add(game);
        }
        public void Remove(Game game)
        {
            Context.Games.Remove(game);
        }

        public void Update(Game game)
        {
            // Check if the entity is already being tracked
            var trackedEntity = Context.Games.Local
                .FirstOrDefault(e => e.Id == game.Id);

            if (trackedEntity != null)
            {
                // Detach the tracked entity to avoid conflicts
                Context.Entry(trackedEntity).State = EntityState.Detached;
            }
            Context.Games.Update(game);
        }
    }
}
