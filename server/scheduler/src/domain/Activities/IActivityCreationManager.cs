namespace Meets.Scheduler.Activities;

public interface IActivityCreationManager
{
    public Task<Activity> CreateActivityAsync<TState>(
        string name,
        Action<TState, Activity> activitySetter,
        TState state,
        CancellationToken cancellationToken);
}
