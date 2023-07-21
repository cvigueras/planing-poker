using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Users.Repositories
{
    public class UserRepositoryShould
    {
        private UserRepository userRepository;
        private GameRepository gameRepository;
        private SQLiteConnection connection;
        private SetupFixture setupFixture;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connection);
            gameRepository = new GameRepository(connection);
            guidGenerator = Substitute.For<IGuidGenerator>();
        }

        [Test]
        public async Task FailWhenRetrieveANonExistingUser()
        {
            var guidGame = guidGenerator.Generate().ToString();

            var action = async () => await userRepository.GetByNameAndGameId("Juan", guidGame);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingUser()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, string.Empty, true, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);

            var result = await userRepository.GetByNameAndGameId("Carlos", gameGuid);

            var expectedUser = User.Create("Carlos", givenUser.GameId, string.Empty, true, Vote.Create(string.Empty));
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task RetrieveUsersForAGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user1 = User.Create("Carlos", givenGame.Id, string.Empty, true, Vote.Create(string.Empty));
            await userRepository.Add(user1);
            var user2 = User.Create("Pedro", givenGame.Id, string.Empty, false, Vote.Create(string.Empty));
            await userRepository.Add(user2);

            var result = await userRepository.GetUsersGameByGameId(givenGame.Id);

            var expectedUsers = new List<User>
            {
                User.Create("Carlos", givenGame.Id, string.Empty, true, Vote.Create(string.Empty)),
                User.Create("Pedro", givenGame.Id, string.Empty, false, Vote.Create(string.Empty)),
            };
            result.Should().BeEquivalentTo(expectedUsers);
        }

        [Test]
        public async Task RetrieveAnExistingUserByConnectionId()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, "connectionId123", false, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);

            var result = await userRepository.GetByConnectionId("connectionId123");

            var expectedUser = User.Create("Carlos", givenUser.GameId, "connectionId123", false, Vote.Create(string.Empty));
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task UpdateByConnectionIdAnExistingUser()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, "connectionId123", false, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);
            givenUser.ConnectionId = "connectionId456" ;
            await userRepository.UpdateByConnectionId(givenUser, "connectionId123");

            var result = await userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);

            var expectedUser = User.Create("Carlos", givenUser.GameId, "connectionId456", false, Vote.Create(string.Empty));
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task UpdateByGameIdAnExistingUser()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, "connectionId123", false, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);
            givenUser.ConnectionId = "connectionId456" ;
            await userRepository.UpdateByGameIdAndName(givenUser, givenUser.GameId);

            var result = await userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);

            var expectedUser = User.Create("Carlos", givenUser.GameId, "connectionId456", false, Vote.Create(string.Empty));
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task DeleteAnExistingUser()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, "connectionId123", true, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);
            await userRepository.DeleteByGameIdAndName(gameGuid, givenUser.Name);

            var action = async () => await userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task InsertAVoteForAUserSuccesfullyAsync()
        {
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create("Carlos", gameGuid, "connectionId123", false, Vote.Create("3"));
            await userRepository.Add(givenUser);

            givenUser.Vote = Vote.Create("5");
            await userRepository.UpdateByConnectionId(givenUser, "connectionId123");

            var result = await userRepository.GetByNameAndGameId(givenUser.Name, givenUser.GameId);

            var expectedUser = User.Create("Carlos", givenUser.GameId, "connectionId123", false, Vote.Create("5"));
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task RetrieveNumberAllTotalUsersAsync()
        {
            var result = await userRepository.GetTotalNumberUsers();

            result.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}