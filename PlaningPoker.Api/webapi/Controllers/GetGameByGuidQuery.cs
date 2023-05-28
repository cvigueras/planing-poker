using MediatR;

namespace webapi.Controllers;

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