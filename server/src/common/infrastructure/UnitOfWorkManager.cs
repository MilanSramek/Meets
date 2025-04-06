using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal sealed class UnitOfWorkManager : IUnitOfWorkManager
{
    private readonly IUnitOfWorkAccessor _accessor;
    private readonly IDomainEventBus _domainEventBus;
    private readonly IIntegrationEventBus _integrationEventBus;

    public UnitOfWorkManager(
        IUnitOfWorkAccessor accessor,
        IDomainEventBus domainEventBus,
        IIntegrationEventBus integrationEventBus)
    {
        _accessor = accessor
            ?? throw new ArgumentNullException(nameof(accessor));
        _domainEventBus = domainEventBus
            ?? throw new ArgumentNullException(nameof(domainEventBus));
        _integrationEventBus = integrationEventBus
            ?? throw new ArgumentNullException(nameof(integrationEventBus));
    }

    public IUnitOfWork? Current => _accessor.Current;

    public ValueTask<IUnitOfWork> BeginAsync()
    {
        var unitOfWork = _accessor is { Current: IUnitOfWork ancestor }
            ? new UnitOfWork(_domainEventBus, _integrationEventBus, new(_accessor, ancestor))
            : new UnitOfWork(_domainEventBus, _integrationEventBus);

        _accessor.Current = unitOfWork;
        return new(unitOfWork);
    }
}
