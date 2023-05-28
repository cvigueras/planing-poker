namespace webapi;

public class Card
{
    public string Value { get; }

    private Card(string value)
    {
        Value = value;
    }

    public static Card Restore(string value)
    {
        return new Card(value);
    }
}