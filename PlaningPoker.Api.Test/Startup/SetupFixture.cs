using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PlaningPoker.Api.Cards.Repositories;
using PlaningPoker.Api.Games.Repositories;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Repositories;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Startup;
public class SetupFixture : WebApplicationFactory<Program>
{
    private readonly SQLiteConnection connection;

    public SetupFixture()
    {
        connection = new SQLiteConnection("Data Source=:memory:");
        connection.Open();
        _ = new DataBaseStartUp(connection);
    }

    public static IConfiguration InitConfiguration()
    {
        var config = new ConfigurationBuilder()
           .AddJsonFile("appsettings.test.json")
            .AddEnvironmentVariables()
            .Build();
        return config;
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
            services.AddSingleton<IVoteRepository, VoteRepository>();
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
}