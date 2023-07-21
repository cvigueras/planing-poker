using MediatR;

namespace PlaningPoker.Api.Test.Users.Queries
{
    public class GetTotalNumberUsersQuery : IRequest<long>
    {
        public GetTotalNumberUsersQuery()
        {
        }
    }
}