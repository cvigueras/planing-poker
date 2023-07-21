using PlaningPoker.Api.Games.Models;

namespace PlaningPoker.Api.Games.Repositories;

public interface IGameRepository
{
    Task<Game> GetByGuid(string guid);
    Task Add(Game game);

    Task<long> GetTotalNumberMatchs();
}