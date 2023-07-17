using MediatR;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Votes.Queries
{
    public class GetAllVotesAllUsersByGameIdQuery : IRequest<IEnumerable<UsersVotesReadDto>>
    {
        public string GameId { get; set; }

        public GetAllVotesAllUsersByGameIdQuery(string gameId)
        {
            this.GameId = gameId;
        }
    }
}