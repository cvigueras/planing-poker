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

            var responsePost= await _client!.PostAsync("Games",new StringContent(json,
                                                               Encoding.Default,
                                                               MediaType));
            var resultGuid = responsePost.Content.ReadAsStringAsync().Result;
            responsePost.EnsureSuccessStatusCode();

            var response = await _client!.GetAsync($"Games/{resultGuid}");
            response.EnsureSuccessStatusCode();
            var result = response.Content.ReadAsStringAsync().Result;

            await Verify(result);
        }
    }
}