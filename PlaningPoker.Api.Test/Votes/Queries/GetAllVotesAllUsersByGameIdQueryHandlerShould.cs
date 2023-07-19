using AutoMapper;
using FluentAssertions;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Queries;
using PlaningPoker.Api.Votes.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Votes.Queries
{
    public class GetAllVotesAllUsersByGameIdQueryHandlerShould
    {

        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IVoteRepository voteRepository;
        private IUserRepository userRepository;
        private GetAllVotesAllUsersByGameIdQuery query;
        private GetAllVotesAllUsersByGameIdQueryHandler handler;
        string gameId = "anyGameId";

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            voteRepository = new VoteRepository(connection);
            userRepository = new UserRepository(connection);
            query = new GetAllVotesAllUsersByGameIdQuery(gameId);
            mapper = setupFixture.AutoMapperConfigTest();
            handler = new GetAllVotesAllUsersByGameIdQueryHandler(voteRepository, mapper);
        }

        [Test]
        public async Task GetAllVotesAllUserByGameIdSuccesfully()
        {
            var givenFirstUser = User.Create("Carlos", gameId, "anyConnectionId", true, Vote.Create("3"));
            await userRepository.Add(givenFirstUser);            
            var givenSecondUser = User.Create("Juan", gameId, "anyConnectionId", true, Vote.Create("5"));
            await userRepository.Add(givenSecondUser);
            
            var result = await handler.Handle(query, default);

            var expectedFirstVote = new VotesUsersReadDto("Carlos", "3");
            var expectedSecondVote = new VotesUsersReadDto("Juan", "5");

            result.ElementAt(0).Name.Should().BeEquivalentTo(expectedFirstVote.Name);
            result.ElementAt(0).Value.Should().BeEquivalentTo(expectedFirstVote.Value);
            result.ElementAt(1).Name.Should().BeEquivalentTo(expectedSecondVote.Name);
            result.ElementAt(1).Value.Should().BeEquivalentTo(expectedSecondVote.Value);
        }
    }
}
