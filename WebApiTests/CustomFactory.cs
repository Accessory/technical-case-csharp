using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Moq;
using Npgsql;
using Testcontainers.PostgreSql;
using WebApi.Helpers;
using Xunit;

namespace WebApiTests;

public class CustomFactory : WebApplicationFactory<Program>, IAsyncLifetime
{
    private readonly PostgreSqlContainer _postgres = new PostgreSqlBuilder()
        .WithImage("postgres:latest")
        .WithDatabase("RobotSharpTest")
        .WithUsername("tibber")
        .WithPassword("tibber")
        .WithCleanUp(true)
        .Build();

    public Task InitializeAsync()
    {
        // Environment.SetEnvironmentVariable("DATABASE_SERVER", );
        // Environment.SetEnvironmentVariable("DATABASE_DATABASE", "RobotSharp");
        // Environment.SetEnvironmentVariable("DATABASE_USER", "tibber");
        // Environment.SetEnvironmentVariable("DATABASE_PASSWORD", "tibber");
        return _postgres.StartAsync();
    }

    public new Task DisposeAsync()
    {
        return _postgres.DisposeAsync().AsTask();
    }

    private static NpgsqlConnection? _connection = null;

    private NpgsqlConnection GetConnection()
    {
        return new NpgsqlConnection(_postgres.GetConnectionString());
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            services.Configure<DbSettings>(s =>
            {
                s.Database = "RobotSharpTest";
                s.Password = "tibber";
                s.UserId = "tibber";
                s.Server = "127.0.0.1";
            });
            var dbSettings = new DbSettings()
            {
                Database = "RobotSharpTest",
                Password = "tibber",
                UserId = "tibber",
                Server = "127.0.0.1",
            };
            
            var options = Options.Create<DbSettings>(dbSettings);
            
            var dataContext = new Mock<DataContext>(options);
            dataContext.SetupSequence(o => o.CreateConnection())
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection)
                .Returns(GetConnection);
               
            services.AddTransient(_ => dataContext.Object);
        });
    }
}