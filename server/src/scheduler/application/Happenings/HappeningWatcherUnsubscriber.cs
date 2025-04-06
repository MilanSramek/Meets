namespace Meets.Scheduler.Happenings;

internal readonly struct HappeningWatcherUnsubscriber
{
    private readonly Guid _eventId;
    private readonly Func<Guid, ValueTask> _unsubscribe;

    public HappeningWatcherUnsubscriber(Guid eventId, Func<Guid, ValueTask> unsubscribe)
    {
        _eventId = eventId;
        _unsubscribe = unsubscribe ?? throw new ArgumentNullException(nameof(unsubscribe));
    }

    public ValueTask UnsubscribeAsync() => _unsubscribe(_eventId);
}
