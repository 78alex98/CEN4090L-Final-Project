using System.Data.Common;
using Bartering.Data.Database;
using Bartering.Tests.Integration.Configuration;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Npgsql;
using Respawn;

namespace Bartering.Tests.Integration.Fixtures;

public class BarteringDbFixture : BarteringTestWebAppFactory, IAsyncLifetime
{
    private DbConnection _dbConnection = null!;

    private Respawner _respawner = null!;

    public async Task InitializeAsync()
    {
        await Container.StartAsync();

        using (var scope = Services.CreateScope())
        {
            var context = scope.ServiceProvider.GetRequiredService<BarteringDbContext>();
            await context.Database.MigrateAsync();
        }

        _dbConnection = new NpgsqlConnection(Container.GetConnectionString());
        await _dbConnection.OpenAsync();
        _respawner = await Respawner.CreateAsync(
            _dbConnection,
            new RespawnerOptions { DbAdapter = DbAdapter.Postgres, SchemasToInclude = ["public"] }
        );
    }

    public new async Task DisposeAsync() => await Container.DisposeAsync();

    public async Task ResetDatabaseAsync() => await _respawner.ResetAsync(_dbConnection);
}
