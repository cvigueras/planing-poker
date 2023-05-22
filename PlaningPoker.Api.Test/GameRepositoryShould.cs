using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {

        [SetUp]
        public void Setup()
        {

        }

        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var setupFixture = new SetupFixture();
            var connection = setupFixture.Get();
            var guid = Guid.NewGuid().ToString();

            var gameRepository = new GameRepository(connection);
            var action = () => gameRepository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingGame()
        {
            var setupFixture = new SetupFixture();
            var connection = setupFixture.Get();
            var gameRepository = new GameRepository(connection);
            var givenGame = new Game
            {
                CreatedBy = "Carlos",
                Description = "Point Poker to release1",
                Expiration = 60,
                RoundTime = 60,
                Title = "Release1",
            };
            var newGuid = await gameRepository.Add(givenGame);
            givenGame.Guid = newGuid;

            var result = await gameRepository.GetByGuid(newGuid);

            var expectedGame = new Game
            {
                Guid = newGuid,
                CreatedBy = "Carlos",
                Description = "Point Poker to release1",
                Expiration = 60,
                RoundTime = 60,
                Title = "Release1",
            };
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}