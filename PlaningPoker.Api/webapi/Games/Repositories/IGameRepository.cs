using webapi.Games.Models;

namespace webapi.Games.Repositories;

public interface IGameRepository
{
    Task<Game> GetByGuid(string guid);
    Task Add(Game game);
}