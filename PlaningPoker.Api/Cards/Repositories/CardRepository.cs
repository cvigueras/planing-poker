using Dapper;
using PlaningPoker.Api.Cards.Models;
using System.Data;

namespace PlaningPoker.Api.Cards.Repositories;

public class CardRepository : ICardRepository
{
    private readonly IDbConnection connection;

    public CardRepository(IDbConnection connection)
    {
        this.connection = connection;
    }

    public async Task<IEnumerable<Card>> GetAll()
    {
        var rawData = await connection.QueryAsync<dynamic>("SELECT Id AS \"Id\", Value AS \"Value\" FROM Cards");
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