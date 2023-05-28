using Newtonsoft.Json;
using PlaningPoker.Api.Test.PlaningHub.Fixtures;
using PlaningPoker.Api.Test.Startup;

namespace PlaningPoker.Api.Test.Games.Features
{
    public class GameCreateFeature
    {
        public const string RequestUriBase = "Game";
        public const string PathJson = "./Games/Fixtures/Game.json";
        private PlaningPokerClient client;

        [SetUp]
        public void Setup()
        {
            client = new PlaningPokerClient(new SetupFixture().CreateClient());
        }

        [Test]
        public async Task RetrieveAGameAfterPostAsync()
        {
            var json = await client.GetJsonContent(PathJson);

            var responsePost = await client.Post(RequestUriBase, json);
            var gameId = responsePost.Content.ReadAsStringAsync().Result;

            var response = await client.Get($"{RequestUriBase}/{gameId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);

            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}