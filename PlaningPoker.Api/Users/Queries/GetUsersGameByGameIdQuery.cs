using MediatR;
using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Users.Queries;

public class GetUsersGameByGameIdQuery : IRequest<IEnumerable<UsersReadDto>>
{
    public string GameId { get; set; }

    public GetUsersGameByGameIdQuery(string gameId)
    {
        GameId = gameId;
    }

}