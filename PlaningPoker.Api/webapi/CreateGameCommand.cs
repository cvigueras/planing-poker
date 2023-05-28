using MediatR;

namespace webapi;

public class CreateGameCommand : IRequest<string>
{
    public CreateGameCommand(GameCreateDto gameCreate)
    {
        GameCreate = gameCreate;
    }

    public GameCreateDto GameCreate { get; set; }
}