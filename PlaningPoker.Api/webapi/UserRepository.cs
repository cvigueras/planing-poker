using System.Data.SQLite;
using Dapper;

namespace webapi;

public class UserRepository : IUserRepository
{
    private readonly SQLiteConnection connection;

    public UserRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<User> GetById(string id)
    {
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE Id = '{id}'")).First();
        return ToUser(rawData);
    }

    public async Task Add(User user)
    {
        await connection.ExecuteAsync(
            "INSERT INTO Users (Id, Name, GameId) " +
            $"VALUES ('{user.Id}', '{user.Name}', '{user.GameId}');");
    }

    public async Task<IEnumerable<User>> GetUsersGameByGameId(string gameId)
    {
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE GameId = '{gameId}'")).ToList();
        return ToListUser(rawData);
    }

    private User ToUser(dynamic rawData)
    {
        return User.Create(rawData.Id, rawData.Name, rawData.GameId);
    }

    private IEnumerable<User> ToListUser(IEnumerable<dynamic> rawData)
    {
        var dataList = rawData.ToList();
        if (!dataList.Any())
            throw new InvalidOperationException();

        var listUsers = new List<User>();
        foreach (var userItem in dataList)
        {
            var user = User.Create(userItem.Id, userItem.Name, userItem.GameId);
            listUsers.Add(user);
        }
        return listUsers;
    }
}