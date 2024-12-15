using Microsoft.AspNetCore.JsonPatch;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Dto;

namespace Service.Contracts
{
    public interface ITournamentService
    {
        Task<IEnumerable<TournamentDto>> GetTournamentsAsync(bool includeEmployees);
        Task<TournamentDto> GetTournamentAsync(int id);

        Task<TournamentDto> PutTournamentAsync(int id, TournamentUpdateDto dto);
        Task<TournamentDto> PostTournamentAsync(TournamentCreateDto dto);
        Task DeleteTournamentAsync(int id);
        Task<TournamentDto> PatchTournamentAsync(int id, JsonPatchDocument<TournamentUpdateDto> patchDocument);
    }
}
