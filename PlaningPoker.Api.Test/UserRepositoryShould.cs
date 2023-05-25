using System.Data.SQLite;
using FluentAssertions;
using PlaningPoker.Api.Test.Startup;

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
        public void FailWhenRetrieveANonExistingUser()
        {
            var guid = Guid.NewGuid().ToString();

            var action = () => userRepository.GetById(guid);

            action.Should().Throw<ArgumentNullException>();
        }
    }

    public class UserRepository
    {
        private readonly SQLiteConnection _connection;

        public UserRepository(SQLiteConnection connection)
        {
            _connection = connection;
        }

        public object GetById(string id)
        {
            throw new NotImplementedException();
        }
    }
}
