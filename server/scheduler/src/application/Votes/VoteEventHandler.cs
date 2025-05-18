using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using System.Collections.Concurrent;

namespace Meets.Scheduler.Votes;

internal sealed class VoteEventHandler :
    IIntegrationEventHandler<VoteCreatedEvent>,
    IIntegrationEventHandler<VoteChangedEvent>,
    IIntegrationEventHandler<VoteDeletedEvent>,
    IAsyncObservable<VoteCreatedEvent>,
    IAsyncObservable<VoteChangedEvent>,
    IAsyncObservable<VoteDeletedEvent>
{
    private sealed class Subscription<TEvent> : IAsyncDisposable
    {
        private readonly IAsyncObserver<TEvent> _observer;

        private readonly IDictionary<IAsyncObserver<TEvent>, IAsyncObserver<TEvent>> _observers;

        public Subscription(
            IAsyncObserver<TEvent> observer,
            IDictionary<IAsyncObserver<TEvent>, IAsyncObserver<TEvent>> observers)
        {
            _observer = observer ?? throw new ArgumentNullException(nameof(observer));
            _observers = observers ?? throw new ArgumentNullException(nameof(observers));
        }

        public ValueTask DisposeAsync() => _observers.Remove(_observer)
            ? ValueTask.CompletedTask
            : throw new InvalidOperationException("Observer has been unsubscribed.");
    }

    private readonly ConcurrentDictionary<IAsyncObserver<VoteCreatedEvent>,
        IAsyncObserver<VoteCreatedEvent>> _createObservers = [];

    private readonly ConcurrentDictionary<IAsyncObserver<VoteChangedEvent>,
        IAsyncObserver<VoteChangedEvent>> _changeObservers = [];

    private readonly ConcurrentDictionary<IAsyncObserver<VoteDeletedEvent>,
        IAsyncObserver<VoteDeletedEvent>> _deleteObservers = [];

    public async ValueTask HandleAsync(VoteChangedEvent @event,
        CancellationToken cancellationToken)
    {
        foreach (var observer in _changeObservers.Values)
        {
            await observer.OnNextAsync(@event, cancellationToken);
        }
    }

    public IAsyncDisposable SubscribeAsync(IAsyncObserver<VoteChangedEvent> observer)
    {
        return _changeObservers.TryAdd(observer, observer)
            ? new Subscription<VoteChangedEvent>(observer, _changeObservers)
            : throw new InvalidOperationException("Observer has been subscribed.");
    }

    public async ValueTask HandleAsync(VoteCreatedEvent @event, CancellationToken cancellationToken)
    {
        foreach (var observer in _createObservers.Values)
        {
            await observer.OnNextAsync(@event, cancellationToken);
        }
    }

    public IAsyncDisposable SubscribeAsync(IAsyncObserver<VoteCreatedEvent> observer)
    {
        return _createObservers.TryAdd(observer, observer)
            ? new Subscription<VoteCreatedEvent>(observer, _createObservers)
            : throw new InvalidOperationException("Observer has been subscribed.");
    }

    public async ValueTask HandleAsync(VoteDeletedEvent @event, CancellationToken cancellationToken)
    {
        foreach (var observer in _deleteObservers.Values)
        {
            await observer.OnNextAsync(@event, cancellationToken);
        }
    }

    public IAsyncDisposable SubscribeAsync(IAsyncObserver<VoteDeletedEvent> observer)
    {
        return _deleteObservers.TryAdd(observer, observer)
            ? new Subscription<VoteDeletedEvent>(observer, _deleteObservers)
            : throw new InvalidOperationException("Observer has been subscribed.");
    }
}
