using System.Net.Http.Headers;
using System.Text;
using Newtonsoft.Json;
using NSubstitute;
using PlaningPoker.Api.Test.Startup;
using webapi;

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

            var userAddDto = new UsersAddDto("Pedro", gameId);
            var serializedDto = JsonConvert.SerializeObject(userAddDto, Formatting.Indented);
            var responsePut = await _client!.PutAsync($"Game/{gameId}", new StringContent(serializedDto, Encoding.Default,
                MediaType));
            responsePut.EnsureSuccessStatusCode();

            var content = await responsePut.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);
            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}