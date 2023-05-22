namespace webapi;

public interface IGameRepository
{
    object GetByGuid(string guid);
}