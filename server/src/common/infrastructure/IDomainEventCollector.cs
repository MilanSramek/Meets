namespace Meets.Common.Infrastructure;

public interface IDomainEventCollector
{
    public void AddEvent(object @event);
}
