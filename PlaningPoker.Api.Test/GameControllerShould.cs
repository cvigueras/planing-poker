using AutoMapper;
using FluentAssertions;
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
        private IGameRepository? gameRepository;
        private GameController gameController;
        private string guid;

        [SetUp]
        public void SetUp()
        {
            mapper = Substitute.For<IMapper>();
            gameRepository = Substitute.For<IGameRepository>();
            gameController = new GameController(gameRepository,mapper);
            guid = Guid.NewGuid().ToString();
        }

        [Test]
        public void RetrieveAnErrorWhenNonExistingGame()
        {
            gameRepository.GetByGuid(guid)!.Returns((Game)null);
            var actionResult = gameController.Get(guid);

            var result = actionResult.Result as NotFoundResult;

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var givenGame = GameMother.CarlosAsGame();
            givenGame.Guid = guid;
            gameRepository.GetByGuid(guid).Returns(givenGame);
            var expectedGame = new GameReadDto(guid, "Carlos", "Release1", "Session for Release1", 60, 60);
            mapper.Map<GameReadDto>(Arg.Is(givenGame)).Returns(expectedGame);

            var action = await gameController.Get(guid);
            var result = action as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedGame);
        }

    }
}
