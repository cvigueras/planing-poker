using Dapper;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;
using System.Data.SQLite;

namespace PlaningPoker.Api.Users.Repositories;

public class UserRepository : IUserRepository
{
    private readonly SQLiteConnection connection;

    public UserRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<User> GetByNameAndGameId(string name, string gameId)
    {
        var rawData = (await connection.QueryAsync<dynamic>(
            "SELECT * FROM Users WHERE GameId = @gameId AND Name = @name", new { gameId, name })).First();
        return ToUser(rawData);
    }

    public async Task Add(User user)
    {
        await connection.ExecuteAsync(
            "INSERT INTO Users (Name, GameId, ConnectionId, Admin, Vote) " +
            "VALUES (@Name, @GameId, @ConnectionId, @Admin, @Vote)",
            new { user.Name, user.GameId, user.ConnectionId, Admin = user.Admin.ToString(), Vote = user.Vote.Value });
    }

    public async Task<IEnumerable<User>> GetUsersGameByGameId(string gameId)
    {
        var rawData = (await connection.QueryAsync<dynamic>(
            "SELECT * FROM Users WHERE GameId = @gameId", new { gameId })).ToList();
        return ToListUser(rawData);
    }
    public async Task<User> GetByConnectionId(string connectionId)
    {
        var rawData = (await connection.QueryAsync<dynamic>(
            "SELECT * FROM Users WHERE ConnectionId = @connectionId", new { connectionId })).First();
        return ToUser(rawData);
    }

    public async Task UpdateByConnectionId(User givenUser, string connectionId)
    {
        await connection.ExecuteAsync(
            "UPDATE Users SET Name = @Name, GameId = @GameId, ConnectionId = @NewConnectionId, Vote = @Vote WHERE ConnectionId = @connectionId",
            new { givenUser.Name, givenUser.GameId, NewConnectionId = givenUser.ConnectionId, Vote = givenUser.Vote.Value, connectionId });
    }

    public async Task UpdateByGameIdAndName(User user, string gameId)
    {
        await connection.ExecuteAsync(
            "UPDATE Users SET Name = @Name, GameId = @GameId, ConnectionId = @ConnectionId, Vote = @Vote WHERE GameId = @whereGameId AND Name = @Name",
            new { user.Name, user.GameId, user.ConnectionId, Vote = user.Vote.Value, whereGameId = gameId });
    }

    public async Task DeleteByGameIdAndName(string gameGuid, string userName)
    {
        await connection.ExecuteAsync(
            "DELETE FROM Users WHERE GameId = @gameGuid AND Name = @userName", new { gameGuid, userName });
    }

    public async Task ResetVotesByGameId(string gameId)
    {
        await connection.ExecuteAsync(
            "UPDATE Users SET Vote = '' WHERE GameId = @gameId", new { gameId });
    }

    public async Task<long> GetTotalNumberUsers()
    {
        return connection.ExecuteScalar<long>($"SELECT COUNT(*) FROM Users");
    }

    private User ToUser(dynamic rawData)
    {
        return User.Create(rawData.Name, rawData.GameId, rawData.ConnectionId, rawData.Admin, Vote.Create(rawData.Vote));
    }

    private IEnumerable<User> ToListUser(IEnumerable<dynamic> rawData)
    {
        var dataList = rawData.ToList();
        if (!dataList.Any())
            throw new InvalidOperationException();

        var listUsers = new List<User>();
        foreach (var userItem in dataList)
        {
            var vote = userItem.Vote == null ? string.Empty : userItem.Vote;
            var user = User.Create(userItem.Name, userItem.GameId, string.Empty, userItem.Admin, Vote.Create(vote));
            listUsers.Add(user);
        }
        return listUsers;
    }
}