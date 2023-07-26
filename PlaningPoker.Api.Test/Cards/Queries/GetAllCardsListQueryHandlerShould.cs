using AutoMapper;
using FluentAssertions;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Test.Cards.Fixtures;
using PlaningPoker.Api.Test.Startup;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Cards.Queries
{
    public class GetAllCardsListQueryHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private ICardRepository cardRepository;
        private GetAllCardsListQueryHandler handler;

        [SetUp]
        public void SetUp()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            mapper = AutoMapperProfileStartup.AutoMapperConfig();
            cardRepository = new CardRepository(connection);
            handler = new GetAllCardsListQueryHandler(cardRepository, mapper);
        }

        [Test]
        public async Task RetrieveListCardsSuccessfully()
        {
            var query = new GetAllCardsListQuery();

            var result = await handler.Handle(query, default);

            var expectedListCards = CardMother.GetAll();
            result.Should().BeEquivalentTo(expectedListCards);
        }
    }
}
