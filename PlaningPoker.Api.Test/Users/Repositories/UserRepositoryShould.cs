using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
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
            var givenUser = User.Create("Carlos", gameGuid);
            await userRepository.Add(givenUser);

            var result = await userRepository.GetByNameAndGameId("Carlos", gameGuid);

            var expectedUser = User.Create("Carlos", givenUser.GameId);
            result.Should().BeEquivalentTo(expectedUser);
        }

        [Test]
        public async Task RetrieveUsersForAGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user1 = User.Create("Carlos", givenGame.Id);
            await userRepository.Add(user1);
            var user2 = User.Create("Pedro", givenGame.Id);
            await userRepository.Add(user2);

            var result = await userRepository.GetUsersGameByGameId(givenGame.Id);

            var expectedUsers = new List<User>
            {
                User.Create("Carlos", givenGame.Id),
                User.Create("Pedro", givenGame.Id),
            };
            result.Should().BeEquivalentTo(expectedUsers);
        }
    }
}