using Meets.Common.Domain;

namespace Meets.Common.Infrastructure;

internal sealed class UnitOfWork : IUnitOfWork, IDomainEventCollector, IIntegrationEventCollector, IAsyncDisposable
{
    private readonly IDomainEventBus _domainEventBus;
    private readonly IIntegrationEventBus _integrationEventBus;
    private readonly UnitOfWorkDisposer? _disposable;

    private List<object>? _domainEvents;
    private List<object>? _distributedEvents;

    public UnitOfWork(
        IDomainEventBus domainEventBus,
        IIntegrationEventBus integrationEventBus,
        UnitOfWorkDisposer? disposable = default)
    {
        _domainEventBus = domainEventBus
            ?? throw new ArgumentNullException(nameof(domainEventBus));
        _integrationEventBus = integrationEventBus
            ?? throw new ArgumentNullException(nameof(integrationEventBus));
        _disposable = disposable;
    }

    public async ValueTask CompleteAsync(CancellationToken cancellationToken)
    {
        await PublishDomainEventsAsync(cancellationToken);
        // ToDo: Commit possible transaction
        await PublishIntegrationEvents(cancellationToken);
    }

    public ValueTask DisposeAsync()
    {
        _disposable?.Dispose();
        return ValueTask.CompletedTask;
    }

    public ValueTask RollbackAsync(CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    public ValueTask SaveChangesAsync(CancellationToken cancellationToken)
    {
        return ValueTask.CompletedTask;
    }

    void IDomainEventCollector.AddEvent(object @event)
    {
        _domainEvents ??= [];
        _domainEvents.Add(@event);
    }

    void IIntegrationEventCollector.AddEvent(object @event)
    {
        _distributedEvents ??= [];
        _distributedEvents.Add(@event);
    }

    private async Task PublishDomainEventsAsync(CancellationToken cancellationToken)
    {
        if (_domainEvents is null)
        {
            return;
        }

        foreach (object @event in _domainEvents)
        {
            await _domainEventBus.PublishAsync(@event, cancellationToken);
        }
    }

    private async Task PublishIntegrationEvents(CancellationToken cancellationToken)
    {
        if (_distributedEvents is null)
        {
            return;
        }

        foreach (object @event in _distributedEvents)
        {
            await _integrationEventBus.PublishAsync(@event, cancellationToken);
        }
    }
}
