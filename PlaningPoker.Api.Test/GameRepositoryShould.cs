using FluentAssertions;
using NSubstitute;
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
        private GameRepository repository;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new GameRepository(connection);
            guidGenerator = Substitute.For<IGuidGenerator>();
        }

        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var guid = guidGenerator.Generate().ToString();

            var action = () => repository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingGame()
        {
            var expectedCreatedBy = "Carlos";
            var expectedTitle = "Release1";
            var expectedDescription = "Point Poker to release1";
            var expectedRoundTime = 60;
            var expectedExpiration = 60;
            var givenGame = GameMother.CarlosAsGame();
            await repository.Add(givenGame);

            var result = await repository.GetByGuid(givenGame.Id);

            result.CreatedBy.Should().Be(expectedCreatedBy);
            result.Title.Should().Be(expectedTitle);
            result.Description.Should().Be(expectedDescription);
            result.RoundTime.Should().Be(expectedRoundTime);
            result.Expiration.Should().Be(expectedExpiration);
        }
    }
}