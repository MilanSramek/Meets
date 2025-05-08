using Meets.Common.Domain;
using Meets.Common.Persistence.MongoDb;
using Meets.Scheduler.Happenings;
using Meets.Scheduler.Polls;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Driver;

namespace Meets.Scheduler;

public static class Registrations
{
    public static IServiceCollection AddMongoDbPersistence(this IServiceCollection services) => services
        .AddMongoClient()
        .AddRepositories();

    private static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddTransient<IHappeningRepository, HappeningsRepository>()
        .AddTransient<IReadOnlyRepository<Happening, Guid>, HappeningsRepository>()
        .AddTransient<IPollRepository, PollRepository>()
        .AddTransient<IReadOnlyRepository<Poll, Guid>, PollRepository>()
        .AddTransient<IVoteRepository, VoteRepository>()
        .AddTransient<IReadOnlyRepository<Vote, Guid>, VoteRepository>();

    private static void RegisterMaps() => new ClassMapRegistry()
        .AddMap(new EntityConfig())
        .AddMap(new AggregateRootConfig())
        .AddMap(new HappeningConfig())
        .AddMap(new PollConfig())
        .AddMap(new VoteConfig())
        .AddMap(new VoteItemConfig());

    private static IServiceCollection AddMongoClient(this IServiceCollection services)
    {
        static MongoClient CreateMongoClient(IServiceProvider serviceProvider)
        {
            var options = serviceProvider.GetRequiredService<IOptions<MongoClientDbOptions>>().Value;

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

        RegisterMaps();
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return services
            .AddSingleton<IMongoClient>(CreateMongoClient)
            .AddTransient(serviceProvider =>
            {
                var client = serviceProvider.GetRequiredService<IMongoClient>();
                var options = serviceProvider.GetRequiredService<IOptions<MongoClientDbOptions>>().Value;
                return client.GetDatabase(options.Database);
            });
    }
}
