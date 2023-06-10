using FluentAssertions;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.AspNetCore.TestHost;
using NSubstitute;
using PlaningPoker.Api.Test.Startup;
using PlaningPoker.Api.Users.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.PlaningHub.Features
{
    public class PlaningHubShould
    {
        private SetupFixture setupFixture;
        private SQLiteConnection connectionSql;
        private TestServer server;
        private HubConnection connection;
        private UserRepository userRepository;

        [SetUp]
        public void Setup()
        {
            setupFixture = new SetupFixture();
            setupFixture.CreateClient();
            server = setupFixture.Server;
            setupFixture = new SetupFixture();
            connectionSql = setupFixture.GetSQLiteConnection();
            userRepository = new UserRepository(connectionSql);
        }

        [Test]
        public async Task SendMessageToAllSuccessfully()
        {
            connection = await setupFixture.StartHubConnectionAsync(server.CreateHandler(), "planing");

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
            connection = await setupFixture.StartHubConnectionAsync(server.CreateHandler(), "planing");

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
            var messageHub = new Api.Startup.PlaningHub(userRepository);
            messageHub.Context = hubContext;
            messageHub.Clients = clients;
            messageHub.Groups = groups;

            await messageHub.JoinGroup("group1", "Carlos");

            await messageHub.Groups.Received(1).AddToGroupAsync(Arg.Any<string>(), Arg.Any<string>(), default);
        }

        [Test]
        public async Task PublishNewUserInGroupSuccessfully()
        {
            connection = await setupFixture.StartHubConnectionAsync(server.CreateHandler(), "planing");

            var user = "Pedro";
            var publishedUser = string.Empty;
            connection.On<string>("OnJoinGroup", (userReceived) =>
            {
                publishedUser = userReceived;
            });
            await connection.InvokeAsync("JoinGroup", "group1", "Pedro");
            await Task.Delay(200);

            publishedUser.Should().Be(user);
        }
    }
}