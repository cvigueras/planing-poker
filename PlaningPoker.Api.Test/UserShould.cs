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

            action.Should().Throw<ArgumentException>().WithMessage("The value name must be 2 characters at least");
        }
    }
}
