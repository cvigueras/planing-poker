using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Cards.Queries;
using PlaningPoker.Api.Games.Models;

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
            return Ok(await sender.Send(new GetAllCardsListQuery()));
        }
    }
}
