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
using PlaningPoker.Api.Votes.Models;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Games.Queries
{
    public class GetGameByGuidQueryHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IGameRepository gameRepository;
        private GetGameByGuidQueryHandler handle;
        private IGuidGenerator guidGenerator;
        private IUserRepository userRepository;

        [SetUp]
        public void SetUp()
        {
            guidGenerator = Substitute.For<IGuidGenerator>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            gameRepository = new GameRepository(connection);
            handle = new GetGameByGuidQueryHandler(gameRepository, mapper);
            userRepository = new UserRepository(connection);
            mapper = AutoMapperProfileStartup.AutoMapperConfig();
        }

        [Test]
        public async Task RetrieveAnErrorWhenNonExistingGame()
        {
            var guid = guidGenerator.Generate().ToString();

            var queryGame =
                new GetGameByGuidQuery(guid, Enumerable.Empty<UsersReadDto>(), Enumerable.Empty<CardReadDto>());
            var action = async () => await handle.Handle(queryGame, default);

            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        [Test]
        public async Task RetrieveAGameWhenExists()
        {
            var givenGame = GameMother.CarlosAsGame();
            await gameRepository.Add(givenGame);
            var user = User.Create(givenGame.CreatedBy, givenGame.Id, string.Empty, false, Vote.Create(string.Empty));
            await userRepository.Add(user);
            var cards = CardMother.GetAll();
            var expectedListUser = new List<UsersReadDto>
            {
                new(givenGame.CreatedBy, givenGame.Id, false, null),
            };
            var queryGame =
                new GetGameByGuidQuery(givenGame.Id, expectedListUser, cards);
            var result = await handle.Handle(queryGame, default);

            var expectedGame = new GameReadDto(givenGame.Id, givenGame.CreatedBy, givenGame.Title, givenGame.Description, 60, 60, expectedListUser, cards);
            result.Should().BeEquivalentTo(expectedGame);
        }
    }
}