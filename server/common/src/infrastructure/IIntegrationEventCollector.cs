namespace Meets.Common.Infrastructure;

public interface IIntegrationEventCollector
{
    public void AddEvent(object @event);
}
