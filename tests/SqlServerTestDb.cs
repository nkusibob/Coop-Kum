using System;
using Microsoft.Data.SqlClient; // if you're on Microsoft.Data.SqlClient, use that
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Model.Cooperative;

public static class SqlServerTestDb
{
    // Base server connection (master DB) using your config
    private const string MasterConn =
        "Server=.;Database=master;Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";

    public static string NewDbName() => $"Cooperative_Test_{Guid.NewGuid():N}";

    public static string DbConnectionString(string dbName) =>
        $"Server=.;Database={dbName};Trusted_Connection=True;MultipleActiveResultSets=true;Encrypt=True;TrustServerCertificate=True;";

    public static DbContextOptions<CooperativeContext> CoopOptions(string dbName) =>
        new DbContextOptionsBuilder<CooperativeContext>()
            .UseSqlServer(DbConnectionString(dbName))
            .Options;

    public static async Task CreateDatabaseAsync(string dbName)
    {
        await using var conn = new SqlConnection(MasterConn);
        await conn.OpenAsync();

        await using var cmd = conn.CreateCommand();
        cmd.CommandText = $"IF DB_ID(N'{dbName}') IS NULL CREATE DATABASE [{dbName}];";
        await cmd.ExecuteNonQueryAsync();
    }

    public static async Task DropDatabaseAsync(string dbName)
    {
        await using var conn = new SqlConnection(MasterConn);
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
