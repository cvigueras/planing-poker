using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using PlaningPoker.Api.Test.Startup;
using webapi;
using webapi.Controllers;

namespace PlaningPoker.Api.Test
{
    public class GameControllerShould
    {

        [Test]
        public void RetrieveAnErrorWhenNonExistingGame()
        {
            var mapper = Substitute.For<IMapper>();
            var gameRepository = Substitute.For<IGameRepository>();
            var gameController = new GameController(gameRepository,mapper);
            var guid = Guid.NewGuid().ToString();


            gameRepository.GetByGuid(guid)!.Returns((Game)null);
            var actionResult = gameController.Get(guid);
            var result = actionResult.Result as NotFoundResult;

            result.StatusCode.Should().Be(404);
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var mapper = Substitute.For<IMapper>();
            var guid = Guid.NewGuid().ToString();
            
            var gameRepository = Substitute.For<IGameRepository>();
            var gameController = new GameController(gameRepository, mapper);
            var givenGame = new Game
            {
                Guid = guid,
                CreatedBy = "Carlos",
                Description = "Point Poker to release1",
                Expiration = 60,
                RoundTime = 60,
                Title = "Release1",
            };
            gameRepository.Add(givenGame).Returns(guid);
            gameRepository.GetByGuid(guid).Returns(givenGame);
            var expectedGame = new GameReadDto(guid, "Carlos", "Release1", "Session for Release1", 60, 60);
            mapper.Map<GameReadDto>(Arg.Is(givenGame)).Returns(expectedGame);

            var action = await gameController.Get(guid);
            var result = action as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedGame);
        }

    }
}
