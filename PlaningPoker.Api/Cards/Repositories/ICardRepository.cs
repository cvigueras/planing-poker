using PlaningPoker.Api.Cards.Models;

namespace PlaningPoker.Api.Cards.Repositories;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAll();
}