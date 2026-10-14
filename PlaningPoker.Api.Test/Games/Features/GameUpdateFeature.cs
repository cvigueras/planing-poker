using FluentAssertions;
using Newtonsoft.Json;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Test.PlaningHub.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using System.Net;

namespace PlaningPoker.Api.Test.Games.Features
{
    public class GameUpdateFeature
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
        public async Task RetrieveAGameWithNewUserAdded()
        {
            var json = await client.GetJsonContent(PathJson);

            var responsePost = await client.Post(RequestUriBase, json);
            var game = responsePost.Content.ReadAsStringAsync().Result;
            var gameResult = JsonConvert.DeserializeObject<GameReadDto>(game);

            var userAddDto = new UsersAddDto("Pedro", gameResult.Id, false);
            var serializedDto = JsonConvert.SerializeObject(userAddDto, Formatting.Indented);
            var responsePut = await client.Put($"{RequestUriBase}/{gameResult.Id}", serializedDto);

            var content = await responsePut.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);

            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }

        [Test]
        public async Task ReturnNotFoundAndNotCreateTheUserWhenTheGameDoesNotExist()
        {
            var fixture = new SetupFixture();
            client = new PlaningPokerClient(fixture.CreateClient());
            var userRepository = new UserRepository(fixture.GetSQLiteConnection());
            var totalUsersBefore = await userRepository.GetTotalNumberUsers();

            var userAddDto = new UsersAddDto("Pedro", "nonExistingGame", false);
            var serializedDto = JsonConvert.SerializeObject(userAddDto, Formatting.Indented);
            var action = async () => await client.Put($"{RequestUriBase}/nonExistingGame", serializedDto);

            // The test client throws when the response is not successful.
            await action.Should().ThrowAsync<HttpRequestException>().Where(e => e.StatusCode == HttpStatusCode.NotFound);
            (await userRepository.GetTotalNumberUsers()).Should().Be(totalUsersBefore);
        }
    }
}