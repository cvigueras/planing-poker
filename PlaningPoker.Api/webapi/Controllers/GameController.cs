using AutoMapper;
using Microsoft.AspNetCore.Http.Extensions;
using Microsoft.AspNetCore.Mvc;
using PlaningPoker.Api.Test;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository gameRepository;
    private readonly IMapper mapper;
    private readonly IGuidGenerator guidGenerator;

    public GameController(IGameRepository gameRepository, IMapper mapper, IGuidGenerator guidGenerator)
    {
        this.gameRepository = gameRepository;
        this.mapper = mapper;
        this.guidGenerator = guidGenerator;
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
        entity.Id = guidGenerator.Generate().ToString();
        await gameRepository.Add(entity);

        return Ok(entity.Id);
    }
}