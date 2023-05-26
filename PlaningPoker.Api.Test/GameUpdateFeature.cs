using System.Text;
using Newtonsoft.Json;
using PlaningPoker.Api.Test.Startup;

namespace PlaningPoker.Api.Test
{
    public class GameUpdateFeature
    {
        private HttpClient? _client;
        private const string MediaType = "application/json";

        [SetUp]
        public void Setup()
        {
            _client = new SetupFixture().CreateClient();
        }

        [Test]
        public async Task RetrieveAGameWithNewUserAdded()
        {
            using var jsonReaderPost = new StreamReader("./Fixtures/Game.json");
            var jsonPost = await jsonReaderPost.ReadToEndAsync();

            var responsePost = await _client!.PostAsync("Game", new StringContent(jsonPost,
                Encoding.Default,
                MediaType));
            responsePost.EnsureSuccessStatusCode();
            var gameId = responsePost.Content.ReadAsStringAsync().Result;

            var newUserInGame = "Pedro";
            var responsePut = await _client!.PutAsync($"Game/{gameId}", new StringContent(newUserInGame));
            responsePut.EnsureSuccessStatusCode();

            var content = await responsePut.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);
            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}