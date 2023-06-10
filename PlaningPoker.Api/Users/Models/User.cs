namespace PlaningPoker.Api.Users.Models;

public class User
{
    private User(string name, string gameId, string connectionId)
    {
        Name = name;
        GameId = gameId;
        ConnectionId = connectionId;
    }

    public static User Create(string name, string gameId, string connectionId)
    {
        if (!IsValidName(name)) throw new ArgumentException("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        return new User(name, gameId, connectionId);
    }

    private static bool IsValidName(string name)
    {
        return name.Length is > 2 and < 20;
    }

    public string Name { get; }
    public string GameId { get; }
    public string ConnectionId { get; set; }
}