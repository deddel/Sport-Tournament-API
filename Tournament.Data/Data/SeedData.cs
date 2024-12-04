using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Data.Data
{
    public class SeedData
    {
        public static ICollection<TournamentDetails> GenerateTournamentDetails()
        {
            var tournamentDetails = new TournamentDetails
            {
                Title = "Swedish Open",
                StartDate = new DateTime(2025, 07, 12),
                Games = new List<Game>
                {
                    new Game 
                    {
                        Title = "Semi Final 1",
                        Time = new DateTime(2025,07,12,15,0,0),
                    },
                    new Game
                    {
                        Title = "Semi Final 2",
                        Time = new DateTime(2025,07,13,15,0,0),
                    }

                }
            };

            return new List<TournamentDetails> {tournamentDetails};
        }
    }
}
