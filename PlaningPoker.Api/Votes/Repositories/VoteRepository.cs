using Dapper;
using PlaningPoker.Api.Votes.Models;
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

        public async Task AddVoteByUserNameAndGroupIdAsync(string name, string gameId, string vote)
        {
            await connection.ExecuteAsync($"UPDATE Users SET Vote = '{vote}' WHERE Name = '{name}' AND GameId = '{gameId}'");
        }

        public async Task<IEnumerable<VotesUsers>> GetAllVotesByGroupIdAsync(string gameId)
        {
            var rawData = (await connection.QueryAsync<dynamic>($"SELECT * FROM Users WHERE GameId = '{gameId}'")).ToList();
            return ToVotesUsers(rawData);
        }

        private IEnumerable<VotesUsers> ToVotesUsers(List<dynamic> rawData)
        {
            var dataList = rawData.ToList();
            if (!dataList.Any())
                throw new InvalidOperationException();

            var listVotesUsers = new List<VotesUsers>();
            foreach (var userItem in dataList)
            {
                var vote = userItem.Vote == null ? string.Empty : userItem.Vote;
                var user = VotesUsers.Create(userItem.Name, userItem.GameId, userItem.Admin, Vote.Create(vote));
                listVotesUsers.Add(user);
            }
            return listVotesUsers;
        }
    }
}
