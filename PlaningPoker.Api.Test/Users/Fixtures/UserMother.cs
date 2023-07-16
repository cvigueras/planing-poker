using PlaningPoker.Api.Users.Models;

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
