using AutoMapper;

namespace webapi.Startup
{
    public class MapperConfig : Profile
    {
        public MapperConfig()
        {
            CreateMap<Game, GameReadDto>().ReverseMap();
            CreateMap<Game, GameCreateDto>().ReverseMap();
        }
    }
}
