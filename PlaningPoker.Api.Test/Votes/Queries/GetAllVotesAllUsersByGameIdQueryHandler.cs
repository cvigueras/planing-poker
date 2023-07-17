using AutoMapper;
using PlaningPoker.Api.Votes.Queries;
using PlaningPoker.Api.Votes.Repositories;

namespace PlaningPoker.Api.Test.Votes.Queries
{
    internal class GetAllVotesAllUsersByGameIdQueryHandler
    {
        private IVoteRepository voteRepository;
        private IMapper mapper;

        public GetAllVotesAllUsersByGameIdQueryHandler(IVoteRepository voteRepository, IMapper mapper)
        {
            this.voteRepository = voteRepository;
            this.mapper = mapper;
        }

        internal object Handle(GetAllVotesAllUsersByGameIdQuery query)
        {
            throw new NotImplementedException();
        }
    }
}