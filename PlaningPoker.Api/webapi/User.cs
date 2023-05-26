namespace webapi;

public class User
{

    private User(string id, string name, string gameId)
    {
        Id = id;
        Name = name;
        GameId = gameId;
    }

    public static User Create(string id, string name, string gameId)
    {
        return new User(id,name,gameId);
    }

    public string Id { get; }
    public string Name { get; }
    public string GameId { get; }
}