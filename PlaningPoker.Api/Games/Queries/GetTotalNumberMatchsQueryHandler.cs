using MediatR;
using PlaningPoker.Api.Games.Repositories;

namespace PlaningPoker.Api.Test.Games.Queries
{
    public class GetTotalNumberMatchsQueryHandler : IRequestHandler<GetTotalNumberMatchsQuery, long>
    {
        private IGameRepository repository;

        public GetTotalNumberMatchsQueryHandler(IGameRepository repository)
        {
            this.repository = repository;
        }

        public Task<long> Handle(GetTotalNumberMatchsQuery request, CancellationToken cancellationToken)
        {
            return repository.GetTotalNumberMatchs();
        }
    }
}