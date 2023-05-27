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
            userRepository = new UserRepository(connection);
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
            var givenGame = GameMother.CarlosAsGame();
            var guidUser = Guid.Parse("49c4d829-b7e7-45ba-8db0-9da9eaee4388").ToString();
            var user = User.Create(guidUser, givenGame.CreatedBy, givenGame.Id);
            await userRepository.Add(user);
            await gameRepository.Add(givenGame);

            var userAddedDto = new UsersAddDto("Juan", givenGame.Id);
            var expectedUsers = new List<UsersReadDto>
            {
                new (guidUser, givenGame.CreatedBy, givenGame.Id),
                new (guidGenerator.Generate().ToString(), userAddedDto.Name, givenGame.Id)
            };
            mapper.Map<List<UsersReadDto>>(Arg.Any<List<User>>()).Returns(expectedUsers);

            var result = gameController.Put(givenGame.Id, userAddedDto);

            var userResult = await result as OkObjectResult;
            var expectedUser = new GameUsersReadDto(givenGame.Id, givenGame.CreatedBy, givenGame.Title, givenGame.Description, 60, 60, expectedUsers);
            userResult.Value.Should().BeEquivalentTo(expectedUser);
        }
    }
}