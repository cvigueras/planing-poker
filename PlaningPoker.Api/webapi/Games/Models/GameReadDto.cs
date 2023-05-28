using webapi.Cards.Models;
using webapi.Users.Models;

namespace webapi.Games.Models;

public record GameReadDto(string Id, string CreatedBy, string Title, string Description,
    long RoundTime, long Expiration, List<UsersReadDto> Users, List<CardReadDto> Cards);