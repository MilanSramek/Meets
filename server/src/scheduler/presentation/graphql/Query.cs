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

    // ToDo: Remove
    public async Task<IEnumerable<ActivityModel>> GetActivitiesAsync(
        [Service] IReadOnlyRepository<Activity, Guid> activities,
        CancellationToken cancellationToken)
    {
        var activity = await activities.ToListAsync(cancellationToken);
        return activity
            .Select(ActivityMapper.MapToModel);
    }
}
