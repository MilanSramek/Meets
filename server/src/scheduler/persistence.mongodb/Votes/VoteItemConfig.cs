using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler.Votes;

internal sealed class VoteItemConfig : IClassMapConfiguration<VoteItem>
{
    public void Configure(BsonClassMap<VoteItem> item)
    {
        item
            .MapProperty(_ => _.Date)
            .SetIsRequired(true);

        item
            .MapProperty(_ => _.Statement)
            .SetIsRequired(true);
    }
}
