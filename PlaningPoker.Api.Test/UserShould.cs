using FluentAssertions;
using webapi;

namespace PlaningPoker.Api.Test
{
    public class UserShould
    {

        [Test]
        public void NotBeCreatedIfNameIsEmpty()
        {
            var action = () => User.Create(Guid.NewGuid().ToString(), string.Empty, Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The value name must be 2 characters at least.");
        }

        [Test]
        public void NotBeCreatedIfNameHasLessThanTwoCharacters()
        {
            var action = () => User.Create(Guid.NewGuid().ToString(), "a", Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The value name must be 2 characters at least.");
        }

        [Test]
        public void NotBeCreatedIfNameHasMoreThanTwentyCharacters()
        {
            var action = () => User.Create(Guid.NewGuid().ToString(), "MoreThanTwentyCharacter", Guid.NewGuid().ToString());

            action.Should().Throw<ArgumentException>().WithMessage("The name must have a maximum of 20 characters.");
        }
    }
}
