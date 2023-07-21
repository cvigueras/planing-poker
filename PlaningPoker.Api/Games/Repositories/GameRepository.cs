using Dapper;
using PlaningPoker.Api.Games.Models;
using System;
using System.Data.SQLite;

namespace PlaningPoker.Api.Games.Repositories;

public class GameRepository : IGameRepository
{
    private readonly SQLiteConnection connection;

    public GameRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<Game> GetByGuid(string guid)
    {
        var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Games WHERE Id = '{guid}'")).First();
        return ToGame(rawData);

    }

    public async Task Add(Game game)
    {
        await connection.ExecuteAsync(
            "INSERT INTO Games (Id, CreatedBy, Title, Description, RoundTime, Expiration) " +
            $"VALUES ('{game.Id}', '{game.CreatedBy}','{game.Title}', '{game.Description}','{game.RoundTime}', '{game.Expiration}')");
    }


    private Game ToGame(dynamic rawData)
    {
        return Game.Create(rawData.Id, rawData.CreatedBy, rawData.Title, rawData.Description, rawData.RoundTime,
            rawData.Expiration);
    }

    public async Task<long> GetTotalNumberMatchs()
    {
        return connection.ExecuteScalar<long>($"SELECT COUNT(*) FROM Games");
    }
}