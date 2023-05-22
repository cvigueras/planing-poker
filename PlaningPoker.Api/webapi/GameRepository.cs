using System.Data.SQLite;

namespace webapi;

public class GameRepository : IGameRepository
{
    public GameRepository(SQLiteConnection connection)
    {
        throw new NotImplementedException();
    }

    public object GetByGuid(string guid)
    {
        throw new NotImplementedException();
    }
}