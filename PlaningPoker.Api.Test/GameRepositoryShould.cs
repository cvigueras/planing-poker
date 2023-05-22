using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {
        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var setupFixture = new SetupFixture();
            var connection = setupFixture.Get();
            var gameRepository = new GameRepository(connection);
            var guid = Guid.NewGuid().ToString();

            var action = () => gameRepository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }
    }
}