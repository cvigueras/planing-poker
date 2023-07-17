using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Votes.Repositories
{
    public interface IVoteRepository
    {
        Task<IEnumerable<VotesUsers>> GetVotesByGameIdAsync(string gameId);
    }
}