namespace Meets.Scheduler.Activities;

internal readonly struct ActivityWatcherUnsubscriber
{
    private readonly Guid _activityId;
    private readonly Func<Guid, ValueTask> _unsubscribe;

    public ActivityWatcherUnsubscriber(Guid activityId, Func<Guid, ValueTask> unsubscribe)
    {
        _activityId = activityId;
        _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
    }

    public ValueTask UnsubscribeAsync() => _unsubscribe(_activityId);
}
