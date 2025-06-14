namespace Meets.Scheduler.Activities;

public interface IActivityCreationService
{
    public Task<ActivityModel> CreateActivityAsync(CreateActivityModel input,
        CancellationToken cancellationToken);
}
