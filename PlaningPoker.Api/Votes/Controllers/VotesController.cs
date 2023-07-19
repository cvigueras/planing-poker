using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Test.Votes.Commands;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Votes.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class VotesController : ControllerBase
    {
        private ISender sender { get; }

        public VotesController(ISender sender)
        {
            this.sender = sender;
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult> Post(VotesUsersCreateDto votesUsers)
        {
            await sender.Send(new CreateVoteCommand(votesUsers));
            return Ok("Vote inserted succesfully");
        }
    }
}
