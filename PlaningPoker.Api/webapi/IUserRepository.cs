namespace webapi;

public interface IUserRepository
{
    Task<User> GetById(string id);
    Task Add(User user);
    Task<IEnumerable<User>> GetUsersGameByGameId(string gameId);
}