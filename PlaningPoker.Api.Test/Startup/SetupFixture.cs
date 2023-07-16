using AutoMapper;
using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Test.Cards.Fixtures;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Startup;
public class SetupFixture : WebApplicationFactory<Program>
{
    private readonly SQLiteConnection connection;

    public SetupFixture()
    {
        connection = new SQLiteConnection("Data Source=:memory:");

        connection.Open();

        CreateDataBase();

        Seed();
    }


    private void CreateDataBase()
    {
        connection.Execute(@"CREATE TABLE IF NOT EXISTS Games(
                Id VARCHAR(60) NOT NULL,
                CreatedBy VARCHAR(60) NOT NULL,
                Title VARCHAR(100) NOT NULL,
                Description VARCHAR(200) NOT NULL,
                RoundTime INTEGER NOT NULL,
                Expiration INTEGER NOT NULL)"
        );

        connection.Execute(@"CREATE TABLE IF NOT EXISTS Users(
                ConnectionId VARCHAR(200),
                Name VARCHAR(20) NOT NULL,
                GameId VARCHAR(60) NOT NULL,
                Admin BOOLEAN,
                Vote VARCHAR(10) NOT NULL)"
        );

        connection.Execute(@"CREATE TABLE IF NOT EXISTS Cards(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Value VARCHAR(5) NOT NULL)"
        );
    }

    private void Seed()
    {
        var cards = CardMother.GetAll();
        connection.Execute("INSERT INTO Cards(Value) VALUES (@Value)", cards);
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(connection);
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSingleton<IUserRepository, UserRepository>();
            services.AddSingleton<IGuidGenerator, GuidGenerator>();
            services.AddSingleton<ICardRepository, CardRepository>();
            services.AddSignalR(options => { options.EnableDetailedErrors = true; });
        });

        return base.CreateHost(builder);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<Api.Startup.PlaningHub>("/hubs/planing");
        });
    }

    public SQLiteConnection GetSQLiteConnection()
    {
        return connection;
    }

    public async Task<HubConnection> StartHubConnectionAsync(HttpMessageHandler handler, string hubName)
    {
        var hubConnection = new HubConnectionBuilder()
            .WithUrl($"ws://localhost/hubs/{hubName}", o =>
            {
                o.HttpMessageHandlerFactory = _ => handler;
            })
            .Build();

        await hubConnection.StartAsync();

        return hubConnection;
    }

    public IMapper AutoMapperConfigTest(Guid gameGuid = default)
    {
        if (gameGuid == default)
        {
            gameGuid = new GuidGenerator().Generate();
        }
        var config = new MapperConfiguration(cfg =>
        {
            cfg.CreateMap<Game, GameCreateDto>();

            cfg.CreateMap<GameCreateDto, Game>()
                .ConstructUsing(x => Game.Create(gameGuid
                        .ToString(),
                    x.CreatedBy,
                    x.Title,
                    x.Description,
                    x.RoundTime,
                    x.Expiration));

            cfg.CreateMap<UsersReadDto, User>()
                .ConstructUsing(x => User.Create(x.Name,
                    x.GameId, string.Empty, x.Admin, Vote.Create(x.Value)));

            cfg.CreateMap<User, UsersReadDto>()
                .ConstructUsing(x => new UsersReadDto(x.Name, x.GameId, x.Admin, x.Vote.Value));

            cfg.CreateMap<Game, GameReadDto>().ReverseMap();
            cfg.CreateMap<Card, CardReadDto>().ReverseMap();
        });

        return config.CreateMapper();
    }
}