using AutoMapper;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Commands;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Test.Users.Fixtures;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Queries;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using System.Data.SQLite;

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
            mapper = AutoMapperProfileStartup.AutoMapperConfig(gameGuid);
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
            var userCreate = User.Create(givenGame.CreatedBy, givenGame.Id, string.Empty, false, Vote.Create("3"));
            await userRepository.Add(userCreate);
            var newUser = User.Create("Juan", givenGame.Id, string.Empty, false, Vote.Create("3"));
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

        [Test]
        public async Task InsertNewVoteSuccesfullyAsync()
        {
            var givenUser = User.Create("Carlos","anyGameId", "anyConnectionId", true, Vote.Create(string.Empty));
            await userRepository.Add(givenUser);
            givenUser.Vote = Vote.Create("3");
            await userRepository.UpdateByConnectionId(givenUser, givenUser.ConnectionId);

            var expectedUser = UserMother.GetUserWithValidVote();
            var result = await userRepository.GetByConnectionId(givenUser.ConnectionId);

            result.Should().BeEquivalentTo(expectedUser);
        }
    }
}
