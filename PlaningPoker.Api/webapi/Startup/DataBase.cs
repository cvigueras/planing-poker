using Dapper;
using System.Data.SQLite;

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

            connection.Close();
            connection.Dispose();
        }
    }
}