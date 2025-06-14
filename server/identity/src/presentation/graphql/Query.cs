using Meets.Common.Domain;
using Meets.Identity.Users;

namespace Meets.Identity;

internal sealed class Query
{
    public async Task<UserModel> GetUserAsync(
        Guid id,
        [Service] IReadOnlyRepository<User, Guid> users,
        CancellationToken cancellationToken)
    {
        var user = await users.GetAsync(id, cancellationToken);  // ToDo: DataLoader
        return user.MapToModel();
    }
}
