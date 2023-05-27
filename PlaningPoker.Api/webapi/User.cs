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
        if (name.Length < 2) throw new ArgumentException("The value name must be 2 characters at least.");
        return new User(id, name, gameId);
    }

    public string Id { get; }
    public string Name { get; }
    public string GameId { get; }
}