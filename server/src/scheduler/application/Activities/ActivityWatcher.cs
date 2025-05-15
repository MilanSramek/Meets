using System.Collections.Concurrent;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityWatcher : IAsyncEnumerable<ActivityModel>
{
    private readonly Func<CancellationToken, Task<ActivityModel>> _activityProvider;
    private readonly ActivityWatcherUnsubscriber _unsubscriber;

    private readonly ConcurrentDictionary<ActivityWatcherEnumerator, ActivityWatcherEnumerator> _watchers = [];
    private int _lastActivityVersion;
    private bool _refreshInProgress;
    private ActivityModel _lastActivity;

    public ActivityWatcher(
        ActivityModel initialValue,
        Func<CancellationToken, Task<ActivityModel>> activityProvider,
        ActivityWatcherUnsubscriber unsubscriber)
    {
        _lastActivity = initialValue
            ?? throw new ArgumentNullException(nameof(initialValue));
        _activityProvider = activityProvider
            ?? throw new ArgumentNullException(nameof(activityProvider));
        _unsubscriber = unsubscriber;

        _lastActivityVersion = initialValue.Version;
    }

    public IAsyncEnumerator<ActivityModel> GetAsyncEnumerator(
        CancellationToken cancellationToken)
    {
        var watcher = new ActivityWatcherEnumerator(
            _lastActivity,
            new(UnsubscribeActivityWatcherAsync),
            cancellationToken);
        _watchers.TryAdd(watcher, watcher);

        return watcher;
    }

    public ValueTask RefreshAsync(int activityVersion, CancellationToken cancellationToken)
    {
        SetLastVersion(activityVersion);
        return PublishLastActivityAsync(cancellationToken);
    }

    private bool SetLastVersion(int activityVersion)
    {
        int lastActivityVersion;
        do
        {
            lastActivityVersion = _lastActivityVersion;
            if (lastActivityVersion >= activityVersion)
            {
                return false;
            }
        }
        while (lastActivityVersion != Interlocked
            .CompareExchange(ref _lastActivityVersion, activityVersion, lastActivityVersion));

        return true;

    }

    private async ValueTask PublishLastActivityAsync(CancellationToken cancellationToken)
    {
        int activityVersion = _lastActivityVersion;
        do
        {
            if (Interlocked.CompareExchange(ref _refreshInProgress, true, false))
            {
                break;
            }

            _lastActivity = await _activityProvider(cancellationToken);
            foreach (var watcher in _watchers.Values)
            {
                watcher.Publish(_lastActivity);
            }

            _refreshInProgress = false;
            activityVersion = _lastActivity.Version;
        }
        while (_lastActivityVersion > activityVersion);
    }

    private ValueTask UnsubscribeActivityWatcherAsync(ActivityWatcherEnumerator watcher)
    {
        _watchers.TryRemove(watcher, out _);
        return _watchers.IsEmpty
            ? _unsubscriber.UnsubscribeAsync()
            : ValueTask.CompletedTask;
    }
}
