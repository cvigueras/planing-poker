using AutoMapper;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly IGameRepository gameRepository;
    private readonly IMapper mapper;
    private readonly GetGameByGuidQueryHandler _getGameByGuidQueryHandler;
    private readonly GetUsersGameByGameIdQueryHandler _getUsersGameByGameIdQueryHandler;
    private readonly GetAllCardsListQueryHandler _getAllCardsListQueryHandler;
    private readonly CreateUserCommandHandler _createUserCommandHandler;

    public GameController(IGameRepository gameRepository, IUserRepository userRepository,
        ICardRepository cardRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.mapper = mapper;
        _getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository, mapper);
        _getUsersGameByGameIdQueryHandler = new GetUsersGameByGameIdQueryHandler(userRepository, mapper);
        _getAllCardsListQueryHandler = new GetAllCardsListQueryHandler(cardRepository, mapper);
        _createUserCommandHandler = new CreateUserCommandHandler(userRepository);
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        try
        {
            var usersReadDto = await _getUsersGameByGameIdQueryHandler.Handle(new GetUsersGameByGameIdQuery(guid), default);
            var cardDtoList = await _getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
            return Ok(await _getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default));
        }
        catch (InvalidOperationException ex)
        {
            return NotFound("Guid game contains no elements");
        }
    }

    [HttpPost]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Post(GameCreateDto gameCreated)
    {
        var game = mapper.Map<Game>(gameCreated);
        await gameRepository.Add(game);
        await _createUserCommandHandler.Handle(new CreateUserCommand(new UsersAddDto(game.CreatedBy, game.Id)), default);
        return Ok(game.Id);
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        await _createUserCommandHandler.Handle(new CreateUserCommand(userAdd),default);
        var usersReadDto = await _getUsersGameByGameIdQueryHandler.Handle(new GetUsersGameByGameIdQuery(guid), default);
        var cardDtoList = await _getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
        return Ok(await _getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default));
    }
}