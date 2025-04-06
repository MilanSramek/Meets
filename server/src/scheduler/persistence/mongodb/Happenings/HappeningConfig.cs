using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningConfig : IClassMapConfiguration<Happening>
{
    public void Configure(BsonClassMap<Happening> @event)
    {
        @event
            .MapProperty(_ => _.Name)
            .SetIsRequired(true);

        @event
            .MapProperty(_ => _.Description);

        @event
            .MapProperty(_ => _.Version)
            .SetIsRequired(true);
    }
}
