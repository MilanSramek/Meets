namespace Meets.Common.Domain;

public interface IIntegrationEventPublisher
{
    public ValueTask PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
}
