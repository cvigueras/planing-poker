using FluentAssertions;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Test.Users.Models
{
    public class UserShould
    {

        [Test]
        public void NotBeCreatedIfNameIsEmpty()
        {
            var action = () => User.Create(string.Empty, Guid.NewGuid().ToString(), string.Empty, false, Vote.Create(string.Empty));

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }

        [Test]
        public void NotBeCreatedIfNameHasLessThanTwoCharacters()
        {
            var action = () => User.Create("a", Guid.NewGuid().ToString(), string.Empty, false, Vote.Create(string.Empty));

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }

        [Test]
        public void NotBeCreatedIfNameHasMoreThanTwentyCharacters()
        {
            var action = () => User.Create("MoreThanTwentyCharacter", Guid.NewGuid().ToString(), string.Empty, false, Vote.Create(string.Empty));

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }
    }
}
