using Bartering.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Core.Extensions;

public static class MigrationExtensions
{
    /// <summary>
    /// Applies a migration for the context to the database.
    /// </summary>
    /// <param name="host">The <see cref="T:Microsoft.Extensions.Hosting.IHost" /> instance this method extends.</param>
    /// <param name="targetMigration">The target migration to apply to. Applies the latest migration if null.</param>
    /// <remarks>Expects <see cref="T:Bartering.Data.Database.BarteringDbContext"/>  to have been registered to
    ///     <see cref="Microsoft.Extensions.DependencyInjection.IServiceCollection"/>.</remarks>
    public static void ApplyMigration(this IHost host, string? targetMigration = null)
    {
        using var scope = host.Services.CreateScope();
        var context = scope.ServiceProvider.GetService<BarteringDbContext>();
        try
        {
            context?.Database.Migrate(targetMigration);
        }
        catch (Exception e)
        {
            var logger = scope
                .ServiceProvider.GetService<ILoggerFactory>()
                ?.CreateLogger("Bartering.Core.Extensions.MigrationExtensions");

            targetMigration ??= "latest";

            logger?.LogError(
                e,
                "Could not apply {targetMigration} migration to the database. Check to see if the database is running.",
                targetMigration
            );
        }
    }
}
