using MongoDB.Driver;

namespace Meets.Common.Persistence.MongoDb;

internal static class MongoDatabaseExtensions
{
    public static IMongoCollection<TEntity> GetCollection<TEntity>(this IMongoDatabase database) =>
        database.GetCollection<TEntity>(typeof(TEntity).Name);
}
