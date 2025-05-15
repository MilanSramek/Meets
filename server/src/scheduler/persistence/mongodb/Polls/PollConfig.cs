using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler.Polls;

internal sealed class PollConfig : IClassMapConfiguration<Poll>
{
    public void Configure(BsonClassMap<Poll> poll)
    {
        poll
            .MapProperty(_ => _.ActivityId)
            .SetIsRequired(true);
    }
}
