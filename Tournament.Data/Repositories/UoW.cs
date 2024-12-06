using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tournament.Core.Repositories;

namespace Tournament.Data.Repositories
{
    public class UoW : IUoW
    {
        public ITournamentRepository TournamentRepository => throw new NotImplementedException();

        public IGameRepository GameRepository => throw new NotImplementedException();

        public Task CompleteAsync()
        {
            throw new NotImplementedException();
        }
    }
}
