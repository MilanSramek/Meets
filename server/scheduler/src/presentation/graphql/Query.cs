using Meets.Common.Domain;
using Meets.Scheduler.Activities;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Meets.Scheduler;

internal sealed class Query
{
    public async Task<ActivityModel> GetActivityAsync(
        Guid id,
        [Service] IReadOnlyRepository<Activity, Guid> activities,
        CancellationToken cancellationToken)
    {
        var activity = await activities.GetAsync(id, cancellationToken);  // ToDo: DataLoader
        return activity.MapToModel();
    }

    public async Task<IEnumerable<ActivityModel>> GetActivitiesAsync(
        [Service] IHttpContextAccessor httpContextAccessor,
        [Service] IReadOnlyRepository<Activity, Guid> activities,
        [Service] ILogger<Query> logger,
        CancellationToken cancellationToken)
    {
        // ToDo: IdentityContext
        var user = httpContextAccessor.HttpContext?.User;
        string? rawUserId = user?.FindFirst("sub")?.Value;
        if (rawUserId is null || !Guid.TryParse(rawUserId, out var userId))
        {
            logger.LogError(
                "Failed to parse user ID from claims: {RawUserId}",
                rawUserId);
            throw new GraphQLException(ErrorBuilder.New()
                .SetMessage("You are not allowed to access this resource.")
                .SetCode("FORBIDDEN")
                .Build());
        }
        var activity = await activities
            .Where(_ => _.OwnerId == userId)
            .ToListAsync(cancellationToken);
        return activity
            .Select(ActivityMapper.MapToModel);
    }
}
