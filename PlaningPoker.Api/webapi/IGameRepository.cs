namespace webapi;

public interface IGameRepository
{
    Task<Game> GetByGuid(string guid);
    Task<string> Add(Game game);
}