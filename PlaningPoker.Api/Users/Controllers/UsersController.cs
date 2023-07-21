using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Test.Users.Queries;

namespace PlaningPoker.Api.Users.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        public ISender sender { get; }
        public UsersController(ISender sender)
        {
            this.sender = sender;
        }


        [HttpGet]
        [Route("GetTotalNumberUsers")]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(long))]
        public async Task<ActionResult> GetNumberMatchsAsync()
        {
            return Ok(await sender.Send(new GetTotalNumberUsersQuery()));
        }
    }
}
