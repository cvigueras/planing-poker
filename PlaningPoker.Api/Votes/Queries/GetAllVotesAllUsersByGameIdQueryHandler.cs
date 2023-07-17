using MediatR;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Queries;

namespace PlaningPoker.Api.Test.Votes.Queries
{
    internal class GetAllVotesAllUsersByGameIdQueryHandler : IRequestHandler<GetAllVotesAllUsersByGameIdQuery, IEnumerable<UsersVotesReadDto>>
    {
        public Task<IEnumerable<UsersVotesReadDto>> Handle(GetAllVotesAllUsersByGameIdQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}