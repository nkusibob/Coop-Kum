using System;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;

public static class SqlServerTestDb
{
    // Use your local SQL Server settings here
    // IMPORTANT: do NOT include "Database=..." here because we will append it.
    private const string ServerConnection =
        "Server=.;Trusted_Connection=True;Encrypt=True;TrustServerCertificate=True;MultipleActiveResultSets=true;";

    public static string NewDbName()
        => $"Cooperative_Test_{Guid.NewGuid():N}";

    public static string ConnectionString(string dbName)
        => $"{ServerConnection}Database={dbName};";

    public static DbContextOptions<CooperativeContext> CoopOptions(string dbName)
        => new DbContextOptionsBuilder<CooperativeContext>()
            .UseSqlServer(ConnectionString(dbName))
            .EnableSensitiveDataLogging()
            .Options;

    public static DbContextOptions<ApplicationDbContext> IdentityOptions(string dbName)
        => new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseSqlServer(ConnectionString(dbName))
            .EnableSensitiveDataLogging()
            .Options;

    public static async Task CreateDatabaseAsync(string dbName)
    {
        await using var conn = new SqlConnection($"{ServerConnection}Database=master;");
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $@"
IF DB_ID(N'{dbName}') IS NULL
BEGIN
    CREATE DATABASE [{dbName}];
END";
        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task DropDatabaseAsync(string dbName)
    {
        await using var conn = new SqlConnection($"{ServerConnection}Database=master;");
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $@"
IF DB_ID(N'{dbName}') IS NOT NULL
BEGIN
    ALTER DATABASE [{dbName}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [{dbName}];
END";
        await cmd.ExecuteNonQueryAsync();
    }
}
