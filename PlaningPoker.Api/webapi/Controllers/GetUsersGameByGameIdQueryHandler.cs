using AutoMapper;
using MediatR;

namespace webapi.Controllers;

public class GetUsersGameByGameIdQueryHandler : IRequestHandler<GetUsersGameByGameIdQuery, IEnumerable<UsersReadDto>>
{
    private readonly IUserRepository userRepository;
    private readonly IMapper mapper;

    public GetUsersGameByGameIdQueryHandler(IUserRepository userRepository, IMapper mapper)
    {
        this.userRepository = userRepository;
        this.mapper = mapper;
    }


    public async Task<IEnumerable<UsersReadDto>> Handle(GetUsersGameByGameIdQuery request, CancellationToken cancellationToken)
    {
        var user = await userRepository.GetUsersGameByGameId(request.GameId);
        return mapper.Map<List<UsersReadDto>>(user);
    }
}