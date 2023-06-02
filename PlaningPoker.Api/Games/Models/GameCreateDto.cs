namespace PlaningPoker.Api.Games.Models;

public record GameCreateDto(string CreatedBy, string Title, string Description, int RoundTime, int Expiration);