namespace PlaningPoker.Api.Cards.Models;

public class Card
{
    public string Value { get; }
    public int Id { get; }

    private Card(string value, int id)
    {
        Value = value;
        Id = id;
    }

    public static Card Restore(string value, int id)
    {
        return new Card(value, id);
    }
}