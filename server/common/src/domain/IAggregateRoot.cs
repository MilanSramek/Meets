namespace Meets.Common.Domain;

public interface IAggregateRoot<TId> : IEntity<TId> where TId : notnull
{
    public IReadOnlyCollection<object> DomainEvents { get; }
    public IReadOnlyCollection<object> IntegrationEvents { get; }

    public void AddLocalEvent(object @event);
    public void AddDistributedEvent(object @event);
}