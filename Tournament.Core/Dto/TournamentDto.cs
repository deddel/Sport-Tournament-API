using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public class TournamentDto
    {
        public int Id { get; init; }
        public string? Title { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate 
        { 
            get 
            {
                return StartDate.AddMonths(3);
            }
        }
        public ICollection<GameDto>? Games { get; init; }
    }
}
