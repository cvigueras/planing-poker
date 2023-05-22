using Dapper;
using System.Data.SQLite;

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
        return (await connection.QueryAsync<Game>($"SELECT *, Games.Id as guid FROM Games WHERE Id = '{guid}'")).First();
    }

    public async Task<string> Add(Game game)
    {
        game.Guid = Guid.NewGuid().ToString();
        await connection.ExecuteAsync(
            $"INSERT INTO Games (Id, CreatedBy, Title, Description, RoundTime, Expiration) " +
            $"VALUES ('{game.Guid}', '{game.CreatedBy}','{game.Title}', '{game.Description}','{game.RoundTime}', '{game.Expiration}');");
        return game.Guid;
    }
}