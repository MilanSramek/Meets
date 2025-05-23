using Meets.Common.Domain;
using Meets.Common.Persistence.MongoDb;
using Meets.Scheduler.Activities;
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
        .AddMongoDb()
        .AddRepositories();

    private static IServiceCollection AddRepositories(this IServiceCollection services) => services
        .AddTransient<IActivityRepository, ActivitiesRepository>()
        .AddTransient<IReadOnlyRepository<Activity, Guid>, ActivitiesRepository>()
        .AddTransient<IPollRepository, PollRepository>()
        .AddTransient<IReadOnlyRepository<Poll, Guid>, PollRepository>()
        .AddTransient<IVoteRepository, VoteRepository>()
        .AddTransient<IReadOnlyRepository<Vote, Guid>, VoteRepository>();

    private static void RegisterMaps() => new ClassMapRegistry()
        .AddMap(new EntityConfig())
        .AddMap(new AggregateRootConfig())
        .AddMap(new ActivityConfig())
        .AddMap(new PollConfig())
        .AddMap(new VoteConfig())
        .AddMap(new VoteItemConfig());

    private static IServiceCollection AddMongoDb(this IServiceCollection services)
    {
        RegisterMaps();
        BsonSerializer.RegisterSerializer(new GuidSerializer(GuidRepresentation.Standard));
        return services
            .AddMongoDatabase();
    }
}
