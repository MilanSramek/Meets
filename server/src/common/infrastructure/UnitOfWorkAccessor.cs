using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal sealed class UnitOfWorkAccessor :
    IUnitOfWorkAccessor,
    IDomainEventCollectorAccessor,
    IIntegrationEventCollectorAccessor
{
    private static readonly AsyncLocal<IUnitOfWork?> s_current = new();

    public IUnitOfWork? Current { get => s_current.Value; set => s_current.Value = value; }

    IDomainEventCollector? IDomainEventCollectorAccessor.EventCollector => s_current
        .Value as IDomainEventCollector;

    IIntegrationEventCollector? IIntegrationEventCollectorAccessor.EventCollector => s_current
        .Value as IIntegrationEventCollector;
}
