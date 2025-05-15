namespace Meets.Scheduler.Activities;

public interface IActivityCreationService
{
    public Task<ActivityModel> CreateActivityAsync(CreateActivityInput input,
        CancellationToken cancellationToken);
}
