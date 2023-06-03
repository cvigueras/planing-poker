using AutoMapper;
using FluentAssertions;
using Microsoft.AspNet.SignalR.Client;
using NSubstitute;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Cards.Fixtures;
using PlaningPoker.Api.Users.Repositories;
using System.Data.SQLite;
using PlaningPoker.Api.Test.Startup;

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
            mapper = setupFixture.AutoMapperConfigTest();
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
