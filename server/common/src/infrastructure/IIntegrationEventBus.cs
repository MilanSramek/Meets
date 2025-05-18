using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal interface IIntegrationEventBus
{
    public ValueTask PublishAsync(object @event, CancellationToken cancellationToken);
    public IDisposable Subscribe<TEvent>(IIntegrationEventHandler<TEvent> handler);
}
