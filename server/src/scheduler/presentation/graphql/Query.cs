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
        var Activity = await activities.GetAsync(id, cancellationToken);  // ToDo: DataLoader
        return Activity.MapToModel();
    }

    // ToDo: Remove
    public async Task<IEnumerable<ActivityModel>> GetActivitiesAsync(
        [Service] IReadOnlyRepository<Activity, Guid> activities,
        CancellationToken cancellationToken)
    {
        var Activity = await activities.ToListAsync(cancellationToken);
        return Activity
            .Select(ActivityMapper.MapToModel);
    }
}
