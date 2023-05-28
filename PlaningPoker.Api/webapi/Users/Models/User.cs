namespace webapi.Users.Models;

public class User
{
    private User(string name, string gameId)
    {
        Name = name;
        GameId = gameId;
    }

    public static User Create(string name, string gameId)
    {
        if (!IsValidName(name)) throw new ArgumentException("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        return new User(name, gameId);
    }

    private static bool IsValidName(string name)
    {
        return name.Length is > 2 and < 20;
    }

    public string Name { get; }
    public string GameId { get; }
}