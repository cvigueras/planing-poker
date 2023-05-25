using Dapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Data.SQLite;
using webapi;

namespace PlaningPoker.Api.Test.Startup;
public class SetupFixture : WebApplicationFactory<Program>
{
    private readonly SQLiteConnection connection;

    public SetupFixture()
    {
        connection = new SQLiteConnection("Data Source=:memory:");

        connection.Open();

        CreateDataBase();
    }

    private void CreateDataBase()
    {
        connection.Execute(@"CREATE TABLE IF NOT EXISTS Games(
                Id VARCHAR(60) NOT NULL,
                CreatedBy VARCHAR(60) NOT NULL,
                Title VARCHAR(100) NOT NULL,
                Description VARCHAR(200) NOT NULL,
                RoundTime INTEGER NOT NULL,
                Expiration INTEGER NOT NULL,
                ReturnUrl VARCHAR(250) NOT NULL)"
        );
    }

    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services.AddSingleton(connection);
            services.AddSingleton<IGameRepository, GameRepository>();
            services.AddSignalR(options => { options.EnableDetailedErrors = true; });
        });

        return base.CreateHost(builder);
    }

    public void Configure(IApplicationBuilder app)
    {
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapHub<PlaningHub>("/hubs/planing");
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
}