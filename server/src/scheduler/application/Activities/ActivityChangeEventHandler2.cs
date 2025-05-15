using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using System.Collections.Concurrent;

namespace Meets.Scheduler.Activities;

internal class ActivityChangeEventHandler2 :
    IIntegrationEventHandler<ActivityChangedEvent>,
    IAsyncObservable<ActivityChangedEvent>
{
    private sealed class Subscription : IAsyncDisposable
    {
        private readonly IAsyncObserver<ActivityChangedEvent> _observer;

        private readonly ActivityChangeEventHandler2 _handler;

        public Subscription(
            IAsyncObserver<ActivityChangedEvent> observer,
            ActivityChangeEventHandler2 handler)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public ValueTask DisposeAsync() => _handler._observers.TryRemove(_observer, out _)
            ? ValueTask.CompletedTask
            : throw new InvalidOperationException("Observer has been unsubscribed.");
    }

    private readonly ConcurrentDictionary<IAsyncObserver<ActivityChangedEvent>,
        IAsyncObserver<ActivityChangedEvent>> _observers = [];

    public async ValueTask HandleAsync(ActivityChangedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var observer in _observers.Values)
        {
            await observer.OnNextAsync(@event, cancellationToken);
        }
    }

    public IAsyncDisposable SubscribeAsync(
        IAsyncObserver<ActivityChangedEvent> observer)
    {
        return _observers.TryAdd(observer, observer)
            ? new Subscription(observer, this)
            : throw new InvalidOperationException("Observer has been subscribed.");
    }
}
