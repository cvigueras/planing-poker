using MediatR;
using webapi.Users.Models;

namespace webapi.Users.Commands;

public class CreateUserCommand : IRequest
{
    public CreateUserCommand(UsersAddDto usersAdd)
    {
        UsersAdd = usersAdd;
    }

    public UsersAddDto UsersAdd { get; set; }
}