using MongoDB.Bson.Serialization;

namespace Meets.Common.Persistence.MongoDb;

public readonly struct ClassMapRegistry
{
    public ClassMapRegistry AddMap<T>(IClassMapConfiguration<T> config)
    {
        BsonClassMap.RegisterClassMap<T>(config.Configure);
        return this;
    }
}
