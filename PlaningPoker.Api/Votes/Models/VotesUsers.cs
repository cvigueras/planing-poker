namespace PlaningPoker.Api.Votes.Models
{
    public class VotesUsers
    {
        public string UserName { get; set; }
        public string GameId { get; set; }
        public bool Admin { get; set; }
        public Vote Vote { get; set; }

        private VotesUsers(string userName, string gameId, bool admin, Vote vote)
        {
            UserName = userName;
            GameId = gameId;
            Admin = admin;
            Vote = vote;
        }

        public static VotesUsers Create(string userName, string gameId, bool admin, Vote vote)
        {
            return new VotesUsers(userName, gameId, admin, vote);
        }
    }
}