using Microsoft.AspNetCore.SignalR.Client;
namespace PlaningPoker.Api.Test.Startup
{
    public static class PlaningHubStartup
    {
        public static async Task<HubConnection> StartHubConnectionAsync(HttpMessageHandler handler, string hubName)
        {
            var config = SetupFixture.InitConfiguration();
            var baseUrl = config["BaseUrl"];
            var hubConnection = new HubConnectionBuilder()
                .WithUrl($"ws://{baseUrl}/hubs/{hubName}", o =>
                {
                    o.HttpMessageHandlerFactory = _ => handler;
                })
                .Build();

            await hubConnection.StartAsync();

            return hubConnection;
        }
    }
}
