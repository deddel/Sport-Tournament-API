using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Entities;

namespace Tournament.Core.Dto
{
    public class GameUpdateDto
    {
        public string? Title { get; set; }
        public DateTime Time { get; set; }
    }
}
