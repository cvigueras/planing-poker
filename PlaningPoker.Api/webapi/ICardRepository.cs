namespace webapi;

public interface ICardRepository
{
    Task<IEnumerable<Card>> GetAll();
}