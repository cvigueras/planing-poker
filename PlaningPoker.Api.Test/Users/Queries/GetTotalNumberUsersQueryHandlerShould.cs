using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Users.Queries
{
    public class GetTotalNumberUsersHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IUserRepository repository;
        private GetTotalNumberUsersQueryHandler handle;

        [SetUp]
        public void SetUp()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new UserRepository(connection);
            handle = new GetTotalNumberUsersQueryHandler(repository);
        }

        [Test]
        public async Task ReturnTotalNumberUsersSuccesfullyAsync()
        {
            var result = await handle.Handle(new GetTotalNumberUsersQuery(), default);

            result.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}
