using FluentAssertions;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Test.Games.Queries;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Repositories;
using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlaningPoker.Api.Test.Users.Queries
{
    public class GetTotalNumberUserssHandlerShould
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
