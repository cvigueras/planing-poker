namespace webapi;

public class Game
{
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
        return new Game(id, createdBy, title, description, roundTime, expiration);
    }

    public string Id { get; }
    public string CreatedBy { get; }
    public string Title { get; }
    public string Description { get; }
    public long RoundTime { get; }
    public long Expiration { get; }
}