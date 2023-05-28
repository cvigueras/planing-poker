using System.Data.SQLite;
using Dapper;

namespace webapi;

public class CardRepository : ICardRepository
{
    private readonly SQLiteConnection connection;

    public CardRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<Card>> GetAll()
    {
       return await connection.QueryAsync<Card>($"SELECT * FROM Cards");
    }
}