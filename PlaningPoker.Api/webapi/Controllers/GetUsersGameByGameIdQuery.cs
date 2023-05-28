using MediatR;

namespace webapi.Controllers;

public class GetUsersGameByGameIdQuery : IRequest<IEnumerable<UsersReadDto>>
{
    public GetUsersGameByGameIdQuery(string gameId)
    {
        GameId = gameId;
    }

    public string GameId { get; set; }
}