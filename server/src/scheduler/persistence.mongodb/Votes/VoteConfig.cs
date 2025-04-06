using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler.Votes;

internal sealed class VoteConfig : IClassMapConfiguration<Vote>
{
    public void Configure(BsonClassMap<Vote> vote)
    {
        vote
            .MapProperty(_ => _.PollId)
            .SetIsRequired(true);

        vote
            .MapField("_items")
            .SetIsRequired(true);

        vote
            .MapProperty(_ => _.Version)
            .SetIsRequired(true);
    }
}
