using PlaningPoker.Api.Cards.Models;

namespace PlaningPoker.Api.Test.Cards.Fixtures;

public class CardMother
{
    public static List<CardReadDto> GetAll()
    {
        return new List<CardReadDto>
        {
            new("?", 1),
            new("coffee", 2),
            new("0", 3),
            new("0,5", 4),
            new("1", 5),
            new("2", 6),
            new("3", 7),
            new("5", 8),
            new("8", 9),
            new("13", 10),
            new("20", 11),
            new("40", 12),
            new("100", 13),
        };
    }
}