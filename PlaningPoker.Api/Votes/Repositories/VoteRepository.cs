﻿using Dapper;
using PlaningPoker.Api.Users.Models;
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

        public Task<IEnumerable<User>> AddVoteByUserNameAndGroupIdAsync(string name, string gameId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<User>> GetAllVotesByGroupIdAsync(string gameId)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<VotesUsers>> GetVotesByGameIdAsync(string gameId)
        {
            var rawData = (await connection.QueryAsync<dynamic>($"SELECT Name, Vote FROM Users WHERE GameId = '{gameId}'")).ToList();
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
                var user = VotesUsers.Create(userItem.Name, Vote.Create(vote));
                listVotesUsers.Add(user);
            }
            return listVotesUsers;
        }
    }
}
