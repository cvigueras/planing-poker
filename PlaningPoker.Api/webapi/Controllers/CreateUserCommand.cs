using MediatR;

namespace webapi.Controllers;

public class CreateUserCommand : IRequest
{
    public CreateUserCommand(UsersAddDto usersAdd)
    {
        UsersAdd = usersAdd;
    }

    public UsersAddDto UsersAdd { get; set; }
}