using MediatR;
using webapi.Games.Models;

namespace webapi.Games.Commands;

public class CreateGameCommand : IRequest<string>
{
    public CreateGameCommand(GameCreateDto gameCreate)
    {
        GameCreate = gameCreate;
    }

    public GameCreateDto GameCreate { get; set; }
}