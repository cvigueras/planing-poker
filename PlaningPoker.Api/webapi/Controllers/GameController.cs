using AutoMapper;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace webapi.Controllers;

[ApiController]
[Route("[controller]")]
public class GameController : ControllerBase
{
    private readonly GetGameByGuidQueryHandler getGameByGuidQueryHandler;
    private readonly GetUsersGameByGameIdQueryHandler getUsersGameByGameIdQueryHandler;
    private readonly GetAllCardsListQueryHandler getAllCardsListQueryHandler;
    private readonly CreateUserCommandHandler createUserCommandHandler;
    private readonly CreateGameCommandHandler createGameCommandHandler;
    private ISender sender;

    public GameController(IGameRepository gameRepository, IUserRepository userRepository,
        ICardRepository cardRepository, IMapper mapper, ISender sender)
    {
        this.sender = sender;
        getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository, mapper);
        getUsersGameByGameIdQueryHandler = new GetUsersGameByGameIdQueryHandler(userRepository, mapper);
        getAllCardsListQueryHandler = new GetAllCardsListQueryHandler(cardRepository, mapper);
        createUserCommandHandler = new CreateUserCommandHandler(userRepository);
        createGameCommandHandler = new CreateGameCommandHandler(gameRepository, mapper);
    }

    [HttpGet("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Get(string guid)
    {
        try
        {
            var usersReadDto = await sender.Send(new GetUsersGameByGameIdQuery(guid));
            var cardDtoList = await getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
            return Ok(await getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default));
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
        var gameQuery = new CreateGameCommand(gameCreated);
        var gameId = await createGameCommandHandler.Handle(gameQuery, default);
        await createUserCommandHandler.Handle(new CreateUserCommand(new UsersAddDto(gameCreated.CreatedBy, gameId)), default);
        return Ok(gameId);
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        await createUserCommandHandler.Handle(new CreateUserCommand(userAdd), default);
        var usersReadDto = await getUsersGameByGameIdQueryHandler.Handle(new GetUsersGameByGameIdQuery(guid), default);
        var cardDtoList = await getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
        return Ok(await getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default));
    }
}