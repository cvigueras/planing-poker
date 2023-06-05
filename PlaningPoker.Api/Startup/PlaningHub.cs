using Microsoft.AspNetCore.SignalR;

namespace PlaningPoker.Api.Startup;

public class PlaningHub : Hub
{
    public async Task SendMessageToAll(string user, string message)
    {
        await Clients.All.SendAsync("OnReceiveMessage", user, message);
    }

    public Task SendMessageToCaller(string message)
    {
        return Clients.Caller.SendAsync("OnReceiveMessage", message);
    }

    public Task SendMessageToUser(string connectionId, string message)
    {
        return Clients.Client(connectionId).SendAsync("OnReceiveMessage", message);
    }

    public async Task JoinGroup(string group, string user)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await PublishUser(group, user);
    }
    
    public async Task PublishUser(string group, string user)
    {
        await Clients.Group(group).SendAsync("OnJoinGroup", user);
    }

    public async Task SendMessageToGroup(string group, string message)
    {
        await Clients.Group(group).SendAsync("OnReceiveMessage", message);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.All.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception ex)
    {
        await Clients.All.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(ex);
    }
}