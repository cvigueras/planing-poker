using AutoMapper;
using Microsoft.AspNetCore.Mvc;


namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository gameRepository;
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public GameController(IGameRepository gameRepository, IUserRepository userRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.userRepository = userRepository;
        this.mapper = mapper;
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        try
        {
            var game = await gameRepository.GetByGuid(guid);
            var result = mapper.Map<GameReadDto>(game);
            return Ok(result);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    public async Task<ActionResult> Post(GameCreateDto gameCreated)
    {
        var entity = mapper.Map<Game>(gameCreated);
        await gameRepository.Add(entity);
        return Ok(entity.Id);
    }

    [HttpPut("{gameId}")]
    public async Task<ActionResult> Put(string gameId, string userName)
    {
        var user = webapi.User.Create(new GuidGenerator().Generate().ToString(), userName, gameId);
        await userRepository.Add(user);
        var game = await gameRepository.GetByGuid(gameId);
        var usersGame = userRepository.GetUsersGameByGameId(gameId);
        var usersReadDto = mapper.Map<List<UsersReadDto>>(usersGame);
        return Ok(new GameUsersReadDto(game.Id, game.CreatedBy, game.Title, game.Description, game.RoundTime,
            game.Expiration, usersReadDto));
    }
}