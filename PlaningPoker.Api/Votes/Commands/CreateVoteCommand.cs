using MediatR;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Test.Votes.Commands
{
    public class CreateVoteCommand : IRequest<VotesUsersReadDto>
    {
        public CreateVoteCommand()
        {
            
        }
    }
}