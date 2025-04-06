using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Happenings;

internal static class Registrations
{
    public static IServiceCollection AddHappening(this IServiceCollection services) => services
        .AddScoped<IHappeningCreationService, HappeningCreationService>()
        .AddScoped<IHappeningUpdateService, HappeningUpdateService>()
        .AddHappeningChangeEventHandler2();

    private static IServiceCollection AddHappeningChangeEventHandler2(this IServiceCollection services) => services
        .AddSingleton(provider =>
        {
            var handler = new HappeningChangeEventHandler2();
            provider.GetRequiredService<IIntegrationEventSubscribePoint>().Subscribe(handler);
            return handler;
        })
        .AddSingleton<IIntegrationEventHandler<HappeningChangedEvent>>(provider => provider
            .GetRequiredService<HappeningChangeEventHandler2>())
        .AddSingleton<IAsyncObservable<HappeningChangedEvent>>(provider => provider
            .GetRequiredService<HappeningChangeEventHandler2>());

    private static IServiceCollection AddHappeningChangeEventHandler(this IServiceCollection services) => services
        .AddSingleton(provider =>
        {
            var events = provider.GetRequiredService<IReadOnlyRepository<Happening, Guid>>();
            var handler = new HappeningChangeEventHandler(events);
            provider.GetRequiredService<IIntegrationEventSubscribePoint>().Subscribe(handler);
            return handler;
        })
        .AddSingleton<IIntegrationEventHandler<HappeningChangedEvent>>(provider => provider
            .GetRequiredService<HappeningChangeEventHandler>())
        .AddSingleton<IHappeningWatcherProvider>(provider => provider
            .GetRequiredService<HappeningChangeEventHandler>());
}
