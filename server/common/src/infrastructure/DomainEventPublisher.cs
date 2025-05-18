using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal sealed class DomainEventPublisher : IDomainEventPublisher
{
    private readonly IDomainEventCollectorAccessor _eventCollectorAccessor;

    public DomainEventPublisher(IDomainEventCollectorAccessor eventCollectorAccessor)
    {
        _eventCollectorAccessor = eventCollectorAccessor
            ?? throw new ArgumentNullException(nameof(eventCollectorAccessor));
    }

    public ValueTask PublishAsync<TEvent>(TEvent @event, CancellationToken _)
    {
        _eventCollectorAccessor.EventCollector?.AddEvent(@event!);
        return ValueTask.CompletedTask;
    }
}
