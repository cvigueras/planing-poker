namespace PlaningPoker.Api.Votes.Models
{
    public class VotesUsers
    {
        public string UserName { get; set; }
        public Vote Vote { get; set; }

        private VotesUsers(string userName, Vote vote)
        {
            UserName = userName;
            Vote = vote;
        }

        public static VotesUsers Create(string userName, Vote vote)
        {
            return new VotesUsers(userName, vote);
        }
    }
}