using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Games.Models;

public record GameReadDto(string Id, string CreatedBy, string Title, string Description,
    long RoundTime, long Expiration, List<UsersReadDto> Users, List<CardReadDto> Cards);