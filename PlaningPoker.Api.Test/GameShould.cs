﻿using FluentAssertions;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class GameShould
    {
        [Test]
        public void NotBeCreatedIfCreatedByIsEmpty()
        {
            var action = () => Game.Create(new GuidGenerator().Generate().ToString(),string.Empty, "Title", "Description",90,60);

            action.Should().Throw<ArgumentException>().WithMessage("The field CreatedBy cannot be blank, and must have at least 2 characters and maximum 20.");
        }
    }
}
