namespace Meets.Scheduler.Activities;

public interface IActivityUpdateService
{
    public Task<ActivityModel> UpdateActivityAsync(Guid id, UpdateActivityModel input,
        CancellationToken cancellationToken);
}
