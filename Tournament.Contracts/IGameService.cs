using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts
{
    public interface IGameService
    {
        Task<IEnumerable<GameDto>> GetGamesAsync(int tournamentId);
        Task<GameDto> GetGameAsync(int tournamentId, int id);
        Task<IEnumerable<GameDto>> FindGames(string searchString);
    }
}
