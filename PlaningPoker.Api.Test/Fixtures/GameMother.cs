﻿using webapi;

namespace PlaningPoker.Api.Test.Fixtures
{
    public class GameMother
    {
        public static Game CarlosAsGame()
        {
            return Game.Create(Guid.NewGuid().ToString(),"Carlos", "Release1", "Point Poker to release1", 60, 60);
        }
    }
}
