using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Votes.Repositories
{
    public class VotesRepositoryShould
    {

        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IVoteRepository voteRepository;
        private IUserRepository userRepository;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            voteRepository = new VoteRepository(connection);
            userRepository = new UserRepository(connection);
        }

        [Test]
        public async Task ReturnAllVotesByGameIdAsync()
        {
            var givenFirtsUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create("3"));
            var givenSecondUser = User.Create("Juan", "anyGameId", "anyConnectionId", true, Vote.Create("5"));

            await userRepository.Add(givenFirtsUser);
            await userRepository.Add(givenSecondUser);

            var result = await voteRepository.GetAllVotesByGroupIdAsync("anyGameId");

            var expectedVotesUsers = new List<VotesUsers>
            {
                VotesUsers.Create("Carlos", Vote.Create("3")),
                VotesUsers.Create("Juan", Vote.Create("5")),
            };
            result.Should().BeEquivalentTo(expectedVotesUsers);
        }

        [Test]
        public async Task InsertAVoteForAUserAndGroupIdSuccesfully()
        {
            var givenUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create(""));
            await userRepository.Add(givenUser);
            givenUser.Vote = Vote.Create("3");
            await voteRepository.AddVoteByUserNameAndGroupIdAsync(givenUser.Name, givenUser.GameId, givenUser.Vote.Value);

            var result = userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);

            var expectedUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create("3"));
            result.Should().BeEquivalentTo(expectedUser);
        }
    }
}
