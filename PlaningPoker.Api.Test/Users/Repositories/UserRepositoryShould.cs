using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi.Games.Repositories;
using webapi.Helpers;
using webapi.Users.Models;
using webapi.Users.Repositories;

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
            var userGuid = guidGenerator.Generate().ToString();
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
            var guidUser1 = Guid.Parse("49c4d829-b7e7-45ba-8db0-9da9eaee4388").ToString();
            var guidUser2 = Guid.Parse("2fdb57e9-d2bb-43dc-bd2b-c3577717d5da").ToString();
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