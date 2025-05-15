using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityCreationManager : IActivityCreationManager
{
    private readonly IActivityRepository _activityRepository;
    private readonly IPollRepository _pollRepository;

    public ActivityCreationManager(IActivityRepository activityRepository, IPollRepository pollRepository)
    {
        _activityRepository = activityRepository
            ?? throw new ArgumentNullException(nameof(activityRepository));
        _pollRepository = pollRepository
            ?? throw new ArgumentNullException(nameof(pollRepository));
    }

    public async Task<Activity> CreateActivityAsync<TState>(
        string name,
        Action<TState, Activity> activitySetter,
        TState state,
        CancellationToken cancellationToken)
    {
        var activity = new Activity(name);
        activitySetter?.Invoke(state, activity);

        await _activityRepository.InsertAsync(activity, cancellationToken);
        await _pollRepository.InsertAsync(new Poll(activity.Id), cancellationToken);

        return activity;
    }
}
