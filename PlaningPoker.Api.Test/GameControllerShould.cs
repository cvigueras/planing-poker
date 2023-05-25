using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi;
using webapi.Controllers;

namespace PlaningPoker.Api.Test
{
    public class GameControllerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IGameRepository gameRepository;
        private GameController gameController;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void SetUp()
        {
            guidGenerator = Substitute.For<IGuidGenerator>();
            mapper = Substitute.For<IMapper>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            gameRepository = new GameRepository(connection);
            gameController = new GameController(gameRepository, mapper, guidGenerator);
        }

        [Test]
        public void RetrieveAnErrorWhenNonExistingGame()
        {
            var guid = guidGenerator.Generate().ToString();

            var actionResult = gameController.Get(guid);
            var result = actionResult.Result as NotFoundObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
            result.Value.Should().Be("Sequence contains no elements");
        }

        [Test]
        public async Task PostGameSuccessFully()
        {
            var givenGame = new GameCreateDto("Carlos", "Release1", "Session for Release1", 60, 60);
            var game = GameMother.CarlosAsGame();
            var guid = guidGenerator.Generate();
            guidGenerator.Generate().Returns(guid);
            mapper.Map<Game>(Arg.Is(givenGame)).Returns(game);

            var action = await gameController.Post(givenGame);
            var result = action as OkObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(guid.ToString());
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var guid = Guid.Parse("4c1bf4dc-143f-451b-b5ad-495fb849794c");
            var givenGame = GameMother.CarlosAsGame();
            givenGame.Id = guid.ToString();
            var expectedGame = new GameReadDto(guid.ToString(), "Carlos", "Release1", "Session for Release1", 60, 60);
            mapper.Map<GameReadDto>(Arg.Any<Game>()).Returns(expectedGame);
            await gameRepository.Add(givenGame);

            var action = await gameController.Get(guid.ToString());
            var result = action as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedGame);
        }
    }
}