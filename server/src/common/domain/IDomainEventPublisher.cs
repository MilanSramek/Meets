namespace Meets.Common.Domain;

public interface IDomainEventPublisher
{
    public ValueTask PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken);
}
