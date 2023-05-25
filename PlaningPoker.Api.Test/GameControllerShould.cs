using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PlaningPoker.Api.Test.Fixtures;
using webapi;
using webapi.Controllers;

namespace PlaningPoker.Api.Test
{
    public class GameControllerShould
    {
        private IMapper mapper;
        private IGameRepository gameRepository;
        private GameController gameController;
        private IGuidGenerator guidGenerator;

        [SetUp]
        public void SetUp()
        {
            guidGenerator = Substitute.For<IGuidGenerator>();
            mapper = Substitute.For<IMapper>();
            gameRepository = Substitute.For<IGameRepository>();
            gameController = new GameController(gameRepository, mapper, guidGenerator);
        }

        [Test]
        public void RetrieveAnErrorWhenNonExistingGame()
        {
            var guid = guidGenerator.Generate().ToString();
            gameRepository.GetByGuid(guid)!.Returns((Game)null);
            var actionResult = gameController.Get(guid);

            var result = actionResult.Result as NotFoundResult;

            result.StatusCode.Should().Be(StatusCodes.Status404NotFound);
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

            await gameRepository.Received(1).Add(game);
            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(guid.ToString());
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var guid = guidGenerator.Generate();
            var givenGame = GameMother.CarlosAsGame();
            gameRepository.GetByGuid(guid.ToString()).Returns(givenGame);
            var expectedGame = new GameReadDto(guid.ToString(), "Carlos", "Release1", "Session for Release1", 60, 60);
            mapper.Map<GameReadDto>(Arg.Is(givenGame)).Returns(expectedGame);

            var action = await gameController.Get(guid.ToString());
            var result = action as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedGame);
        }
    }
}