using Meets.Common.Persistence.MongoDb;

using MongoDB.Bson.Serialization;

namespace Meets.Identity.Users;

internal sealed class UserConfig : IClassMapConfiguration<User>
{
    public void Configure(BsonClassMap<User> user)
    {
        user
            .MapProperty(_ => _.UserName)
            .SetIsRequired(true);

        user
            .MapProperty(_ => _.NormalizedUserName)
            .SetIsRequired(true);

        user
            .MapProperty(_ => _.PasswordHash);

        user
            .MapProperty(_ => _.SecurityStamp)
            .SetIsRequired(true);

        user
            .MapProperty(_ => _.ConcurrencyStamp)
            .SetIsRequired(true);
    }
}
