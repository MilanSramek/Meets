namespace Meets.Common.Domain;

public interface IIntegrationEventHandler<in TEvent>
{
    public ValueTask HandleAsync(TEvent @event, CancellationToken cancellationToken);
}
