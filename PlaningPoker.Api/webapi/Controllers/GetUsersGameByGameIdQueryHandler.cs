using MediatR;

namespace webapi.Controllers;

public class GetUsersGameByGameIdQueryHandler : IRequestHandler<GetUsersGameByGameIdQuery, IEnumerable<User>>
{
    private readonly IUserRepository userRepository;

    public GetUsersGameByGameIdQueryHandler(IUserRepository userRepository)
    {
        this.userRepository = userRepository;
    }


    public async Task<IEnumerable<User>> Handle(GetUsersGameByGameIdQuery request, CancellationToken cancellationToken)
    {
        return await userRepository.GetUsersGameByGameId(request.GameId);
    }
}