using Dapper;
using PlaningPoker.Api.Users.Models;
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
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE GameId = '{gameId}' AND Name='{name}'")).First();
        return ToUser(rawData);
    }

    public async Task Add(User user)
    {
        await connection.ExecuteAsync(
            "INSERT INTO Users (Name, GameId, ConnectionId) " +
            $"VALUES ('{user.Name}', '{user.GameId}', '{user.ConnectionId}');");
    }

    public async Task<IEnumerable<User>> GetUsersGameByGameId(string gameId)
    {
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE GameId = '{gameId}'")).ToList();
        return ToListUser(rawData);
    }
    public async Task<User> GetByConnectionId(string connectionId)
    {
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE ConnectionId = '{connectionId}'")).First();
        return ToUser(rawData);
    }

    public async Task UpdateByConnectionId(User givenUser, string connectionId)
    {
        await connection.ExecuteAsync(
            $"UPDATE Users SET Name = '{givenUser.Name}', GameId = '{givenUser.GameId}', ConnectionId = '{givenUser.ConnectionId}' WHERE ConnectionId = '{connectionId}'");
    }

    public async Task UpdateByGameIdAndName(User user, string gameId)
    {
        await connection.ExecuteAsync(
            $"UPDATE Users SET Name = '{user.Name}', GameId = '{user.GameId}', ConnectionId = '{user.ConnectionId}' WHERE GameId = '{gameId}' AND Name= '{user.Name}'");
    }

    public async Task DeleteByGameIdAndName(string gameGuid, string gameId)
    {
        await connection.ExecuteAsync(
            $"DELETE FROM Users WHERE GameId = '{gameId}'");
    }

    private User ToUser(dynamic rawData)
    {
        return User.Create(rawData.Name, rawData.GameId, rawData.ConnectionId);
    }

    private IEnumerable<User> ToListUser(IEnumerable<dynamic> rawData)
    {
        var dataList = rawData.ToList();
        if (!dataList.Any())
            throw new InvalidOperationException();

        var listUsers = new List<User>();
        foreach (var userItem in dataList)
        {
            var user = User.Create(userItem.Name, userItem.GameId, string.Empty);
            listUsers.Add(user);
        }
        return listUsers;
    }

}