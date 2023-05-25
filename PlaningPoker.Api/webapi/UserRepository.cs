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
        return (await connection.QueryAsync<User>($"SELECT *, Users.Id as guid FROM Users WHERE Id = '{id}'")).First();
    }

    public async Task Add(User givenUser)
    {
        throw new NotImplementedException();
    }
}