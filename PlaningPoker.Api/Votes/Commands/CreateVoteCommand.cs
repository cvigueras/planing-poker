using MediatR;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Test.Votes.Commands
{
    public class CreateVoteCommand : IRequest
    {
        public VotesUsersCreateDto VotesUsers { get; }
        public CreateVoteCommand(VotesUsersCreateDto votesUsers)
        {
            VotesUsers = votesUsers;
        }

    }
}