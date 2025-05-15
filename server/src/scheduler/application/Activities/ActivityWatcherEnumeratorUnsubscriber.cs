namespace Meets.Scheduler.Activities;

internal readonly struct ActivityWatcherEnumeratorUnsubscriber
{
    private readonly Func<ActivityWatcherEnumerator, ValueTask> _unsubscribe;

    public ActivityWatcherEnumeratorUnsubscriber(Func<ActivityWatcherEnumerator, ValueTask> unsubscribe)
    {
        _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
    }

    public ValueTask UnsubscribeAsync(ActivityWatcherEnumerator watcher) => _unsubscribe(watcher);
}
