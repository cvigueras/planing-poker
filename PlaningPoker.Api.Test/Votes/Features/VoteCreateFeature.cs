using PlaningPoker.Api.Test.PlaningHub.Fixtures;
using PlaningPoker.Api.Test.Startup;

namespace PlaningPoker.Api.Test.Votes.Features
{
    public class CreateVoteFeature
    {
        public const string RequestUriBase = "Votes";
        public const string PathJson = "./Votes/Fixtures/Vote.json";
        private PlaningPokerClient client;

        [SetUp]
        public void Setup()
        {
            client = new PlaningPokerClient(new SetupFixture().CreateClient());
        }

        [Test]
        public async Task InsertVoteSuccessfully()
        {
            var json = await client.GetJsonContent(PathJson);

            var response = await client.Post(RequestUriBase, json);
            
            response.EnsureSuccessStatusCode();
        }
    }
}
