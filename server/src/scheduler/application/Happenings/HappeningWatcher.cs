using System.Collections.Concurrent;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningWatcher : IAsyncEnumerable<HappeningModel>
{
    private readonly Func<CancellationToken, Task<HappeningModel>> _eventProvider;
    private readonly HappeningWatcherUnsubscriber _unsubscriber;

    private readonly ConcurrentDictionary<HappeningWatcherEnumerator, HappeningWatcherEnumerator> _watchers = [];
    private int _lastEventVersion;
    private bool _refreshInProgress;
    private HappeningModel _lastEvent;

    public HappeningWatcher(
        HappeningModel initialValue,
        Func<CancellationToken, Task<HappeningModel>> eventProvider,
        HappeningWatcherUnsubscriber unsubscriber)
    {
        _lastEvent = initialValue
            ?? throw new ArgumentNullException(nameof(initialValue));
        _eventProvider = eventProvider
            ?? throw new ArgumentNullException(nameof(eventProvider));
        _unsubscriber = unsubscriber;

        _lastEventVersion = initialValue.Version;
    }

    public IAsyncEnumerator<HappeningModel> GetAsyncEnumerator(
        CancellationToken cancellationToken)
    {
        var watcher = new HappeningWatcherEnumerator(
            _lastEvent,
            new(UnsubscribeEventWatcherAsync),
            cancellationToken);
        _watchers.TryAdd(watcher, watcher);

        return watcher;
    }

    public ValueTask RefreshAsync(int eventVersion, CancellationToken cancellationToken)
    {
        SetLastVersion(eventVersion);
        return PublishLastEventAsync(cancellationToken);
    }

    private bool SetLastVersion(int eventVersion)
    {
        int lastEventVersion;
        do
        {
            lastEventVersion = _lastEventVersion;
            if (lastEventVersion >= eventVersion)
            {
                return false;
            }
        }
        while (lastEventVersion != Interlocked
            .CompareExchange(ref _lastEventVersion, eventVersion, lastEventVersion));

        return true;

    }

    private async ValueTask PublishLastEventAsync(CancellationToken cancellationToken)
    {
        int eventVersion = _lastEventVersion;
        do
        {
            if (Interlocked.CompareExchange(ref _refreshInProgress, true, false))
            {
                break;
            }

            _lastEvent = await _eventProvider(cancellationToken);
            foreach (var watcher in _watchers.Values)
            {
                watcher.Publish(_lastEvent);
            }

            _refreshInProgress = false;
            eventVersion = _lastEvent.Version;
        }
        while (_lastEventVersion > eventVersion);
    }

    private ValueTask UnsubscribeEventWatcherAsync(HappeningWatcherEnumerator watcher)
    {
        _watchers.TryRemove(watcher, out _);
        return _watchers.IsEmpty
            ? _unsubscriber.UnsubscribeAsync()
            : ValueTask.CompletedTask;
    }
}
