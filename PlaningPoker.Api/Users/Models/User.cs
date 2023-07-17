using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Users.Models;

public class User
{
    public string Name { get; }
    public string GameId { get; }
    public string ConnectionId { get; set; }
    public bool Admin { get; set; }
    public Vote Vote { get; set; }

    private User(string name, string gameId, string connectionId, bool admin, string vote)
    {
        Name = name;
        GameId = gameId;
        ConnectionId = connectionId;
        Admin = admin;
        Vote = Vote.Create(vote);
    }

    public static User Create(string name, string gameId, string connectionId, bool admin, Vote vote)
    {
        if (!IsValidName(name)) throw new ArgumentException("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        return new User(name, gameId, connectionId, admin, vote.Value);
    }

    private static bool IsValidName(string name)
    {
        return name.Length is > 2 and < 20;
    }

}