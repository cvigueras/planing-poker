using Microsoft.AspNetCore.SignalR;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Startup;

public class PlaningHub : Hub
{
    private readonly IUserRepository userRepository;

    public PlaningHub(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }

    public async Task NotifyUserHasVoted(string group, string name, string vote)
    {        
        await Clients.Group(group).SendAsync("OnNotifyUserHasVoted", name, vote);
    }

    public async Task JoinGroup(string group, string user, bool admin)
    {
        var userCreate = User.Create(user, group, Context.ConnectionId, admin, Vote.Create(string.Empty));
        await userRepository.UpdateByGameIdAndName(userCreate, userCreate.GameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await PublishUser(group, user, admin);
    }

    public async Task RemoveFromGroup(string userName, string gameId)
    {
        var user = await userRepository.GetByNameAndGameId(userName, gameId);
        await Groups.RemoveFromGroupAsync(user.ConnectionId, user.GameId);
        await Clients.Group(user.GameId).SendAsync("OnRemoveGroup", user, user.ConnectionId);
    }

    public async Task PublishUser(string group, string user, bool admin)
    {
        await Clients.Group(group).SendAsync("OnJoinGroup", user, admin);
    }

    public Task SendMessageToUser(string connectionId, string message)
    {
        return Clients.Client(connectionId).SendAsync("OnReceiveMessage", message);
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


    public async Task SendMessageToGroup(string group, string message)
    {
        await Clients.Group(group).SendAsync("OnReceiveMessage", message);
    }
}