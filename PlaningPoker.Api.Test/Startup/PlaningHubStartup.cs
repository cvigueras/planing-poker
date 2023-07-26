using Microsoft.AspNetCore.SignalR.Client;
namespace PlaningPoker.Api.Test.Startup
{
    public static class PlaningHubStartup
    {
        public static async Task<HubConnection> StartHubConnectionAsync(HttpMessageHandler handler, string hubName)
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
}
