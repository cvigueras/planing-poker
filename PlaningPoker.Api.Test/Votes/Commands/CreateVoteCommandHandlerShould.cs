using AutoMapper;
using FluentAssertions;
using NSubstitute;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Commands;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Queries;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Votes.Commands
{
    public class CreateVoteCommandHandlerShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connection;
        private IMapper mapper;
        private IVoteRepository voteRepository;
        private IUserRepository userRepository;
        private CreateVoteCommandHandler handler;

        [SetUp]
        public void SetUp()
        {
            Substitute.For<IGuidGenerator>();
            setupFixture = new SetupFixture();
            connection = setupFixture.GetSQLiteConnection();
            voteRepository = new VoteRepository(connection);
            userRepository = new UserRepository(connection);
            mapper = setupFixture.AutoMapperConfigTest();
            handler = new CreateVoteCommandHandler(voteRepository, mapper);
        }

        [Test]
        public async Task InsertAVoteForAUserInAGroupSuccesfully()
        {
            var givenUser = User.Create("Carlos", "anyGameId", "anyConnectionId", true, Vote.Create(""));
            await userRepository.Add(givenUser);
            givenUser.Vote = Vote.Create("3");
            await voteRepository.AddVoteByUserNameAndGroupIdAsync(givenUser.Name, givenUser.GameId, givenUser.Vote.Value);

            var result = await voteRepository.GetAllVotesByGroupIdAsync(givenUser.GameId);

            var expectedUserVote = new VotesUsersReadDto("Carlos", "3");
            result.Single().UserName.Should().BeEquivalentTo(expectedUserVote.Name);
            result.Single().Vote.Value.Should().BeEquivalentTo(expectedUserVote.Value);
        }
    }
}