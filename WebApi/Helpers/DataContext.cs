namespace WebApi.Helpers;

using System.Data;
using Dapper;
using Microsoft.Extensions.Options;
using Npgsql;

public class DataContext
{
    private DbSettings _dbSettings;

    public DataContext(IOptions<DbSettings> dbSettings)
    {
        _dbSettings = dbSettings.Value;
    }

    public virtual IDbConnection CreateConnection()
    {
        var server = IsNullOrDefault(Environment.GetEnvironmentVariable("DATABASE_SERVER"), _dbSettings.Server!);
        var database = IsNullOrDefault(Environment.GetEnvironmentVariable("DATABASE_DATABASE"), _dbSettings.Server!);
        var userId = IsNullOrDefault(Environment.GetEnvironmentVariable("DATABASE_USER"), _dbSettings.Server!);
        var password = IsNullOrDefault(Environment.GetEnvironmentVariable("DATABASE_PASSWORD"), _dbSettings.Server!);
        var connectionString = $"Host={server}; Database={database}; Username={userId}; Password={password};";
        return new NpgsqlConnection(connectionString);
    }

    private string IsNullOrDefault(string? value, string defaultValue)
    {
        return value ?? defaultValue;
    }

    public async Task Init()
    {
        await _initDatabase();
        _initTables();
    }

    private async Task _initDatabase()
    {
        // create database if it doesn't exist
        using var connection = CreateConnection();
        var database = IsNullOrDefault(Environment.GetEnvironmentVariable("DATABASE_DATABASE"), _dbSettings.Server!);
        var sqlDbCount = $"SELECT COUNT(*) FROM pg_database WHERE datname = '{database}';";
        var dbCount = await connection.ExecuteScalarAsync<int>(sqlDbCount);
        if (dbCount == 0)
        {
            var sql = $"CREATE DATABASE \"{database}\"";
            await connection.ExecuteAsync(sql);
        }
    }

    private void _initTables()
    {
        // create tables if they don't exist
        const string sql = """
                               CREATE TABLE IF NOT EXISTS public.job
                           (
                               id BIGSERIAL NOT NULL,
                               "timestamp" timestamp without time zone NOT NULL,
                               commands bigint NOT NULL,
                               result bigint NOT NULL,
                               duration double precision NOT NULL,
                               CONSTRAINT jobs_pkey PRIMARY KEY (id)
                           );
                           """;
        CreateConnection().Execute(sql);
    }
}