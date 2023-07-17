using System.Data.SQLite;

namespace PlaningPoker.Api.Votes.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private SQLiteConnection connection;

        public VoteRepository(SQLiteConnection connection)
        {
            this.connection = connection;
        }
    }
}
