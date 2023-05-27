namespace webapi;

public interface IUserRepository
{
    Task<User> GetByNameAndGameId(string name, string gameId);
    Task Add(User user);
    Task<IEnumerable<User>> GetUsersGameByGameId(string gameId);
}