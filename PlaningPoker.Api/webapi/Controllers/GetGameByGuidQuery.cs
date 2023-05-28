using MediatR;

namespace webapi.Controllers;

public class GetGameByGuidQuery : IRequest<Game>
{
    public GetGameByGuidQuery(string guid)
    {
        Guid = guid;
    }

    public string Guid { get; set; }
}