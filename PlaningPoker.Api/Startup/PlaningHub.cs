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

    public async Task JoinGroup(string group, string user)
    {
        var existingUser = await FindByNameAndGameId(user, group);
        var admin = existingUser?.Admin ?? false;
        var vote = existingUser?.Vote ?? Vote.Create(string.Empty);
        var userCreate = User.Create(user, group, Context.ConnectionId, admin, vote);
        await userRepository.UpdateByGameIdAndName(userCreate, userCreate.GameId);
        await Groups.AddToGroupAsync(Context.ConnectionId, group);
        await PublishUser(group, user, admin);
    }

    public async Task RemoveFromGroup(string userName, string gameId)
    {
        var caller = await FindByConnectionId(Context.ConnectionId);
        if (caller is null || !caller.Admin || caller.GameId != gameId)
            throw new HubException("Only the admin of the game can remove users.");

        var user = await FindByNameAndGameId(userName, gameId);
        if (user is null || user.Admin) return;

        await userRepository.DeleteByGameIdAndName(gameId, userName);
        if (!string.IsNullOrEmpty(user.ConnectionId))
        {
            await Clients.Client(user.ConnectionId).SendAsync("OnReceiveMessage", "You has been disconnected");
            await Groups.RemoveFromGroupAsync(user.ConnectionId, user.GameId);
        }
        await Clients.Group(gameId).SendAsync("OnRemoveGroup", userName);
    }

    public override async Task OnDisconnectedAsync(Exception? ex)
    {
        var user = await FindByConnectionId(Context.ConnectionId);
        if (user is not null)
        {
            user.ConnectionId = string.Empty;
            await userRepository.UpdateByConnectionId(user, Context.ConnectionId);
            await Clients.Group(user.GameId).SendAsync("UserDisconnected", user.Name);
        }
        await base.OnDisconnectedAsync(ex);
    }

    private async Task PublishUser(string group, string user, bool admin)
    {
        await Clients.Group(group).SendAsync("OnJoinGroup", user, admin);
    }

    private async Task<User?> FindByNameAndGameId(string name, string gameId)
    {
        try
        {
            return await userRepository.GetByNameAndGameId(name, gameId);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
    }

    private async Task<User?> FindByConnectionId(string connectionId)
    {
        try
        {
            return await userRepository.GetByConnectionId(connectionId);
        }
        catch (InvalidOperationException)
        {
            return null;
        }
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