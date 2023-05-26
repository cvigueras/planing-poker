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
        private IUserRepository userRepository;
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
            userRepository = Substitute.For<IUserRepository>();
            gameRepository = new GameRepository(connection);
            gameController = new GameController(gameRepository, userRepository, mapper);
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
            //var guid = guidGenerator.Generate();
            //guidGenerator.Generate().Returns(guid);
            mapper.Map<Game>(Arg.Is(givenGame)).Returns(game);

            var action = await gameController.Post(givenGame);
            var result = action as OkObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(game.Id);
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var expectedGame = new GameReadDto(givenGame.Id, "Carlos", "Release1", "Session for Release1", 60, 60);
            mapper.Map<GameReadDto>(Arg.Any<Game>()).Returns(expectedGame);

            var action = await gameController.Get(givenGame.Id);
            var result = action as OkObjectResult;

            result.Value.Should().BeEquivalentTo(expectedGame);
        }

        [Test]
        public async Task RetrieveAGameUpdatedWithNewUser()
        {
            var userId = guidGenerator.Generate().ToString();
            var givenGame = GameMother.CarlosAsGame();
            var newUserName = "Juan";
            var expectedUsers = new List<UsersReadDto>
            {
                new (guidGenerator.Generate().ToString(), givenGame.CreatedBy, givenGame.Id),
                new (guidGenerator.Generate().ToString(), newUserName, givenGame.Id)
            };
            await gameRepository.Add(givenGame);

            var result = gameController.Put(givenGame.Id, newUserName);

            var userResult = await result as OkObjectResult;
            var expectedUser = new GameUsersReadDto(givenGame.Id, "Carlos", "Release1", "Session for Release1", 60, 60, expectedUsers); 
            userResult.Value.Should().BeEquivalentTo(expectedUser);
        }
    }
}