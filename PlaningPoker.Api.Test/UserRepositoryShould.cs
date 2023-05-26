using System.Data.SQLite;
using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class UserRepositoryShould
    {
        private UserRepository repository;
        private SQLiteConnection connection;
        private SetupFixture setupFixture;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new UserRepository(connection);
        }

        [Test]
        public async Task FailWhenRetrieveANonExistingUser()
        {
            var guid = Guid.NewGuid().ToString();

            var action = async () => await repository.GetById(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingUser()
        {
            var givenUser = new User
            {
                Id = Guid.NewGuid().ToString(),
                Name = "Carlos",
                GameId = Guid.NewGuid().ToString(),
            };
            await repository.Add(givenUser);
            var expectedUser = new User
            {
                Id = givenUser.Id,
                Name = "Carlos",
                GameId = givenUser.GameId,
            };

            var result = await repository.GetById(expectedUser.Id);
            
            result.Should().BeEquivalentTo(expectedUser);
        }
    }
}