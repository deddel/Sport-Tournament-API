using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Repositories
{
    public interface IGameRepository
    {
        Task<IEnumerable<Game>> GetAllAsync(int TournamentDetailsId);
        Task<Game?> GetAsync(int TournamentDetailsId, int gameId);
        Task<bool> AnyAsync(int TournamentDetailsId, int gameId);
        void Add(Game game);
        void Update(Game game);
        void Remove(Game game);
    }
}
