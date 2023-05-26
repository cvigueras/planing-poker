namespace webapi;

public interface IUserRepository
{
    Task<User> GetById(string id);
    Task Add(User user);
    object GetUsersGameByGameId(string gameId);
}