using AutoMapper;
using MediatR;
using PlaningPoker.Api.Users.Models;
using PlaningPoker.Api.Users.Repositories;

namespace PlaningPoker.Api.Users.Queries;

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