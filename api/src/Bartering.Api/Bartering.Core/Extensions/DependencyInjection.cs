using Bartering.Core.Features.Authentication;
using Bartering.Core.Features.Bids;
using Bartering.Core.Features.Items;
using Bartering.Core.Features.Listings;
using Bartering.Data.Database;
using Microsoft.EntityFrameworkCore;

namespace Bartering.Core.Extensions;

public static class DependencyInjection
{
    public static IServiceCollection AddDatabaseContexts(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<BarteringDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("BarteringContext")).UseSnakeCaseNamingConvention()
        );

        services.AddScoped<BarteringDbContext, BarteringDbContext>();

        return services;
    }

    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddScoped<IAuthenticationService, AuthenticationService>();
        services.AddScoped<IItemService, ItemService>();
        services.AddScoped<IListingService, ListingService>();
        services.AddScoped<IBidService, BidService>();

        return services;
    }
}
