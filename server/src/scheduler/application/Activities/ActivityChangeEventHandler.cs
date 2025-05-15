using Meets.Common.Domain;

using System.Collections.Concurrent;

namespace Meets.Scheduler.Activities;

internal sealed class ActivityChangeEventHandler : IIntegrationEventHandler<ActivityChangedEvent>, IActivityWatcherProvider
{
    private sealed class WatcherManager
    {
        private int _usageCount = 1;

        public WatcherManager(Lazy<Task<ActivityWatcher>> watcher)
        {
            Watcher = watcher ?? throw new ArgumentNullException(nameof(watcher));
        }

        public Lazy<Task<ActivityWatcher>> Watcher { get; }

        public int UsageCount => _usageCount;

        public bool IncrementUsageCount()
        {
            int usageCount;
            int incrementedUsageCount;
            do
            {
                usageCount = _usageCount;
                if (usageCount <= 0)
                {
                    return false;
                }

                incrementedUsageCount = usageCount + 1;
            }
            while (usageCount != Interlocked.CompareExchange(ref _usageCount, incrementedUsageCount, usageCount));

            return true;
        }

        public void DecrementUsageCount()
        {
            Interlocked.Decrement(ref _usageCount);
        }
    }

    private readonly IReadOnlyRepository<Activity, Guid> _activities;
    private readonly ConcurrentDictionary<Guid, WatcherManager> _activityWatchers = [];

    public ActivityChangeEventHandler(IReadOnlyRepository<Activity, Guid> activities)
    {
        _activities = activities ?? throw new ArgumentNullException(nameof(activities));
    }

    public async ValueTask HandleAsync(ActivityChangedEvent activityChangedEvent,
        CancellationToken cancellationToken)
    {
        while (_activityWatchers.TryGetValue(activityChangedEvent.Id, out var watcherManager))
        {
            if (watcherManager.UsageCount <= 0)
            {
                continue;
            }

            var activitySource = await watcherManager.Watcher.Value;
            await activitySource.RefreshAsync(activityChangedEvent.Version, cancellationToken);
            break;
        }
    }

    public async ValueTask<IAsyncEnumerable<ActivityModel>> GetWatcherAsync(Guid activityId,
        CancellationToken cancellationToken)
    {
        WatcherManager manager = new(new(CreateActivityWatcherAsync(activityId)));
        WatcherManager resultingManager;
        do
        {
            resultingManager = _activityWatchers.GetOrAdd(activityId, manager);
            if (resultingManager == manager)
            {
                break;
            }
        }
        while (!resultingManager.IncrementUsageCount());

        return await resultingManager.Watcher.Value;
    }

    private async Task<ActivityWatcher> CreateActivityWatcherAsync(Guid activityId)
    {
        // Default cancellation token due to the fact that the result can be awaited by many consumers
        var activity = await _activities.GetAsync(activityId, default);

        return new ActivityWatcher(
            activity.MapToModel(),
            async cancellationToken => (await _activities.GetAsync(activityId, cancellationToken)).MapToModel(),
            new ActivityWatcherUnsubscriber(activityId, UnsubscribeActivitySourceAsync));
    }

    private ValueTask UnsubscribeActivitySourceAsync(Guid activityId)
    {
        if (_activityWatchers.TryGetValue(activityId, out var watcherManager))
        {
            watcherManager.DecrementUsageCount();
            if (watcherManager.UsageCount <= 0)
            {
                _activityWatchers.TryRemove(activityId, out _);
            }
        }

        return ValueTask.CompletedTask;
    }
}
