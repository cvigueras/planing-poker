using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

public class GameController : ControllerBase
{
    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public Task<ActionResult> Get(object guid)
    {
        throw new NotImplementedException();
    }
}