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

        public Task<long> Handle(GetTotalNumberUsersQuery request, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }
    }
}