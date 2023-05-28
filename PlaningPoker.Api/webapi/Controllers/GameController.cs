using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private ISender sender;

    public GameController(ISender sender)
    {
        this.sender = sender;
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        try
        {
            var usersReadDto = await sender.Send(new GetUsersGameByGameIdQuery(guid));
            var cardDtoList = await sender.Send(new GetAllCardsListQuery());
            return Ok(await sender.Send(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList)));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound("Guid game contains no elements");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(GameCreateDto gameCreated)
    {
        var gameQuery = new CreateGameCommand(gameCreated);
        var gameId = await sender.Send(gameQuery);
        await sender.Send(new CreateUserCommand(new UsersAddDto(gameCreated.CreatedBy, gameId)));
        return Ok(gameId);
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        await sender.Send(new CreateUserCommand(userAdd));
        var usersReadDto = await sender.Send(new GetUsersGameByGameIdQuery(guid));
        var cardDtoList = await sender.Send(new GetAllCardsListQuery());
        return Ok(await sender.Send(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList)));
    }
}