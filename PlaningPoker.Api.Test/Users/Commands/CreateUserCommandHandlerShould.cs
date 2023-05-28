using AutoMapper;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi.Cards.Queries;
using webapi.Cards.Repositories;
using webapi.Games.Commands;
using webapi.Games.Models;
using webapi.Games.Queries;
using webapi.Games.Repositories;
using webapi.Helpers;
using webapi.Users.Models;
using webapi.Users.Queries;
using webapi.Users.Repositories;

namespace PlaningPoker.Api.Test.Users.Commands
{
    public class CreateUserCommandHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IUserRepository userRepository;
        private CreateGameCommandHandler createGameCommandHandler;
        private GetGameByGuidQueryHandler getGameByGuidQueryHandler;
        private GetUsersGameByGameIdQueryHandler getUsersGameByGameIdQueryHandler;
        private GetAllCardsListQueryHandler getAllCardsListQueryHandler;
        private IGameRepository gameRepository;
        private ICardRepository cardRepository;
        private Guid gameGuid;

        [SetUp]
        public void SetUp()
        {
            Substitute.For<IGuidGenerator>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connection);
            gameRepository = new GameRepository(connection);
            cardRepository = new CardRepository(connection);
            gameGuid = new GuidGenerator().Generate();
            mapper = setupFixture.AutoMapperConfigTest(gameGuid);
            getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository, mapper);
            getUsersGameByGameIdQueryHandler = new GetUsersGameByGameIdQueryHandler(userRepository, mapper);
            getAllCardsListQueryHandler = new GetAllCardsListQueryHandler(cardRepository, mapper);
            createGameCommandHandler = new CreateGameCommandHandler(gameRepository, mapper);
        }

        [Test]
        public async Task PostGameSuccessFully()
        {
            var givenGame = new GameCreateDto("Carlos", "Release1", "Session for Release1", 60, 60);

            var gameQuery = new CreateGameCommand(givenGame);
            var result = await createGameCommandHandler.Handle(gameQuery, default);

            result.Should().BeEquivalentTo(gameGuid.ToString());
        }

        [Test]
        public async Task RetrieveAGameUpdatedWithNewUser()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var userCreate = User.Create(givenGame.CreatedBy, givenGame.Id);
            await userRepository.Add(userCreate);
            var newUser = User.Create("Juan", givenGame.Id);
            await userRepository.Add(newUser);

            var usersReadDto = await getUsersGameByGameIdQueryHandler.Handle(new GetUsersGameByGameIdQuery(givenGame.Id), default);
            var cardDtoList = await getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
            var result = await getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(givenGame.Id, usersReadDto, cardDtoList), default);

            var expectedGame = new GameReadDto(givenGame.Id,
                givenGame.CreatedBy,
                givenGame.Title,
                givenGame.Description,
                60,
                60,
                usersReadDto.ToList(),
                cardDtoList.ToList());
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}
