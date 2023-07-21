using FluentAssertions;
using Newtonsoft.Json;
using PlaningPoker.Api.Test.PlaningHub.Fixtures;
using PlaningPoker.Api.Test.Startup;

namespace PlaningPoker.Api.Test.Games.Features
{
    public class GameInfoFeature
    {
        public const string RequestUriBase = "Game/GetNumberMatchs";
        private PlaningPokerClient client;

        [SetUp]
        public void Setup()
        {
            client = new PlaningPokerClient(new SetupFixture().CreateClient());
        }

        [Test]
        public async Task RetrieveNumberTotalMatchsAfterGetAsync()
        {
            var responsePost = await client.Get(RequestUriBase);
            var game = responsePost.Content.ReadAsStringAsync().Result;
            var gameResult = JsonConvert.DeserializeObject<long>(game);

            gameResult.Should().BeGreaterThanOrEqualTo(0);
        }
    }
}
