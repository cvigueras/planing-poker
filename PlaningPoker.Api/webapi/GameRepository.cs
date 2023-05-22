using System.Data.Common;
using System.Data.SQLite;
using Dapper;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace webapi;

public class GameRepository : IGameRepository
{
    private readonly SQLiteConnection connection;

    public GameRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<Game> GetByGuid(string guid)
    {
        return (await connection.QueryAsync<Game>($"SELECT * FROM Games WHERE Id = '{guid}'")).First();
    }

    public void Add(Game givenGame)
    {
        throw new NotImplementedException();
    }
}