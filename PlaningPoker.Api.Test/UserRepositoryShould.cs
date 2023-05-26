using System.Data.SQLite;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class UserRepositoryShould
    {
        private UserRepository repository;
        private SQLiteConnection connection;
        private SetupFixture setupFixture;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new UserRepository(connection);
            guidGenerator = Substitute.For<IGuidGenerator>();
        }

        [Test]
        public async Task FailWhenRetrieveANonExistingUser()
        {
            var guid = guidGenerator.Generate().ToString();

            var action = async () => await repository.GetById(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingUser()
        {
            var userGuid = guidGenerator.Generate().ToString();
            var gameGuid = guidGenerator.Generate().ToString();
            var givenUser = User.Create(userGuid, "Carlos", gameGuid);
            await repository.Add(givenUser);

            var result = await repository.GetById(givenUser.Id);
            
            var expectedUserId = givenUser.Id;
            var expectedUserName = "Carlos";
            var expectedGameId = givenUser.GameId;
            result.Id.Should().BeEquivalentTo(expectedUserId);
            result.Name.Should().BeEquivalentTo(expectedUserName);
            result.GameId.Should().BeEquivalentTo(expectedGameId);
        }
    }
}