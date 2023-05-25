using System.Data.SQLite;
using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class UserRepositoryShould
    {
        private UserRepository userRepository;
        private SQLiteConnection connection;
        private SetupFixture setupFixture;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connection);
        }

        [Test]
        public async Task FailWhenRetrieveANonExistingUser()
        {
            var guid = Guid.NewGuid().ToString();

            var action = async () => await userRepository.GetById(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}
