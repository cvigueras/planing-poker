namespace webapi;

public record GameUsersReadDto(string Id, string CreatedBy, string Title, string Description,
    long RoundTime, long Expiration, List<UsersReadDto> usersReadDtos);