namespace PlaningPoker.Api.Games.Models;

public class Game
{
    public string Id { get; }
    public string CreatedBy { get; }
    public string Title { get; }
    public string Description { get; }
    public long RoundTime { get; }
    public long Expiration { get; }

    private Game(string id, string createdBy, string title, string description, long roundTime, long expiration)
    {
        Id = id;
        CreatedBy = createdBy;
        Title = title;
        Description = description;
        RoundTime = roundTime;
        Expiration = expiration;
    }

    public static Game Create(string id,
        string createdBy,
        string title,
        string description,
        long roundTime,
        long expiration)
    {
        if (!IsValidCreatedBy(createdBy)) throw new ArgumentException("The field CreatedBy cannot be blank, and must have at least 2 characters and maximum 20.");
        if (!IsValidRoundTime(roundTime)) throw new ArgumentException("The round time value must be greater than 0.");
        if (!IsValidExpiration(expiration)) throw new ArgumentException("Expiration time value must be greater than 0.");
        return new Game(id, createdBy, title, description, roundTime, expiration);
    }

    private static bool IsValidExpiration(long expiration)
    {
        return expiration > 0;
    }

    private static bool IsValidRoundTime(long roundTime)
    {
        return roundTime > 0;
    }

    private static bool IsValidCreatedBy(string name)
    {
        return name.Length is > 2 and < 20;
    }
}