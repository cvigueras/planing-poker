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
            handler = new GetAllVotesAllUsersByGameIdQueryHandler(voteRepository, mapper);
            mapper = setupFixture.AutoMapperConfigTest();
        }

        [Test]
        public async Task GetAllVotesAllUserByGameIdSuccesfully()
        {
            var givenFirstUser = User.Create("Carlos", gameId, "anyConnectionId", true, Vote.Create("3"));
            await userRepository.Add(givenFirstUser);            
            var givenSecondUser = User.Create("Juan", gameId, "anyConnectionId", true, Vote.Create("5"));
            await userRepository.Add(givenSecondUser);
            
            var result = handler.Handle(query);

            var expectedVotes = new List<UsersVotesReadDto> {
                new UsersVotesReadDto("Carlos","5"),
                new UsersVotesReadDto("Juan","3"),
            };
            result.Should().BeEquivalentTo(expectedVotes);
        }
    }
}
