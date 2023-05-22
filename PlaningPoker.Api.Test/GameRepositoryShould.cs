﻿using System.Data.SQLite;
using FluentAssertions;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private GameRepository gameRepository;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.Get();
            gameRepository = new GameRepository(connection);
        }

        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var guid = Guid.NewGuid().ToString();

            var action = () => gameRepository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            var newGuid = await gameRepository.Add(givenGame);
            givenGame.Guid = newGuid;

            var result = await gameRepository.GetByGuid(newGuid);

            var expectedGame = GameMother.CarlosAsGame();
            expectedGame.Guid = newGuid;
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}