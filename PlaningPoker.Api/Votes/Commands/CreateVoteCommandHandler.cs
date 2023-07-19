using AutoMapper;
using MediatR;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;

namespace PlaningPoker.Api.Test.Votes.Commands
{
    public class CreateVoteCommandHandler : IRequestHandler<CreateVoteCommand, VotesUsersReadDto>
    {
        private IVoteRepository voteRepository;
        private IMapper mapper;

        public CreateVoteCommandHandler(IVoteRepository voteRepository, IMapper mapper)
        {
            this.voteRepository = voteRepository;
            this.mapper = mapper;
        }

        public Task<VotesUsersReadDto> Handle(CreateVoteCommand request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}