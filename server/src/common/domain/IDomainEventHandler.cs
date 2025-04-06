namespace Meets.Common.Domain;

public interface IDomainEventHandler<TEvent>
{
    public ValueTask HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
