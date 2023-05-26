namespace webapi;

public record GameUsersReadDto(string Id, string CreatedBy, string Title, string Description,
    int RoundTime, int Expiration, List<UsersReadDto> usersReadDtos);