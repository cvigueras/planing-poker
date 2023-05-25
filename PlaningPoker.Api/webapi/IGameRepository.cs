namespace webapi;

public interface IGameRepository
{
    Task<Game> GetByGuid(string guid);
    Task Add(Game game);
}