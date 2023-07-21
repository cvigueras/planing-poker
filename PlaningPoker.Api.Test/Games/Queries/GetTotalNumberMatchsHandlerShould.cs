using FluentAssertions;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Games.Queries
{
    public class GetTotalNumberMatchsHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IGameRepository repository;
        private GetTotalNumberMatchsQueryHandler handle;

        [SetUp]
        public void SetUp()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new GameRepository(connection);
            handle = new GetTotalNumberMatchsQueryHandler(repository);
        }

        [Test]
        public async Task ReturnTotalNumberMatchsSuccesfullyAsync()
        {
            var result = await handle.Handle(new GetTotalNumberMatchsQuery(), default);

            result.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}
