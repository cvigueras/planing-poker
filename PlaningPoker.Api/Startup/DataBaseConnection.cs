using Npgsql;
using System.Data;
using System.Data.SQLite;

namespace PlaningPoker.Api.Startup;

/// <summary>
/// Chooses the database: Postgres when DATABASE_URL is set (Render + Neon), SQLite file otherwise (local).
/// </summary>
public static class DataBaseConnection
{
    private const string SqliteConnectionString = "Data Source=./PlaningPoker.db";

    public static bool IsPostgres(string? databaseUrl) => !string.IsNullOrWhiteSpace(databaseUrl);

    public static IDbConnection Create(string? databaseUrl) =>
        IsPostgres(databaseUrl)
            ? new NpgsqlConnection(ToNpgsqlConnectionString(databaseUrl!))
            : new SQLiteConnection(SqliteConnectionString);

    // Converts "postgresql://user:password@host/dbname?sslmode=require" (the format Neon gives) to Npgsql's format.
    public static string ToNpgsqlConnectionString(string databaseUrl)
    {
        if (!databaseUrl.StartsWith("postgres", StringComparison.OrdinalIgnoreCase))
            return databaseUrl; // already in Npgsql format

        var uri = new Uri(databaseUrl);
        var userInfo = uri.UserInfo.Split(':', 2);
        return new NpgsqlConnectionStringBuilder
        {
            Host = uri.Host,
            Port = uri.Port > 0 ? uri.Port : 5432,
            Username = Uri.UnescapeDataString(userInfo[0]),
            Password = userInfo.Length > 1 ? Uri.UnescapeDataString(userInfo[1]) : null,
            Database = uri.AbsolutePath.TrimStart('/'),
            SslMode = SslMode.Require,
        }.ConnectionString;
    }
}
