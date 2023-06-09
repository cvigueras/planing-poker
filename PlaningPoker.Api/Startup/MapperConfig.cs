﻿using AutoMapper;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Users.Models;

namespace PlaningPoker.Api.Startup
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Game, GameCreateDto>();

            CreateMap<GameCreateDto, Game>()
                .ConstructUsing(x => Game.Create(new GuidGenerator().Generate()
                        .ToString(),
                    x.CreatedBy,
                    x.Title,
                    x.Description,
                    x.RoundTime,
                    x.Expiration));

            CreateMap<UsersReadDto, User>()
                .ConstructUsing(x => User.Create(x.Name,
                    x.GameId, string.Empty, x.Admin));

            CreateMap<User, UsersReadDto>();
            CreateMap<Game, GameReadDto>().ReverseMap();
            CreateMap<Card, CardReadDto>().ReverseMap();
        }
    }
}