using AutoMapper;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Test.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;
using webapi;

namespace PlaningPoker.Api.Test
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
            var action= async () => await getGameByGuidQueryHandler.Handle(queryGame, default);

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