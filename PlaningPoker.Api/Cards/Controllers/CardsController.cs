using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Cards.Queries;

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

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Card))]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Get()
        {
            return Ok(await sender.Send(new GetAllCardsListQuery()));
        }
    }
}
