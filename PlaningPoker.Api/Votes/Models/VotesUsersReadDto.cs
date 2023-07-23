namespace PlaningPoker.Api.Votes.Models
{
    public record VotesUsersReadDto(string Name, string GameId, bool Admin, string Value);
}