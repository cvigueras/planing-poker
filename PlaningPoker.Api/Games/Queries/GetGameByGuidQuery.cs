using MediatR;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Games.Queries;

public class GetGameByGuidQuery : IRequest<GameReadDto>
{
    public GetGameByGuidQuery(string guid, IEnumerable<UsersReadDto> usersReadDto, IEnumerable<CardReadDto> cardReadDto)
    {
        Guid = guid;
        UsersReadDto = usersReadDto;
        CardReadReadDto = cardReadDto;
    }

    public string Guid { get; set; }
    public IEnumerable<UsersReadDto> UsersReadDto { get; set; }
    public IEnumerable<CardReadDto> CardReadReadDto { get; set; }
}