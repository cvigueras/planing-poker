﻿using Newtonsoft.Json;
using PlaningPoker.Api.Test.PlaningHub.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;

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
            var gameId = responsePost.Content.ReadAsStringAsync().Result;

            var userAddDto = new UsersAddDto("Pedro", gameId);
            var serializedDto = JsonConvert.SerializeObject(userAddDto, Formatting.Indented);
            var responsePut = await client.Put($"{RequestUriBase}/{gameId}", serializedDto);

            var content = await responsePut.Content.ReadAsStringAsync();
            var result = JsonConvert.SerializeObject(JsonConvert.DeserializeObject(content), Formatting.Indented);

            var settings = new VerifySettings();
            settings.ScrubInlineGuids();

            await Verify(result, settings);
        }
    }
}