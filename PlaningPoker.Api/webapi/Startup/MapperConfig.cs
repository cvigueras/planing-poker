﻿using AutoMapper;

namespace webapi.Startup
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Game, GameReadDto>().ReverseMap();
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
                .ConstructUsing(x => User.Create(new GuidGenerator().Generate()
                        .ToString(),
                    x.Name,
                    x.GameId));
            CreateMap<User, UsersReadDto>();
            CreateMap<Game, GameUsersReadDto>().ReverseMap();
        }
    }
}