namespace PlaningPoker.Api.Votes.Models;
public class Vote
{
    public string Value { get; set; }

    private Vote(string value)
    {
        Value = value;
    }

    public static Vote Create(string value)
    {
        return new Vote(value);
    }
}