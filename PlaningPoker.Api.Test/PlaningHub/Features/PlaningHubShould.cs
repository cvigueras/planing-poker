using AutoMapper;
using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.PlaningHub.Features
{
    public class PlaningHubShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connectionSql;
        private TestServer server;
        private HubConnection connection;
        private IUserRepository userRepository;
        private IGameRepository gameRepository;
        private IVoteRepository voteRepository;
        private IMapper mapper;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            setupFixture.CreateClient();
            server = setupFixture.Server;
            connectionSql = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connectionSql);
            gameRepository = new GameRepository(connectionSql);
            mapper = Substitute.For<IMapper>();

        }

        [Test]
        public async Task SendMessageToAllSuccessfully()
        {
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var user = string.Empty;
            var message = string.Empty;
            connection.On<string, string>("OnReceiveMessage", (u, m) =>
            {
                user = u;
                message = m;
            });
            await connection.InvokeAsync("SendMessageToAll", "organizer", "Hello World!!");
            await Task.Delay(200);

            user.Should().Be("organizer");
            message.Should().Be("Hello World!!");
        }

        [Test]
        public async Task SendMessageToGroupSuccessfully()
        {
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var user = string.Empty;
            var message = string.Empty;
            connection.On<string>("OnReceiveMessage", (m) =>
            {
                message = m;
            });
            await connection.InvokeAsync("JoinGroup", "group1", "Carlos");
            await connection.InvokeAsync("SendMessageToGroup", "group1", "Hello Group!!");
            await Task.Delay(200);

            message.Should().Be("Hello Group!!");
        }

        [Test]
        public async Task ConsiderJoinToGroupSuccessfully()
        {
            var clients = Substitute.For<IHubCallerClients>();
            var groups = Substitute.For<IGroupManager>();
            var hubContext = Substitute.For<HubCallerContext>();
            var messageHub = new Api.Startup.PlaningHub(userRepository, voteRepository, mapper);
            messageHub.Context = hubContext;
            messageHub.Clients = clients;
            messageHub.Groups = groups;

            await messageHub.JoinGroup("group1", "Carlos");

            await messageHub.Groups.Received(1).AddToGroupAsync(Arg.Any<string>(), Arg.Any<string>(), default);
        }

        [Test]
        public async Task PublishNewUserInGroupSuccessfully()
        {
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var user = "Pedro";
            var isAdmin = false;
            var publishedUser = string.Empty;
            connection.On<string, bool>("OnJoinGroup", (userReceived, admin) =>
            {
                publishedUser = userReceived;
                isAdmin = admin;
            });
            await connection.InvokeAsync("JoinGroup", "group1", "Pedro");
            await Task.Delay(200);

            publishedUser.Should().Be(user);
        }

        [Test]
        public async Task ConsiderAVoteHasBeenAddedSuccessfully()
        {
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            await connection.InvokeAsync("JoinGroup", "group1", "Carlos");

            var userName = "";
            var vote = "";
            var expectedUserName = "Carlos";
            var expectedVote = "3";
            connection.On<string, string>("OnNotifyUserHasVoted", (userReceived, voteReceived) =>
            {
                userName = userReceived;
                vote = voteReceived;
            });
            await connection.InvokeAsync("NotifyUserHasVoted", "group1", expectedUserName, expectedVote);
            await Task.Delay(200);

            userName.Should().Be(expectedUserName);
            vote.Should().Be(expectedVote);
        }

        [Test]
        public async Task ConsiderReceivedAllVotesByGameIdAsync()
        {
            var game = await GIvenAGame();
            await GivenThreeUsers(game);

            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var votesList = new List<VotesUsersReadDto>();
            connection.On<List<VotesUsersReadDto>>("OnReceiveAllVotes", (listVotes) =>
            {
                votesList = listVotes;
            });

            // The votes are sent to the game group, so the connection has to join it first.
            await connection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await connection.InvokeAsync("ReceiveAllVotes", game.Id);
            await Task.Delay(200);

            var expectedVotesList = new List<VotesUsersReadDto>()
            {
                new("Carlos", "anyGameId", true, "3"),
                new("Juan", "anyGameId", false, "5"),
                new("Paco", "anyGameId", false, "8"),
            };
            votesList.Should().BeEquivalentTo(expectedVotesList);
        }

        [Test]
        public async Task KeepTheVoteOfAUserWhenJoiningTheGroupAgain()
        {
            var game = Game.Create("rejoinGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create("2")));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            await connection.InvokeAsync("JoinGroup", game.Id, "Carlos");

            var user = await userRepository.GetByNameAndGameId("Carlos", game.Id);
            user.Vote.Value.Should().Be("2");
        }

        [Test]
        public async Task ResetTheVotesOfTheGameAndNotifyThePlayers()
        {
            var game = Game.Create("resetGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create("2")));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create("5")));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var notified = false;
            connection.On("OnResetVotes", () => notified = true);
            await connection.InvokeAsync("JoinGroup", game.Id, "Carlos");

            await connection.InvokeAsync("ResetVotes", game.Id);
            await Task.Delay(200);

            notified.Should().BeTrue();
            var users = await userRepository.GetUsersGameByGameId(game.Id);
            users.Should().OnlyContain(user => user.Vote.Value == string.Empty);
        }

        [Test]
        public async Task RemoveAPlayerWhoLeavesTheGameAndNotifyTheOthers()
        {
            var game = Game.Create("leaveGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create("5")));
            var adminConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var pedroConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var removedUser = string.Empty;
            adminConnection.On<string>("OnRemoveGroup", name => removedUser = name);
            await adminConnection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await pedroConnection.InvokeAsync("JoinGroup", game.Id, "Pedro");

            await pedroConnection.InvokeAsync("LeaveGame", game.Id);
            await Task.Delay(200);

            removedUser.Should().Be("Pedro");
            var action = async () => await userRepository.GetByNameAndGameId("Pedro", game.Id);
            await action.Should().ThrowAsync<InvalidOperationException>();
            (await userRepository.GetByNameAndGameId("Carlos", game.Id)).Admin.Should().BeTrue();
        }

        [Test]
        public async Task NotifyTheOthersThatAPlayerHasLeftTheGameBothWhenLeavingAndWhenRemovedByTheAdmin()
        {
            var game = Game.Create("userLeftGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Ana", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            var adminConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var pedroConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var anaConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var usersWhoLeft = new List<string>();
            adminConnection.On<string>("OnUserLeft", name => usersWhoLeft.Add(name));
            await adminConnection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await pedroConnection.InvokeAsync("JoinGroup", game.Id, "Pedro");
            await anaConnection.InvokeAsync("JoinGroup", game.Id, "Ana");

            await adminConnection.InvokeAsync("RemoveFromGroup", "Ana", game.Id);
            await pedroConnection.InvokeAsync("LeaveGame", game.Id);
            await Task.Delay(200);

            usersWhoLeft.Should().Equal("Ana", "Pedro");
        }

        [Test]
        public async Task PassTheAdminToAnotherPlayerWhenTheAdminLeavesTheGame()
        {
            var game = Game.Create("leaveAdminGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            var adminConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var pedroConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var newAdmin = string.Empty;
            pedroConnection.On<string>("OnAdminChanged", name => newAdmin = name);
            await adminConnection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await pedroConnection.InvokeAsync("JoinGroup", game.Id, "Pedro");

            await adminConnection.InvokeAsync("LeaveGame", game.Id);
            await Task.Delay(200);

            newAdmin.Should().Be("Pedro");
            (await userRepository.GetByNameAndGameId("Pedro", game.Id)).Admin.Should().BeTrue();
        }

        [Test]
        public async Task NotAllowAUserOutsideTheGameToLeaveIt()
        {
            var game = Game.Create("leaveDeniedGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var action = async () => await connection.InvokeAsync("LeaveGame", game.Id);

            await action.Should().ThrowAsync<HubException>();
            (await userRepository.GetByNameAndGameId("Carlos", game.Id)).Should().NotBeNull();
        }

        [Test]
        public async Task NotAllowAUserOutsideTheGameToResetItsVotes()
        {
            var game = Game.Create("resetDeniedGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create("2")));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var action = async () => await connection.InvokeAsync("ResetVotes", game.Id);

            await action.Should().ThrowAsync<HubException>();
            var user = await userRepository.GetByNameAndGameId("Carlos", game.Id);
            user.Vote.Value.Should().Be("2");
        }

        [Test]
        public async Task TakeAdminValueFromStoredUserWhenJoiningGroup()
        {
            var game = Game.Create("adminGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var admins = new Dictionary<string, bool>();
            connection.On<string, bool>("OnJoinGroup", (name, admin) => admins[name] = admin);
            await connection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await connection.InvokeAsync("JoinGroup", game.Id, "Pedro");
            await Task.Delay(200);

            admins["Carlos"].Should().BeTrue();
            admins["Pedro"].Should().BeFalse();
        }

        [Test]
        public async Task NotAllowANonAdminUserToRemoveAnotherUser()
        {
            var game = Game.Create("removeGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            connection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            await connection.InvokeAsync("JoinGroup", game.Id, "Pedro");

            var action = async () => await connection.InvokeAsync("RemoveFromGroup", "Carlos", game.Id);

            await action.Should().ThrowAsync<HubException>();
            (await userRepository.GetByNameAndGameId("Carlos", game.Id)).Should().NotBeNull();
        }

        [Test]
        public async Task AllowAdminToRemoveAUserAndNotifyThem()
        {
            var game = Game.Create("removeOkGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            await userRepository.Add(User.Create("Carlos", game.Id, string.Empty, true, Vote.Create(string.Empty)));
            await userRepository.Add(User.Create("Pedro", game.Id, string.Empty, false, Vote.Create(string.Empty)));
            var adminConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var pedroConnection = await PlaningHubStartup.StartHubConnectionAsync(server.CreateHandler(), "planing");
            var pedroMessage = string.Empty;
            var removedUser = string.Empty;
            pedroConnection.On<string>("OnReceiveMessage", m => pedroMessage = m);
            adminConnection.On<string>("OnRemoveGroup", u => removedUser = u);
            await adminConnection.InvokeAsync("JoinGroup", game.Id, "Carlos");
            await pedroConnection.InvokeAsync("JoinGroup", game.Id, "Pedro");

            await adminConnection.InvokeAsync("RemoveFromGroup", "Pedro", game.Id);
            await Task.Delay(200);

            pedroMessage.Should().Be("You has been disconnected");
            removedUser.Should().Be("Pedro");
            var action = async () => await userRepository.GetByNameAndGameId("Pedro", game.Id);
            await action.Should().ThrowAsync<InvalidOperationException>();
        }

        private async Task GivenThreeUsers(Game game)
        {
            var user1 = User.Create("Carlos", game.Id, "anyConnectionId", true, Vote.Create("3"));
            await userRepository.Add(user1);
            var user2 = User.Create("Juan", game.Id, "anyConnectionId", false, Vote.Create("5"));
            await userRepository.Add(user2);
            var user3 = User.Create("Paco", game.Id, "anyConnectionId", false, Vote.Create("8"));
            await userRepository.Add(user3);
        }

        private async Task<Game> GIvenAGame()
        {
            var game = Game.Create("anyGameId", "Carlos", "Release1", "Description", 60, 60);
            await gameRepository.Add(game);
            return game;
        }
    }
}