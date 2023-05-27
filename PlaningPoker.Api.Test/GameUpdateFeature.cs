using Newtonsoft.Json;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameUpdateFeature
    {
        private PlaningPokerClient client;

        [SetUp]
        public void Setup()
        {
            client = new PlaningPokerClient(new SetupFixture().CreateClient());
        }

        [Test]
        public async Task RetrieveAGameWithNewUserAdded()
        {
            var json = await client.GetJsonContent("./Fixtures/Game.json");

            var responsePost = await client.Post("Game", json);
            var gameId = responsePost.Content.ReadAsStringAsync().Result;

            var userAddDto = new UsersAddDto("Pedro", gameId);
            var serializedDto = JsonConvert.SerializeObject(userAddDto, Formatting.Indented);
            var responsePut = await client.Put($"Game/{gameId}", serializedDto);

            var content = await responsePut.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);

            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}