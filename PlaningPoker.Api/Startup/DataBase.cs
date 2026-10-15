using Dapper;
using PlaningPoker.Api.Cards.Models;
using System.Data;

namespace PlaningPoker.Api.Startup
{
    public class DataBase
    {
        public static void Create(IDbConnection connection, bool postgres)
        {
            // The admin flag is stored as text ("True" / "False"), so it is VARCHAR in Postgres and not BOOLEAN.
            var cardId = postgres ? "SERIAL PRIMARY KEY" : "INTEGER PRIMARY KEY AUTOINCREMENT";
            var bigInt = postgres ? "BIGINT" : "INTEGER";
            var admin = postgres ? "VARCHAR(5)" : "BOOLEAN";

            connection.Execute($@"CREATE TABLE IF NOT EXISTS Games 
                (Id VARCHAR(60) NOT NULL,
                CreatedBy VARCHAR(60) NOT NULL,
                Title VARCHAR(100) NOT NULL,
                Description VARCHAR(200) NOT NULL,
                RoundTime {bigInt} NOT NULL,
                Expiration {bigInt} NOT NULL)"
            );

            connection.Execute($@"CREATE TABLE IF NOT EXISTS Users(
                ConnectionId VARCHAR(200),
                Name VARCHAR(20) NOT NULL,
                GameId VARCHAR(60) NOT NULL,
                Admin {admin},
                Vote VARCHAR(10) NOT NULL)"
            );

            connection.Execute($@"CREATE TABLE IF NOT EXISTS Cards(
                Id {cardId},
                Value VARCHAR(5) NOT NULL)"
            );

            Seed(connection);
        }

        private static void Seed(IDbConnection connection)
        {
            if (ExistTableCards(connection)) return;
            var cards = new List<Card>
            {
                Card.Restore("?", 1),
                Card.Restore("coffee", 2),
                Card.Restore("0", 3),
                Card.Restore("0,5", 3),
                Card.Restore("1", 4),
                Card.Restore("2", 5),
                Card.Restore("3", 6),
                Card.Restore("5", 7),
                Card.Restore("8", 8),
                Card.Restore("13", 9),
                Card.Restore("20", 10),
                Card.Restore("40", 11),
                Card.Restore("100", 12),
            };
            connection.Execute("INSERT INTO Cards(Value) VALUES (@Value)", cards);
        }

        private static bool ExistTableCards(IDbConnection connection)
        {
            var exist = connection.Query<dynamic>("SELECT COUNT(*) AS \"Count\" FROM Cards");
            return exist.Single().Count > 1;
        }
    }
}
