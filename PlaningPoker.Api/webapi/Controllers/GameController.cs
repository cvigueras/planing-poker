using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

public class GameController : ControllerBase
{
    [HttpGet("{guid}")]
    public Task<ActionResult> Get(object guid)
    {
        throw new NotImplementedException();
    }
}