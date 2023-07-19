using AutoMapper;
using MediatR;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;

namespace PlaningPoker.Api.Test.Votes.Commands
{
    public class CreateVoteCommandHandler : IRequestHandler<CreateVoteCommand>
    {
        private IVoteRepository voteRepository;
        private IMapper mapper;

        public CreateVoteCommandHandler(IVoteRepository voteRepository, IMapper mapper)
        {
            this.voteRepository = voteRepository;
            this.mapper = mapper;
        }

        public async Task Handle(CreateVoteCommand request, CancellationToken cancellationToken)
        {
            await voteRepository.AddVoteByUserNameAndGroupIdAsync(
                request.VotesUsers.Name, request.VotesUsers.Group, request.VotesUsers.Value);
        }
    }
}