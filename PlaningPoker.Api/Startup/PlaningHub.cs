using Microsoft.AspNetCore.SignalR;
using PlaningPoker.Api.Users.Repositories;

namespace PlaningPoker.Api.Startup;

public class PlaningHub : Hub
{
    private readonly IUserRepository userRepository;

    public PlaningHub(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task JoinGroup(string group, string user)
    {
        //TODO ES UN UPDATE
        //await userRepository.Add(User.Create(user, group, Context.ConnectionId));
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await PublishUser(group, user);
    }

    public async Task RemoveFromGroup(string connectionId)
    {
        var user = await userRepository.GetByConnectionId(connectionId);
        await Groups.RemoveFromGroupAsync(user.ConnectionId, user.GameId);
    }

    public async Task PublishUser(string group, string user)
    {
        await Clients.Group(group).SendAsync("OnJoinGroup", user);
    }

    public override async Task OnConnectedAsync()
    {
        await Clients.Others.SendAsync("UserConnected", Context.ConnectionId);
        await base.OnConnectedAsync();
    }

    public override async Task OnDisconnectedAsync(Exception ex)
    {
        //await RemoveFromGroup(Context.ConnectionId);
        await Clients.Others.SendAsync("UserDisconnected", Context.ConnectionId);
        await base.OnDisconnectedAsync(ex);
    }

    //Unused functionality
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

    public async Task SendMessageToGroup(string group, string message)
    {
        await Clients.Group(group).SendAsync("OnReceiveMessage", message);
    }
}