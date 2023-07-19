using MediatR;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Votes.Models;

namespace PlaningPoker.Api.Votes.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotesController : ControllerBase
    {
        public ISender Sender { get; }

        public VotesController(ISender sender)
        {
            Sender = sender;
        }

        public Task<IActionResult> Post(VotesUsersCreateDto votesUsers)
        {
            throw new NotImplementedException();
        }
    }
}
