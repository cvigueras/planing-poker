using webapi;

namespace PlaningPoker.Api.Test.Fixtures
{
    public class GameMother
    {
        public static Game CarlosAsGame()
        {
            return new Game
            {
                CreatedBy = "Carlos",
                Description = "Point Poker to release1",
                Expiration = 60,
                RoundTime = 60,
                Title = "Release1",
            };
        }
    }
}
