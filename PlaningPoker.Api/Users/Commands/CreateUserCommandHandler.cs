﻿using MediatR;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;

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
        var user = User.Create(request.UsersAdd.Name, request.UsersAdd.GameId);
        await userRepository.Add(user);
    }
}