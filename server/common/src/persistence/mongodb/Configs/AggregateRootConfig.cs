using Meets.Common.Domain;
using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Common.Persistence.MongoDb.Configs;

public sealed class AggregateGuidRootConfig : IClassMapConfiguration<AggregateRoot<Guid>>
{
    public void Configure(BsonClassMap<AggregateRoot<Guid>> root)
    {
    }
}
