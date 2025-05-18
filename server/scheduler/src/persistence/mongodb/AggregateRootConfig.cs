using Meets.Common.Domain;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Scheduler;

internal sealed class AggregateRootConfig : IClassMapConfiguration<AggregateRoot<Guid>>
{
    public void Configure(BsonClassMap<AggregateRoot<Guid>> root)
    {
    }
}
