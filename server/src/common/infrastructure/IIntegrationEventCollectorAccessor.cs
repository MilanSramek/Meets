namespace Meets.Common.Infrastructure;

public interface IIntegrationEventCollectorAccessor
{
    public IIntegrationEventCollector? EventCollector { get; }
}
