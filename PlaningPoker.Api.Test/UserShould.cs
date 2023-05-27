using FluentAssertions;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class UserShould
    {

        [Test]
        public void NotBeCreatedIfNameIsEmpty()
        {
            var action = () => User.Create(string.Empty, Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }

        [Test]
        public void NotBeCreatedIfNameHasLessThanTwoCharacters()
        {
            var action = () => User.Create("a", Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }

        [Test]
        public void NotBeCreatedIfNameHasMoreThanTwentyCharacters()
        {
            var action = () => User.Create("MoreThanTwentyCharacter", Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The name cannot be blank, and must have at least 2 characters and maximum 20.");
        }
    }
}
