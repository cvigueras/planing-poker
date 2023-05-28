using Dapper;
using System.Data.SQLite;
using webapi.Cards.Models;

namespace webapi.Startup
{
    public class DataBase
    {
        public static void Create()
        {
            var connection = new SQLiteConnection("Data Source=./PlaningPoker.db");

            connection.Execute(@"CREATE TABLE IF NOT EXISTS Games 
                (Id VARCHAR(60) NOT NULL,
                CreatedBy VARCHAR(60) NOT NULL,
                Title VARCHAR(100) NOT NULL,
                Description VARCHAR(200) NOT NULL,
                RoundTime INTEGER NOT NULL,
                Expiration INTEGER NOT NULL)"
            );

            connection.Execute(@"CREATE TABLE IF NOT EXISTS Users(
                Name VARCHAR(20) NOT NULL,
                GameId VARCHAR(60) NOT NULL)"
            );

            connection.Execute(@"CREATE TABLE IF NOT EXISTS Cards(
                Value VARCHAR(5) NOT NULL)"
            );

            Seed(connection);

            connection.Close();
            connection.Dispose();
        }

        private static void Seed(SQLiteConnection connection)
        {
            var cards = new List<Card>
            {
                Card.Restore("?"),
                Card.Restore("coffee"),
                Card.Restore("0"),
                Card.Restore("0,5"),
                Card.Restore("1"),
                Card.Restore("2"),
                Card.Restore("3"),
                Card.Restore("5"),
                Card.Restore("8"),
                Card.Restore("13"),
                Card.Restore("20"),
                Card.Restore("40"),
                Card.Restore("100"),
            };
            connection.Execute("INSERT INTO Cards VALUES (@Value)", cards);
        }
    }
}