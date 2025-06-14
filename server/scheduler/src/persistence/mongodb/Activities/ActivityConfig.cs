using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityConfig : IClassMapConfiguration<Activity>
{
    public void Configure(BsonClassMap<Activity> activity)
    {
        activity
            .MapProperty(_ => _.Name)
            .SetIsRequired(true);

        activity
            .MapProperty(_ => _.Description);

        activity
            .MapProperty(_ => _.Version)
            .SetIsRequired(true);

        activity
            .MapProperty(_ => _.OwnerId);
    }
}
