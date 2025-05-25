using Meets.Common.Domain;
using Meets.Common.Persistence.MongoDb;
using Meets.Common.Persistence.MongoDb.Configs;
using Meets.Identity.Users;

using Microsoft.Extensions.DependencyInjection;

using MongoDB.Bson;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;

namespace Meets.Identity;

public static class Registrations1
{
    public static IServiceCollection AddPersistence(
        this IServiceCollection services)
    {
        return services
            .AddMongoDb()
            .AddRepositories()
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

    private static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddTransient<IUserRepository, UserRepository>()
        .AddTransient<IReadOnlyRepository<User, Guid>, UserRepository>();


    private static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        RegisterMaps();
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return services
            .AddMongoDatabase();
    }

    private static void RegisterMaps() => new ClassMapRegistry()
        .AddMap(new EntityGuidConfig())
        .AddMap(new AggregateGuidRootConfig())
        .AddMap(new UserConfig());
}
