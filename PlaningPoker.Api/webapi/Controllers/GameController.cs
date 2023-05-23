using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository gameRepository;
    private readonly IMapper mapper;

    public GameController(IGameRepository gameRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.mapper = mapper;
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        var game = await gameRepository.GetByGuid(guid);
        if (game != null)
        {
            return Ok(mapper.Map<GameReadDto>(game));
        }
        return NotFound();
    }

    [HttpPost]
    public async Task<ActionResult> Post(GameCreateDto gameCreated)
    {
        var entity = mapper.Map<Game>(gameCreated);
        return Ok(await gameRepository.Add(entity));
    }
}