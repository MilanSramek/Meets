using MongoDB.Bson.Serialization;

namespace Meets.Common.Persistence.MongoDb;

public interface IClassMapConfiguration<T>
{
    public void Configure(BsonClassMap<T> builder);
}
