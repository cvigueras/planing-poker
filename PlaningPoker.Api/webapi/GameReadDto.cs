namespace webapi;

public record GameReadDto(string Id, string CreatedBy, string Title, string Description,
    int RoundTime, int Expiration);