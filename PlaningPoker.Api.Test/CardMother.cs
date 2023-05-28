using webapi;

namespace PlaningPoker.Api.Test;

public class CardMother
{
    public static List<CardReadDto> GetAll()
    {
        return new List<CardReadDto>
        {
            new("?"),
            new("coffee"),
            new("0"),
            new("0,5"),
            new("1"),
            new("2"),
            new("3"),
            new("5"),
            new("8"),
            new("13"),
            new("20"),
            new("40"),
            new("100"),
        };
    }
}