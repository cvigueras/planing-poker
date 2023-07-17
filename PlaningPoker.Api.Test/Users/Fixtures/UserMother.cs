using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Test.Users.Fixtures
{
    public static class UserMother
    {
        public static User GetUserWithValidVote()
        {
            return User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create("3"));
        }
    }
}
