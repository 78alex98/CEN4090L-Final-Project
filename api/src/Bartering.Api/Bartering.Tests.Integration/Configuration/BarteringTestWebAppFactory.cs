using Bartering.Data.Database;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Testcontainers.PostgreSql;

namespace Bartering.Tests.Integration.Configuration;

public class BarteringTestWebAppFactory : WebApplicationFactory<Program>
{
    private protected readonly PostgreSqlContainer Container = new PostgreSqlBuilder()
        .WithDatabase("bartering_test")
        .WithImage("postgres:17.4")
        .Build();

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(services =>
        {
            // Need to replace the app's DbContext with one configured for testing
            services.RemoveAll<DbContextOptions<BarteringDbContext>>();
            services.AddDbContext<BarteringDbContext>(options =>
                options.UseNpgsql(Container.GetConnectionString()).UseSnakeCaseNamingConvention()
            );
        });

        builder.UseEnvironment("Testing");

        // Needed because the application relies on configuration values for e.g. token keys.
        // It is possible to use builder.UseSetting() to add/replace a setting instead of doing this,
        //  but using a standard appsettings file should be easier to configure in the long run.
        builder.ConfigureAppConfiguration(configuration =>
        {
            // Need to set the base path because 'AddJsonFile' will look in the Core project otherwise.
            configuration.SetBasePath(Directory.GetCurrentDirectory());
            configuration.AddJsonFile("appsettings.Testing.json");
        });
    }
}
