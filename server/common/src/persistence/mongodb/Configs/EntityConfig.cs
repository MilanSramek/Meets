using Meets.Common.Domain;

using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.IdGenerators;

namespace Meets.Common.Persistence.MongoDb.Configs;

public sealed class EntityGuidConfig : IClassMapConfiguration<Entity<Guid>>
{
    public void Configure(BsonClassMap<Entity<Guid>> entity)
    {
        entity
            .MapProperty(_ => _.Id)
            .SetIsRequired(true)
            .SetElementName("Id")
            .SetIdGenerator(GuidGenerator.Instance);
        entity
            .SetIdMember(entity.GetMemberMap(_ => _.Id));

        entity
            .SetIsRootClass(true);
    }
}
