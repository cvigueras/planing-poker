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
        private Guid gameGuid;

        [SetUp]
        public void SetUp()
        {
            guidGenerator = Substitute.For<IGuidGenerator>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connection);
            gameRepository = new GameRepository(connection);
            gameGuid = new GuidGenerator().Generate();
            mapper = setupFixture.AutoMapperConfigTest(gameGuid);
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

            var action = await gameController.Post(givenGame);
            var result = action as OkObjectResult;

            result.StatusCode.Should().Be(StatusCodes.Status200OK);
            result.Value.Should().BeEquivalentTo(gameGuid.ToString());
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user = User.Create(givenGame.CreatedBy, givenGame.Id);
            await userRepository.Add(user);

            var action = await gameController.Get(givenGame.Id);
            var result = action as OkObjectResult;

            var expectedGame = new GameReadDto(givenGame.Id, "Carlos", "Release1", "Session for Release1", 60, 60);
            result.Value.Should().BeEquivalentTo(expectedGame);
        }

        [Test]
        public async Task RetrieveAGameUpdatedWithNewUser()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user = User.Create(givenGame.CreatedBy, givenGame.Id);
            await userRepository.Add(user);
            var userAddedDto = new UsersAddDto("Juan", givenGame.Id);

            var result = gameController.Put(givenGame.Id, userAddedDto);
            var userResult = await result as OkObjectResult;

            var expectedUser = new GameUsersReadDto(givenGame.Id, givenGame.CreatedBy, givenGame.Title, givenGame.Description, 60, 60, new List<UsersReadDto>
            {
                new (givenGame.CreatedBy, givenGame.Id),
                new (userAddedDto.Name, givenGame.Id)
            });
            userResult.Value.Should().BeEquivalentTo(expectedUser);
        }
    }
}