using Meets.Common.Domain;

using Microsoft.Extensions.DependencyInjection;

using System.Collections.Concurrent;
using System.Reflection;

namespace Meets.Common.Infrastructure;

internal sealed class DomainEventBus : IDomainEventBus
{
    private static readonly MethodInfo s_publishInnerMethod = typeof(DomainEventBus)
        .GetMethod(nameof(PublishInnerAsync), BindingFlags.Instance | BindingFlags.NonPublic)!;

    private static readonly ConcurrentDictionary<Type, MethodInfo> s_eventHandlerMethods = new();

    private readonly IServiceProvider _serviceProvider;

    public DomainEventBus(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider
            ?? throw new ArgumentNullException(nameof(serviceProvider));
    }

    public Task PublishAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        return PublishInnerAsync(@event, cancellationToken);
    }

    public async Task PublishAsync(object @event, CancellationToken cancellationToken)
    {
        var handleMethod = s_eventHandlerMethods.GetOrAdd(@event.GetType(),
            eventType => s_publishInnerMethod.MakeGenericMethod(eventType));

        await (Task)handleMethod.Invoke(this, [@event, cancellationToken])!;
    }

    private async Task PublishInnerAsync<TEvent>(TEvent @event, CancellationToken cancellationToken)
    {
        var eventHandlers = _serviceProvider.GetServices<IDomainEventHandler<TEvent>>();
        foreach (var eventHandler in eventHandlers)
        {
            await eventHandler.HandleAsync(@event, cancellationToken);
        }
    }
}
