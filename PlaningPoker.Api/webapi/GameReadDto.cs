namespace webapi;

public record GameReadDto(string Guid, string CreatedBy, string Title, string Description,
    int RoundTime, int Expiration);