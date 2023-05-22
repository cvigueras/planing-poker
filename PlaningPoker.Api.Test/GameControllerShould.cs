using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using webapi;
using webapi.Controllers;

namespace PlaningPoker.Api.Test
{
    public class GameControllerShould
    {

        [Test]
        public void RetrieveAnErrorWhenNonExistingGame()
        {
            var gameController = new GameController();
            var guid = Guid.NewGuid().ToString();

            var gameRepository = Substitute.For<IGameRepository>();
            gameRepository.GetByGuid(guid).Returns(null);
            var actionResult = gameController.Get(guid);
            var result = actionResult.Result as NotFoundResult;

            result.StatusCode.Should().Be(404);
        }

    }
}
