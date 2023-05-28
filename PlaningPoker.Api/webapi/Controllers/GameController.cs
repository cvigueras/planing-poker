using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository gameRepository;
    private readonly IUserRepository userRepository;
    private readonly ICardRepository cardRepository;
    private readonly IMapper mapper;
    private readonly GetGameByGuidQueryHandler _getGameByGuidQueryHandler;
    private readonly GetUsersGameByGameIdQueryHandler _getUsersGameByGameIdQueryHandler;

    public GameController(IGameRepository gameRepository, IUserRepository userRepository,
        ICardRepository cardRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.userRepository = userRepository;
        this.cardRepository = cardRepository;
        this.mapper = mapper;
        _getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository);
        _getUsersGameByGameIdQueryHandler = new GetUsersGameByGameIdQueryHandler(userRepository);
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        try
        {
            var queryGame = new GetGameByGuidQuery(guid);
            var game = await _getGameByGuidQueryHandler.Handle(queryGame, default);
            var queryUser = new GetUsersGameByGameIdQuery(guid);
            var usersGame = await _getUsersGameByGameIdQueryHandler.Handle(queryUser, default);
            var usersReadDto = mapper.Map<List<UsersReadDto>>(usersGame);
            var cards = await cardRepository.GetAll();
            var cardDtoList = mapper.Map<IEnumerable<CardReadDto>>(cards);
            var gameReadDto = new GameReadDto(game.Id, game.CreatedBy, game.Title, game.Description, game.RoundTime,
                game.Expiration, usersReadDto, cardDtoList.ToList());
            return Ok(gameReadDto);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(GameCreateDto gameCreated)
    {
        var game = mapper.Map<Game>(gameCreated);
        var user = webapi.User.Create(gameCreated.CreatedBy, game.Id);
        await userRepository.Add(user);
        await gameRepository.Add(game);
        return Ok(game.Id);
    }

    [HttpPut("{gameId}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string gameId, UsersAddDto userAdd)
    {
        var user = webapi.User.Create(userAdd.Name, gameId);
        await userRepository.Add(user);
        var game = await gameRepository.GetByGuid(gameId);
        var usersGame = await userRepository.GetUsersGameByGameId(gameId);
        var usersReadDto = mapper.Map<List<UsersReadDto>>(usersGame);
        var cards = await cardRepository.GetAll();
        var cardDtoList = mapper.Map<IEnumerable<CardReadDto>>(cards);
        return Ok(new GameReadDto(game.Id, game.CreatedBy, game.Title, game.Description, game.RoundTime,
            game.Expiration, usersReadDto, cardDtoList.ToList()));
    }
}