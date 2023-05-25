using FluentAssertions;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private GameRepository gameRepository;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            gameRepository = new GameRepository(connection);
        }

        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var guid = Guid.NewGuid().ToString();

            var action = () => gameRepository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            givenGame.Id = Guid.NewGuid().ToString();
            await gameRepository.Add(givenGame);

            var result = await gameRepository.GetByGuid(givenGame.Id);

            var expectedGame = GameMother.CarlosAsGame();
            expectedGame.Id = result.Id;
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}