using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tournament.Core.Dto
{
    public class GameCreateDto
    {
        public string? Title { get; set; }
        public DateTime Time { get; set; }
    }
}
