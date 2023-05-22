﻿using System.Data.Common;
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
        return (await connection.QueryAsync<Game>($"SELECT *, Games.Id as guid FROM Games WHERE Id = '{guid}'")).First();
    }

    public async Task Add(Game game)
    {
        await connection.ExecuteAsync(
            $"INSERT INTO Games (Id, CreatedBy, Title, Description, RoundTime, Expiration) " +
            $"VALUES ('{game.Guid}', '{game.CreatedBy}','{game.Title}', '{game.Description}','{game.RoundTime}', '{game.Expiration}');");
    }
}