namespace Meets.Scheduler.Happenings;

internal readonly struct HappeningWatcherEnumeratorUnsubscriber
{
    private readonly Func<HappeningWatcherEnumerator, ValueTask> _unsubscribe;

    public HappeningWatcherEnumeratorUnsubscriber(Func<HappeningWatcherEnumerator, ValueTask> unsubscribe)
    {
        _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
    }

    public ValueTask UnsubscribeAsync(HappeningWatcherEnumerator watcher) => _unsubscribe(watcher);
}
