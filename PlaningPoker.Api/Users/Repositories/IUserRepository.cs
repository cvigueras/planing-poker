﻿using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Users.Repositories;

public interface IUserRepository
{
    Task<User> GetByNameAndGameId(string name, string gameId);

    Task Add(User user);

    Task<IEnumerable<User>> GetUsersGameByGameId(string gameId);

    Task<User> GetByConnectionId(string connectionId);
}