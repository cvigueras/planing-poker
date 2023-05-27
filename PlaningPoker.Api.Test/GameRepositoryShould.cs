﻿using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameRepositoryShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private GameRepository repository;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            repository = new GameRepository(connection);
            guidGenerator = Substitute.For<IGuidGenerator>();
        }

        [Test]
        public async Task ReturnExceptionWhenNotExistsAGame()
        {
            var guid = guidGenerator.Generate().ToString();

            var action = () => repository.GetByGuid(guid);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAnExistingGame()
        {
            var givenGame = await GivenANewCreatedGame();

            var result = await WhenRetrieveTheGame(givenGame);

            ThenGameShouldBeExpectedGame(givenGame, result);
        }

        private static void ThenGameShouldBeExpectedGame(Game givenGame, Game result)
        {
            var expectedGame = Game.Create(givenGame.Id, "Carlos", "Release1", "Session for Release1", 60, 60);
            result.Should().BeEquivalentTo(expectedGame);
        }

        private async Task<Game> WhenRetrieveTheGame(Game givenGame)
        {
            var result = await repository.GetByGuid(givenGame.Id);
            return result;
        }

        private async Task<Game> GivenANewCreatedGame()
        {
            var givenGame = GameMother.CarlosAsGame();
            await repository.Add(givenGame);
            return givenGame;
        }
    }
}