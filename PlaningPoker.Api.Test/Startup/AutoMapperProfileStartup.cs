using AutoMapper;
using PlaningPoker.Api.Cards.Models;
using PlaningPoker.Api.Games.Models;
using PlaningPoker.Api.Helpers;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Votes.Models;
namespace PlaningPoker.Api.Test.Startup
{
    public static class AutoMapperProfileStartup
    {
        public static IMapper AutoMapperConfig(Guid gameGuid = default)
        {
            if (gameGuid == Guid.Empty)
            {
                gameGuid = new GuidGenerator().Generate();
            }

            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<Game, GameCreateDto>();

                cfg.CreateMap<GameCreateDto, Game>()
                    .ConstructUsing(x => Game.Create(gameGuid
                            .ToString(),
                        x.CreatedBy,
                        x.Title,
                        x.Description,
                        x.RoundTime,
                        x.Expiration));

                cfg.CreateMap<UsersReadDto, User>()
                    .ConstructUsing(x => User.Create(x.Name,
                        x.GameId, string.Empty, x.Admin, Vote.Create(x.Value)));

                cfg.CreateMap<User, UsersReadDto>()
                    .ConstructUsing(x => new UsersReadDto(x.Name, x.GameId, x.Admin, x.Vote.Value));


                cfg.CreateMap<VotesUsersReadDto, VotesUsers>()
                    .ConstructUsing(x => VotesUsers.Create(x.Name, x.GameId, x.Admin, Vote.Create(x.Value)));

                cfg.CreateMap<VotesUsers, VotesUsersReadDto>()
                    .ConstructUsing(x => new VotesUsersReadDto(x.UserName, x.GameId, x.Admin, x.Vote.Value.ToString()));

                cfg.CreateMap<Game, GameReadDto>().ReverseMap();
                cfg.CreateMap<Card, CardReadDto>().ReverseMap();
            });

            return config.CreateMapper();
        }
    }
}
