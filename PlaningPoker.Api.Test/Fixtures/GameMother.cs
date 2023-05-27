using webapi;

namespace PlaningPoker.Api.Test.Fixtures
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