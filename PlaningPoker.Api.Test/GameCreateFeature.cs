using Newtonsoft.Json;
using PlaningPoker.Api.Test.Startup;
using System.Text;

namespace PlaningPoker.Api.Test
{
    public class GameCreateFeature
    {
        private PlaningPokerClient client;

        [SetUp]
        public void Setup()
        {
            client = new PlaningPokerClient(new SetupFixture().CreateClient());
        }

        [Test]
        public async Task RetrieveAGameAfterPostAsync()
        {
            using var jsonReader = new StreamReader("./Fixtures/Game.json");
            var json = await jsonReader.ReadToEndAsync();

            var responsePost = await client.Post("Game", json);
            var gameId = responsePost.Content.ReadAsStringAsync().Result;

            var response = await client.Get($"Game/{gameId}");
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);
            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }


    }
}