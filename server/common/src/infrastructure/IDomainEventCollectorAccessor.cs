namespace Meets.Common.Infrastructure;

public interface IDomainEventCollectorAccessor
{
    public IDomainEventCollector? EventCollector { get; }
}
