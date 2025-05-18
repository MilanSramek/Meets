using Meets.Common.Domain;

using System.Collections.Concurrent;

namespace Meets.Common.Infrastructure;

internal sealed class IntegrationEventBus : IIntegrationEventBus, IIntegrationEventSubscribePoint
{
    public sealed class Subscription : IDisposable
    {
        private readonly ConcurrentDictionary<Type, Delegate> _handlers;
        private readonly Type _eventType;

        public Subscription(ConcurrentDictionary<Type, Delegate> handlers, Type eventType)
        {
            _handlers = handlers ?? throw new ArgumentNullException(nameof(handlers));
            _eventType = eventType ?? throw new ArgumentNullException(nameof(eventType));
        }

        public void Dispose() => _handlers.TryRemove(_eventType, out _);
    }

    private readonly ConcurrentDictionary<Type, Delegate> _handlers = new();

    public async ValueTask PublishAsync(object @event, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(@event);
        if (_handlers.TryGetValue(@event.GetType(), out var handler))
        {
            await (ValueTask)handler.DynamicInvoke(@event, cancellationToken)!;
        }
    }

    public IDisposable Subscribe<TEvent>(
        IIntegrationEventHandler<TEvent> handler)
    {
        ArgumentNullException.ThrowIfNull(handler);

        var eventType = typeof(TEvent);
        Delegate @delegate = handler.HandleAsync;
        return _handlers.TryAdd(eventType, @delegate)
            ? new Subscription(_handlers, eventType)
            : throw new InvalidOperationException($"Handler for event type '{eventType}' already registered.");
    }
}
