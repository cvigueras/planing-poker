using MediatR;

namespace webapi;

public class CreateUserCommand : IRequest
{
    public CreateUserCommand(UsersAddDto usersAdd)
    {
        UsersAdd = usersAdd;
    }

    public UsersAddDto UsersAdd { get; set; }
}