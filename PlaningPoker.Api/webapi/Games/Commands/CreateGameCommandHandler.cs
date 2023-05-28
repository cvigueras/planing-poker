using AutoMapper;
using MediatR;
using webapi.Games.Models;
using webapi.Games.Repositories;

namespace webapi.Games.Commands;

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