using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;

using MongoDB.Driver;

namespace Meets.Common.Persistence.MongoDb;

public static class ServiceCollectionExtensions1
{
    public static IServiceCollection AddMongoClient(this IServiceCollection services)
    {
        static MongoClient CreateMongoClient(IServiceProvider serviceProvider)
        {
            var options = serviceProvider
                .GetRequiredService<IOptions<MongoClientDbOptions>>().Value;

            return new MongoClient(new MongoClientSettings
            {
                Server = new MongoServerAddress(options.Host),
                Credential = MongoCredential.CreateCredential(
                    options.Database,
                    options.Username,
                    options.Password),
                DirectConnection = true
            });
        }

        services
            .TryAddSingleton<IMongoClient>(CreateMongoClient);

        return services;
    }

    public static IServiceCollection AddMongoDatabase(this IServiceCollection services)
    {
        services
            .AddMongoClient();
        services
            .TryAddSingleton(provider =>
            {
                var client = provider.GetRequiredService<IMongoClient>();
                var options = provider.GetRequiredService<IOptions<MongoClientDbOptions>>().Value;
                return client.GetDatabase(options.Database);
            });

        return services;
    }
}