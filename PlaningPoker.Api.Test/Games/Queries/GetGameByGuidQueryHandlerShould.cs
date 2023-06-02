using AutoMapper;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Cards.Fixtures;
using PlaningPoker.Api.Test.Games.Fixtures;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Games.Queries
{
    public class GetGameByGuidQueryHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IGameRepository gameRepository;
        private GetGameByGuidQueryHandler getGameByGuidQueryHandler;
        private IGuidGenerator guidGenerator;
        private Guid gameGuid;
        private IUserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            guidGenerator = Substitute.For<IGuidGenerator>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            gameRepository = new GameRepository(connection);
            getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository, mapper);
            userRepository = new UserRepository(connection);
            gameGuid = new GuidGenerator().Generate();
            mapper = setupFixture.AutoMapperConfigTest(gameGuid);
        }

        [Test]
        public async Task RetrieveAnErrorWhenNonExistingGame()
        {
            var guid = guidGenerator.Generate().ToString();

            var queryGame =
                new GetGameByGuidQuery(guid, Enumerable.Empty<UsersReadDto>(), Enumerable.Empty<CardReadDto>());
            var action = async () => await getGameByGuidQueryHandler.Handle(queryGame, default);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user = User.Create(givenGame.CreatedBy, givenGame.Id);
            await userRepository.Add(user);
            var cards = CardMother.GetAll();
            var expectedListUser = new List<UsersReadDto>
            {
                new(givenGame.CreatedBy, givenGame.Id),
            };
            var queryGame =
                new GetGameByGuidQuery(givenGame.Id, expectedListUser, cards);
            var result = await getGameByGuidQueryHandler.Handle(queryGame, default);

            var expectedGame = new GameReadDto(givenGame.Id, givenGame.CreatedBy, givenGame.Title, givenGame.Description, 60, 60, expectedListUser, cards);
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}