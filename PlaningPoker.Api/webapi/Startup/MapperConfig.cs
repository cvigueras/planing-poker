using AutoMapper;

namespace webapi.Startup
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<GameReadDto, Game>().ReverseMap();
            CreateMap<Game, GameCreateDto>().ReverseMap();
        }
    }
}
