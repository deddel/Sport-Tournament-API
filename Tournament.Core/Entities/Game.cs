using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Entities
{
    public class Game
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public DateTime Time { get; set; }
        //FK
        public int TournamentDetailsId { get; set; }
        //NP
        public TournamentDetails? TournamentDetails { get; set; }// = default!;
    }
}
