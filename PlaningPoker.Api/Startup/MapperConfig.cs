using AutoMapper;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;

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
                    x.GameId, string.Empty, x.Admin, Vote.Create(x.Value)));

            CreateMap<User, UsersReadDto>()
                .ConstructUsing(x=> new UsersReadDto(x.Name, x.GameId, x.Admin, x.Vote.Value.ToString()));
            
            CreateMap<VotesUsersReadDto, VotesUsers>()
                .ConstructUsing(x => VotesUsers.Create(x.Name, x.GameId, x.Admin, Vote.Create(x.Value)));

            CreateMap<VotesUsers, VotesUsersReadDto>()
                .ConstructUsing(x=> new VotesUsersReadDto(x.UserName, x.GameId, x.Admin, x.Vote.Value.ToString()));

            CreateMap<Game, GameReadDto>().ReverseMap();
            CreateMap<Card, CardReadDto>().ReverseMap();
        }
    }
}