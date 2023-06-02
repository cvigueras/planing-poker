using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Helpers;

namespace PlaningPoker.Api.Test.Games.Fixtures
{
    public class GameMother
    {
        public static Game CarlosAsGame()
        {
            return Game.Create(new GuidGenerator().Generate()
                    .ToString(),
                "Carlos",
                "Release1",
                "Session for Release1",
                60,
                60);
        }
    }
}