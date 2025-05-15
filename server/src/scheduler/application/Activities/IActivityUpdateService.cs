namespace Meets.Scheduler.Activities;

public interface IActivityUpdateService
{
    public Task<ActivityModel> UpdateActivityAsync(Guid id, UpdateActivityInput input,
        CancellationToken cancellationToken);
}
