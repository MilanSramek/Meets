using Meets.Common.Application.Identity;
using Meets.Common.Domain;
using Meets.Scheduler.Activities;

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
        [Service] IIdentityContext identityContext,
        [Service] IReadOnlyRepository<Activity, Guid> activities,
        CancellationToken cancellationToken)
    {
        var userId = identityContext.UserId ?? throw new UnauthorizedException();
        var activity = await activities
            .Where(_ => _.OwnerId == userId)
            .ToListAsync(cancellationToken);
        return activity
            .Select(ActivityMapper.MapToModel);
    }
}
