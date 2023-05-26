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

    public static Game Create(string id, string createdBy, string title, string description, long roundTime,
        long expiration)
    {
        return new Game(id,createdBy, title, description, roundTime, expiration);
    }

    public string Id { get; set; }
    public string CreatedBy { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public long RoundTime { get; set; }
    public long Expiration { get; set; }
}