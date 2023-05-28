using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;

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
    private readonly GetAllCardsListQueryHandler _getAllCardsListQueryHandler;

    public GameController(IGameRepository gameRepository, IUserRepository userRepository,
        ICardRepository cardRepository, IMapper mapper)
    {
        this.gameRepository = gameRepository;
        this.userRepository = userRepository;
        this.cardRepository = cardRepository;
        this.mapper = mapper;
        _getGameByGuidQueryHandler = new GetGameByGuidQueryHandler(gameRepository, mapper);
        _getUsersGameByGameIdQueryHandler = new GetUsersGameByGameIdQueryHandler(userRepository, mapper);
        _getAllCardsListQueryHandler = new GetAllCardsListQueryHandler(cardRepository, mapper);
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
            var gameReadDto = await _getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default);
            return Ok(gameReadDto);
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
        var user = webapi.User.Create(gameCreated.CreatedBy, game.Id);
        await userRepository.Add(user);
        await gameRepository.Add(game);
        return Ok(game.Id);
    }

    [HttpPut("{guid}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Game))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> Put(string guid, UsersAddDto userAdd)
    {
        var user = webapi.User.Create(userAdd.Name, guid);
        await userRepository.Add(user);
        var usersReadDto = await _getUsersGameByGameIdQueryHandler.Handle(new GetUsersGameByGameIdQuery(guid), default);
        var cardDtoList = await _getAllCardsListQueryHandler.Handle(new GetAllCardsListQuery(), default);
        var gameReadDto = await _getGameByGuidQueryHandler.Handle(new GetGameByGuidQuery(guid, usersReadDto, cardDtoList), default);
        return Ok(gameReadDto);
    }
}