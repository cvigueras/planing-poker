using webapi.Users.Models;

namespace webapi.Users.Repositories;

public interface IUserRepository
{
    Task<User> GetByNameAndGameId(string name, string gameId);
    Task Add(User user);
    Task<IEnumerable<User>> GetUsersGameByGameId(string gameId);
}