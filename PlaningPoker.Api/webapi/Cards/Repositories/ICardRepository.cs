using webapi.Cards.Models;

namespace webapi.Cards.Repositories;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAll();
}