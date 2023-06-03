using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Games.Commands;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Users.Commands;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Queries;

namespace PlaningPoker.Api.Games.Controllers;

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