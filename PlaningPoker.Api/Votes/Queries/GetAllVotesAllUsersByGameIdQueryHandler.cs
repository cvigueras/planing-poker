using AutoMapper;
using MediatR;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Queries;
using PlaningPoker.Api.Votes.Repositories;

namespace PlaningPoker.Api.Test.Votes.Queries
{
    public class GetAllVotesAllUsersByGameIdQueryHandler : IRequestHandler<GetAllVotesAllUsersByGameIdQuery, IEnumerable<VotesUsersReadDto>>
    {
        private IVoteRepository voteRepository;
        private IMapper mapper;

        public GetAllVotesAllUsersByGameIdQueryHandler(IVoteRepository voteRepository, IMapper mapper)
        {
            this.voteRepository = voteRepository;
            this.mapper = mapper;
        }

        public async Task<IEnumerable<VotesUsersReadDto>> Handle(GetAllVotesAllUsersByGameIdQuery request, CancellationToken cancellationToken)
        {
            var votesUsers = await voteRepository.GetAllVotesByGroupIdAsync(request.GameId);
            return mapper.Map<List<VotesUsersReadDto>>(votesUsers);
        }
    }
}