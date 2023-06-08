using Dapper;
using PlaningPoker.Api.Cards.Models;
using System.Data.SQLite;

namespace PlaningPoker.Api.Cards.Repositories;

public class CardRepository : ICardRepository
{
    private readonly SQLiteConnection connection;

    public CardRepository(SQLiteConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<Card>> GetAll()
    {
        var rawData = await connection.QueryAsync<dynamic>($"SELECT * FROM Cards");
        return ToCardList(rawData);
    }

    private IEnumerable<Card> ToCardList(IEnumerable<dynamic> rawData)
    {
        var dataList = rawData.ToList();
        var cardList = new List<Card>();
        foreach (var row in dataList)
        {
            cardList.Add(Card.Restore(row.Value, Convert.ToInt32(row.Id)));
        }
        return cardList;
    }
}