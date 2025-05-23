using Meets.Common.Persistence.MongoDb;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Identity;

public static class Registrations
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services)
    {
        return services
            .AddMongoDatabase()
            .AddOpenIddictStorage();
    }

    private static IServiceCollection AddOpenIddictStorage(
        this IServiceCollection services)
    {
        services
            .AddOpenIddict()
            .AddCore(options =>
            {
                options.UseMongoDb();
            });

        return services;
    }
}
