using Dapper;
using PlaningPoker.Api.Test.Cards.Fixtures;
using System.Data.SQLite;

namespace PlaningPoker.Api.Test.Startup
{
    public class DataBaseStartUp
    {
        private readonly SQLiteConnection connection;
        public DataBaseStartUp(SQLiteConnection connection)
        {
            this.connection = connection;

            Create();

            Seed();
        }

        private void Create()
        {
            connection.Execute(@"CREATE TABLE IF NOT EXISTS Games(
                Id VARCHAR(60) NOT NULL,
                CreatedBy VARCHAR(60) NOT NULL,
                Title VARCHAR(100) NOT NULL,
                Description VARCHAR(200) NOT NULL,
                RoundTime INTEGER NOT NULL,
                Expiration INTEGER NOT NULL)"
            );

            connection.Execute(@"CREATE TABLE IF NOT EXISTS Users(
                ConnectionId VARCHAR(200),
                Name VARCHAR(20) NOT NULL,
                GameId VARCHAR(60) NOT NULL,
                Admin BOOLEAN,
                Vote VARCHAR(10) NOT NULL)"
            );

            connection.Execute(@"CREATE TABLE IF NOT EXISTS Cards(
                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                Value VARCHAR(5) NOT NULL)"
            );
        }

        private void Seed()
        {
            var cards = CardMother.GetAll();
            connection.Execute("INSERT INTO Cards(Value) VALUES (@Value)", cards);
        }
    }
}
