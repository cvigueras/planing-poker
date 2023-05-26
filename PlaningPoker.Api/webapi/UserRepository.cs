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

    public object GetUsersGameByGameId(string gameId)
    {
        throw new NotImplementedException();
    }

    private User ToUser(dynamic rawData)
    {
        return User.Create(rawData.Id, rawData.Name, rawData.GameId);
    }
}