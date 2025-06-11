using Meets.Common.Domain;
using Meets.Common.Infrastructure.Identity;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Common.Infrastructure;

public static class Registrations
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services) => services
        .AddUnitOfWork()
        .AddIntegrationEventBus()
        .AddDomainEventBus()
        .AddSingleton<IDomainEventPublisher, DomainEventPublisher>()
        .AddSingleton<IIntegrationEventPublisher, IntegrationEventPublisher>()
        .AddIdentityContext();

    private static IServiceCollection AddUnitOfWork(this IServiceCollection services) => services
        .AddTransient<IUnitOfWorkManager, UnitOfWorkManager>()
        .AddSingleton<UnitOfWorkAccessor>()
        .AddSingleton<IUnitOfWorkAccessor>(provider => provider.GetRequiredService<UnitOfWorkAccessor>())
        .AddSingleton<IDomainEventCollectorAccessor>(provider => provider.GetRequiredService<UnitOfWorkAccessor>())
        .AddSingleton<IIntegrationEventCollectorAccessor>(provider => provider.GetRequiredService<UnitOfWorkAccessor>());

    private static IServiceCollection AddIntegrationEventBus(this IServiceCollection services) => services
        .AddSingleton<IntegrationEventBus>()
        .AddSingleton<IIntegrationEventBus>(provider => provider.GetRequiredService<IntegrationEventBus>())
        .AddSingleton<IIntegrationEventSubscribePoint>(provider => provider.GetRequiredService<IntegrationEventBus>());

    private static IServiceCollection AddDomainEventBus(this IServiceCollection services) => services
        .AddSingleton<IDomainEventBus, DomainEventBus>();
}
