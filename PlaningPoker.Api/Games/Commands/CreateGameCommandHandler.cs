using AutoMapper;
using MediatR;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Repositories;

namespace PlaningPoker.Api.Games.Commands;

public class CreateGameCommandHandler : IRequestHandler<CreateGameCommand, string>
{
    private readonly IGameRepository gameRepository;
    private readonly IMapper mapper;

    public CreateGameCommandHandler(IGameRepository gameRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.mapper = mapper;
    }

    public async Task<string> Handle(CreateGameCommand request, CancellationToken cancellationToken)
    {
        var game = mapper.Map<Game>(request.GameCreate);
        await gameRepository.Add(game);
        return game.Id;
    }
}