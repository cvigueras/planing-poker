using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Games.Commands;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Test.Games.Queries;
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
        var guid = await sender.Send(new CreateGameCommand(gameCreated));
        var userAdded = new UsersAddDto(gameCreated.CreatedBy, guid, true);
        await sender.Send(new CreateUserCommand(userAdded));
        var userById = await sender.Send(new GetUsersGameByGameIdQuery(guid));
        var allCardList = await sender.Send(new GetAllCardsListQuery());
        var foo = new GetGameByGuidQuery(guid, userById, allCardList);
        return Ok(await sender.Send(foo));
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        var userAdded = userAdd with { Admin = false };
        await sender.Send(new CreateUserCommand(userAdded));
        return Ok(await sender.Send(new GetGameByGuidQuery(guid,
            await sender.Send(new GetUsersGameByGameIdQuery(guid)),
            await sender.Send(new GetAllCardsListQuery()))));
    }

    [HttpGet]
    [Route("GetNumberMatchs")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
    public async Task<ActionResult> GetNumberMatchsAsync()
    {
        return Ok(await sender.Send(new GetTotalNumberMatchsQuery()));
    }

}