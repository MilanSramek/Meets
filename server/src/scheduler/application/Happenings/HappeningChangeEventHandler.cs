using Meets.Common.Domain;

using System.Collections.Concurrent;

namespace Meets.Scheduler.Happenings;

internal sealed class HappeningChangeEventHandler : IIntegrationEventHandler<HappeningChangedEvent>, IHappeningWatcherProvider
{
    private sealed class WatcherManager
    {
        private int _usageCount = 1;

        public WatcherManager(Lazy<Task<HappeningWatcher>> watcher)
        {
            Watcher = watcher ?? throw new ArgumentNullException(nameof(watcher));
        }

        public Lazy<Task<HappeningWatcher>> Watcher { get; }

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

    private readonly IReadOnlyRepository<Happening, Guid> _events;
    private readonly ConcurrentDictionary<Guid, WatcherManager> _eventWatchers = [];

    public HappeningChangeEventHandler(IReadOnlyRepository<Happening, Guid> events)
    {
        _events = events ?? throw new ArgumentNullException(nameof(events));
    }

    public async ValueTask HandleAsync(HappeningChangedEvent eventChangedEvent,
        CancellationToken cancellationToken)
    {
        while (_eventWatchers.TryGetValue(eventChangedEvent.Id, out var watcherManager))
        {
            if (watcherManager.UsageCount <= 0)
            {
                continue;
            }

            var eventSource = await watcherManager.Watcher.Value;
            await eventSource.RefreshAsync(eventChangedEvent.Version, cancellationToken);
            break;
        }
    }

    public async ValueTask<IAsyncEnumerable<HappeningModel>> GetWatcherAsync(Guid eventId,
        CancellationToken cancellationToken)
    {
        WatcherManager manager = new(new(CreateEventWatcherAsync(eventId)));
        WatcherManager resultingManager;
        do
        {
            resultingManager = _eventWatchers.GetOrAdd(eventId, manager);
            if (resultingManager == manager)
            {
                break;
            }
        }
        while (!resultingManager.IncrementUsageCount());

        return await resultingManager.Watcher.Value;
    }

    private async Task<HappeningWatcher> CreateEventWatcherAsync(Guid eventId)
    {
        // Default cancellation token due to the fact that the result can be awaited by many consumers
        var @event = await _events.GetAsync(eventId, default);

        return new HappeningWatcher(
            @event.MapToModel(),
            async cancellationToken => (await _events.GetAsync(eventId, cancellationToken)).MapToModel(),
            new HappeningWatcherUnsubscriber(eventId, UnsubscribeEventSourceAsync));
    }

    private ValueTask UnsubscribeEventSourceAsync(Guid eventId)
    {
        if (_eventWatchers.TryGetValue(eventId, out var watcherManager))
        {
            watcherManager.DecrementUsageCount();
            if (watcherManager.UsageCount <= 0)
            {
                _eventWatchers.TryRemove(eventId, out _);
            }
        }

        return ValueTask.CompletedTask;
    }
}
