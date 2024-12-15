using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using System.Text;
using System.Threading.Tasks;
using Service.Contracts;
using Tournament.Core.Repositories;

namespace Tournament.Services
{
    public class ServiceManager: IServiceManager
    {
        private readonly ITournamentService tournamentService;
        private readonly IGameService gameService;
        public ITournamentService TournamentService => tournamentService;

        public IGameService GameService => gameService;


        public ServiceManager(IUoW uow, IMapper mapper) 
        {
            ArgumentNullException.ThrowIfNull(nameof(uow));

            tournamentService = new TournamentService(uow, mapper);

            gameService = new GameService(uow, mapper);
        }

    }
}
