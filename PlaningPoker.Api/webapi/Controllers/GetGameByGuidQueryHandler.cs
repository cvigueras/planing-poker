using MediatR;

namespace webapi.Controllers;

public class GetGameByGuidQueryHandler : IRequestHandler<GetGameByGuidQuery, Game>
{
    private readonly IGameRepository gameRepository;

    public GetGameByGuidQueryHandler(IGameRepository gameRepository)
    {
        this.gameRepository = gameRepository;
    }

    public async Task<Game> Handle(GetGameByGuidQuery request, CancellationToken cancellationToken)
    {
        return await gameRepository.GetByGuid(request.Guid);
    }
}