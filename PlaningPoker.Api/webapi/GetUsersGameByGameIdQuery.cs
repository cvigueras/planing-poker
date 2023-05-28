using MediatR;

namespace webapi;

public class GetUsersGameByGameIdQuery : IRequest<IEnumerable<UsersReadDto>>
{
    public GetUsersGameByGameIdQuery(string gameId)
    {
        GameId = gameId;
    }

    public string GameId { get; set; }
}