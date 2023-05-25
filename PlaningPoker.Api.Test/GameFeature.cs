using Newtonsoft.Json;
using PlaningPoker.Api.Test.Startup;
using System.Text;

namespace PlaningPoker.Api.Test
{
    public class GameFeature
    {
        private HttpClient? _client;
        private const string MediaType = "application/json";

        [SetUp]
        public void Setup()
        {
            _client = new SetupFixture().CreateClient();
        }

        [Test]
        public async Task RetrieveAGameAfterPostAsync()
        {
            using var jsonReader = new StreamReader("./Fixtures/Game.json");
            var json = await jsonReader.ReadToEndAsync();

            var responsePost = await _client!.PostAsync("Game", new StringContent(json,
                                                               Encoding.Default,
                                                               MediaType));
            var resultPost = responsePost.Content.ReadAsStringAsync().Result;
            responsePost.EnsureSuccessStatusCode();

            var response = await _client!.GetAsync($"Game/{resultPost}");
            response.EnsureSuccessStatusCode();
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);
            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}