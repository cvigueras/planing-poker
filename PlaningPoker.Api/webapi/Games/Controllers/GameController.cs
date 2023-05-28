using MediatR;
using Microsoft.AspNetCore.Mvc;
using webapi.Cards.Queries;
using webapi.Games.Commands;
using webapi.Games.Models;
using webapi.Games.Queries;
using webapi.Users.Commands;
using webapi.Users.Models;
using webapi.Users.Queries;

namespace webapi.Games.Controllers;

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
            return Ok(await sender.Send(new GetGameByGuidQuery(guid,
                await sender.Send(new GetUsersGameByGameIdQuery(guid)),
                await sender.Send(new GetAllCardsListQuery()))));
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
        var gameId = await sender.Send(new CreateGameCommand(gameCreated));
        await sender.Send(new CreateUserCommand(new UsersAddDto(gameCreated.CreatedBy, gameId)));
        return Ok(gameId);
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        await sender.Send(new CreateUserCommand(userAdd));
        return Ok(await sender.Send(new GetGameByGuidQuery(guid,
            await sender.Send(new GetUsersGameByGameIdQuery(guid)),
            await sender.Send(new GetAllCardsListQuery()))));
    }
}