namespace Meets.Common.Infrastructure;

public static class EventCollectorExtensions
{
    public static void AddEvents(this IDomainEventCollector eventCollector,
        IEnumerable<object> events)
    {
        ArgumentNullException.ThrowIfNull(eventCollector);
        ArgumentNullException.ThrowIfNull(events);

        foreach (object @event in events)
        {
            eventCollector.AddEvent(@event);
        }
    }

    public static void AddEvents(this IIntegrationEventCollector eventCollector,
        IEnumerable<object> events)
    {
        ArgumentNullException.ThrowIfNull(eventCollector);
        ArgumentNullException.ThrowIfNull(events);

        foreach (object @event in events)
        {
            eventCollector.AddEvent(@event);
        }
    }
}
