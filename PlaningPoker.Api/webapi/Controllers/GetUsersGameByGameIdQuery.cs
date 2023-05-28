using MediatR;

namespace webapi.Controllers;

public class GetUsersGameByGameIdQuery : IRequest<IEnumerable<User>>
{
    public GetUsersGameByGameIdQuery(string gameId)
    {
        GameId = gameId;
    }

    public string GameId { get; set; }
}