using MediatR;
using PlaningPoker.Api.Users.Repositories;

namespace PlaningPoker.Api.Test.Users.Queries
{
    public class GetTotalNumberUsersQueryHandler : IRequestHandler<GetTotalNumberUsersQuery, long>
    {
        private IUserRepository repository;

        public GetTotalNumberUsersQueryHandler(IUserRepository repository)
        {
            this.repository = repository;
        }

        public async Task<long> Handle(GetTotalNumberUsersQuery request, CancellationToken cancellationToken)
        {
            return await repository.GetTotalNumberUsers();
        }
    }
}