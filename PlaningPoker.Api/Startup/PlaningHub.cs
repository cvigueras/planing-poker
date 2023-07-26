using AutoMapper;
using Microsoft.AspNetCore.SignalR;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;
using PlaningPoker.Api.Votes.Repositories;

namespace PlaningPoker.Api.Startup;

public class PlaningHub : Hub
{
    private readonly IUserRepository userRepository;
    private readonly IVoteRepository voteRepository;
    private readonly IMapper mapper;

    public PlaningHub(IUserRepository userRepository, IVoteRepository voteRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.voteRepository = voteRepository;
        this.mapper = mapper;
    }

    public async Task ReceiveAllVotes(string gameId)
    {
        var votes = await voteRepository.GetAllVotesByGroupIdAsync(gameId);
        var votesDTo = mapper.Map<List<VotesUsersReadDto>>(votes);
        await Clients.Group(gameId).SendAsync("OnReceiveAllVotes", votesDTo);
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
        await userRepository.DeleteByGameIdAndName(gameId, userName);
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