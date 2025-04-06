using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using System.Collections.Concurrent;

namespace Meets.Scheduler.Happenings;

internal class HappeningChangeEventHandler2 :
    IIntegrationEventHandler<HappeningChangedEvent>,
    IAsyncObservable<HappeningChangedEvent>
{
    private sealed class Subscription : IAsyncDisposable
    {
        private readonly IAsyncObserver<HappeningChangedEvent> _observer;

        private readonly HappeningChangeEventHandler2 _handler;

        public Subscription(
            IAsyncObserver<HappeningChangedEvent> observer,
            HappeningChangeEventHandler2 handler)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _handler = handler ?? throw new ArgumentNullException(nameof(handler));
        }

        public ValueTask DisposeAsync() => _handler._observers.TryRemove(_observer, out _)
            ? ValueTask.CompletedTask
            : throw new InvalidOperationException("Observer has been unsubscribed.");
    }

    private readonly ConcurrentDictionary<IAsyncObserver<HappeningChangedEvent>,
        IAsyncObserver<HappeningChangedEvent>> _observers = [];

    public async ValueTask HandleAsync(HappeningChangedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var observer in _observers.Values)
        {
            await observer.OnNextAsync(@event, cancellationToken);
        }
    }

    public IAsyncDisposable SubscribeAsync(
        IAsyncObserver<HappeningChangedEvent> observer)
    {
        return _observers.TryAdd(observer, observer)
            ? new Subscription(observer, this)
            : throw new InvalidOperationException("Observer has been subscribed.");
    }
}
