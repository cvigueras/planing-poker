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
            await GivenTwoDifferentsUsersWithVote();

            var result = await WhenTheUsersAreRetrieved();

            ThenBothUsersExistWithVote(result);
        }

        [Test]
        public async Task InsertAVoteForAUserAndGroupIdSuccesfully()
        {
            var givenUser = await GivenANewUser();
            await GivenSameUserWithInsertedVote(givenUser);

            var result = await WhenTheUserIsRetrieved(givenUser);

            TheUserExistWithVote(result);
        }

        private static void ThenBothUsersExistWithVote(IEnumerable<VotesUsers> result)
        {
            var expectedVotesUsers = new List<VotesUsers>
            {
                VotesUsers.Create("Carlos", "anyGameId", true, Vote.Create("3")),
                VotesUsers.Create("Juan", "anyGameId", false, Vote.Create("5")),
            };
            result.Should().BeEquivalentTo(expectedVotesUsers);
        }

        private async Task<IEnumerable<VotesUsers>> WhenTheUsersAreRetrieved()
        {
            return await voteRepository.GetAllVotesByGroupIdAsync("anyGameId");
        }

        private async Task GivenTwoDifferentsUsersWithVote()
        {
            var givenFirtsUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create("3"));
            var givenSecondUser = User.Create("Juan", "anyGameId", "anyConnectionId", false, Vote.Create("5"));
            await userRepository.Add(givenFirtsUser);
            await userRepository.Add(givenSecondUser);
        }

        private static void TheUserExistWithVote(User result)
        {
            var expectedUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create("3"));
            result.Should().BeEquivalentTo(expectedUser);
        }

        private async Task<User> WhenTheUserIsRetrieved(User givenUser)
        {
            return await userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);
        }

        private async Task GivenSameUserWithInsertedVote(User givenUser)
        {
            givenUser.Vote = Vote.Create("3");
            await voteRepository.AddVoteByUserNameAndGroupIdAsync(givenUser.Name, givenUser.GameId, givenUser.Vote.Value);
        }

        private async Task<User> GivenANewUser()
        {
            var givenUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create(""));
            await userRepository.Add(givenUser);
            return givenUser;
        }
    }
}
