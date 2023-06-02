using MediatR;
using PlaningPoker.Api.Games.Models;

namespace PlaningPoker.Api.Games.Commands;

public class CreateGameCommand : IRequest<string>
{
    public CreateGameCommand(GameCreateDto gameCreate)
    {
        GameCreate = gameCreate;
    }

    public GameCreateDto GameCreate { get; set; }
}