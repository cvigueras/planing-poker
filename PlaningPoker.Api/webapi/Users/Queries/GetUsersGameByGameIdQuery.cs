using MediatR;
using webapi.Users.Models;

namespace webapi.Users.Queries;

public class GetUsersGameByGameIdQuery : IRequest<IEnumerable<UsersReadDto>>
{
    public GetUsersGameByGameIdQuery(string gameId)
    {
        GameId = gameId;
    }

    public string GameId { get; set; }
}