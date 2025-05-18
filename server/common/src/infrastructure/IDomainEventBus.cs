
namespace Meets.Common.Infrastructure;

internal interface IDomainEventBus
{
    public Task PublishAsync(object @event, CancellationToken cancellationToken);
}
