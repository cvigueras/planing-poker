namespace webapi;

public interface IUserRepository
{
    Task<User> GetById(string id);
}