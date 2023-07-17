using MediatR;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Users.Commands;

public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand>
{
    private readonly IUserRepository userRepository;

    public CreateUserCommandHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }


    public async Task Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var user = User.Create(request.UsersAdd.Name, request.UsersAdd.GameId, string.Empty, request.UsersAdd.Admin, Vote.Create(string.Empty));
        await userRepository.Add(user);
    }
}