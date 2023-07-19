using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Votes.Repositories
{
    public interface IVoteRepository
    {
        Task AddVoteByUserNameAndGroupIdAsync(string name, string gameId, string vote);
        Task<IEnumerable<VotesUsers>> GetAllVotesByGroupIdAsync(string gameId);
    }
}