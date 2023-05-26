using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi;

namespace PlaningPoker.Api.Test
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
            var guid = guidGenerator.Generate().ToString();

            var action = async () => await userRepository.GetById(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingUser()
        {
            var userGuid = guidGenerator.Generate().ToString();
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create(userGuid, "Carlos", gameGuid);
            await userRepository.Add(givenUser);

            var result = await userRepository.GetById(givenUser.Id);

            var expectedUserId = givenUser.Id;
            var expectedUserName = "Carlos";
            var expectedGameId = givenUser.GameId;
            result.Id.Should().BeEquivalentTo(expectedUserId);
            result.Name.Should().BeEquivalentTo(expectedUserName);
            result.GameId.Should().BeEquivalentTo(expectedGameId);
        }

        [Test]
        public async Task RetrieveUsersForAGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            var guidUser1 = Guid.Parse("49c4d829-b7e7-45ba-8db0-9da9eaee4388").ToString();
            var guidUser2 = Guid.Parse("2fdb57e9-d2bb-43dc-bd2b-c3577717d5da").ToString();
            await gameRepository.Add(givenGame);
            var user1 = User.Create(guidUser1, "Carlos", givenGame.Id);
            await userRepository.Add(user1);
            var user2 = User.Create(guidUser2, "Pedro", givenGame.Id);
            await userRepository.Add(user2);

            var result = userRepository.GetUsersGameByGameId(givenGame.Id);

            var expectedUsers = new List<User>
            {
                User.Create(guidUser1, "Carlos", givenGame.Id),
                User.Create(guidUser2, "Pedro", givenGame.Id),
            };
            result.Should().Be(expectedUsers);
        }
    }
}