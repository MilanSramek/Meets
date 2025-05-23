using System.Security.Principal;

namespace Meets.Common.Domain;

public abstract class AggregateRoot<TId> : Entity<TId>, IAggregateRoot<TId>
    where TId : notnull
{
    private HashSet<object>? _localEvents;

    private HashSet<object>? _distributedEvents;

    public IReadOnlyCollection<object> DomainEvents => _localEvents ?? [];

    public IReadOnlyCollection<object> IntegrationEvents => _distributedEvents ?? [];

    public void AddLocalEvent(object @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        _localEvents ??= [];
        _localEvents.Add(@event);
    }

    public void AddDistributedEvent(object @event)
    {
        ArgumentNullException.ThrowIfNull(@event);

        _distributedEvents ??= [];
        _distributedEvents.Add(@event);
    }
}
