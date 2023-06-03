using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Games.Queries;
using PlaningPoker.Api.Users.Queries;

namespace PlaningPoker.Api.Cards.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class CardsController : ControllerBase
    {
        private readonly ISender sender;

        public CardsController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpGet()]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            try
            {
                return Ok(await sender.Send(new GetAllCardsListQuery()));
            }
            catch (InvalidOperationException ex)
            {
                return NotFound("Guid game contains no elements");
            }
        }
    }
}
