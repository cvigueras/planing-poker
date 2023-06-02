using MediatR;
using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Users.Commands;

public class CreateUserCommand : IRequest
{
    public CreateUserCommand(UsersAddDto usersAdd)
    {
        UsersAdd = usersAdd;
    }

    public UsersAddDto UsersAdd { get; set; }
}