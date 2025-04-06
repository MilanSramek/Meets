using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal sealed class IntegrationEventPublisher : IIntegrationEventPublisher
{
    private readonly IIntegrationEventCollectorAccessor _eventCollectorAccessor;

    public IntegrationEventPublisher(IIntegrationEventCollectorAccessor eventCollectorAccessor)
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
