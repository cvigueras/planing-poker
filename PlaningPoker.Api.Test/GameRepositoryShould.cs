using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {
        [Test]
        public void ReturnNullWhenNotExistsAGame()
        {
            var setupFixture = new SetupFixture();
            var connection = setupFixture.Get();
            var gameRepository = new GameRepository(connection);
            var guid = Guid.NewGuid().ToString();

            var result = gameRepository.GetByGuid(guid);

            result.Should().BeNull();
        }
    }
}
