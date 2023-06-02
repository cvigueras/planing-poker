using AutoMapper;
using MediatR;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Repositories;

namespace PlaningPoker.Api.Games.Queries;

public class GetGameByGuidQueryHandler : IRequestHandler<GetGameByGuidQuery, GameReadDto>
{
    private readonly IGameRepository gameRepository;
    private readonly IMapper mapper;

    public GetGameByGuidQueryHandler(IGameRepository gameRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.mapper = mapper;
    }

    public async Task<GameReadDto> Handle(GetGameByGuidQuery request, CancellationToken cancellationToken)
    {
        try
        {
            var game = await gameRepository.GetByGuid(request.Guid);
            return new GameReadDto(game.Id, game.CreatedBy, game.Title, game.Description, game.RoundTime,
                game.Expiration, request.UsersReadDto.ToList(), request.CardReadReadDto.ToList());
        }
        catch (InvalidOperationException ex)
        {
            throw ex;
        }
    }
}