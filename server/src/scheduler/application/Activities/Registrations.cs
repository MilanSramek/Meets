using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Activities;

internal static class Registrations
{
    public static IServiceCollection AddActivity(this IServiceCollection services) => services
        .AddScoped<IActivityCreationService, ActivityCreationService>()
        .AddScoped<IActivityUpdateService, ActivityUpdateService>()
        .AddActivityChangeEventHandler2();

    private static IServiceCollection AddActivityChangeEventHandler2(this IServiceCollection services) => services
        .AddSingleton(provider =>
        {
            var handler = new ActivityChangeEventHandler2();
            provider.GetRequiredService<IIntegrationEventSubscribePoint>().Subscribe(handler);
            return handler;
        })
        .AddSingleton<IIntegrationEventHandler<ActivityChangedEvent>>(provider => provider
            .GetRequiredService<ActivityChangeEventHandler2>())
        .AddSingleton<IAsyncObservable<ActivityChangedEvent>>(provider => provider
            .GetRequiredService<ActivityChangeEventHandler2>());

    private static IServiceCollection AddActivityChangeEventHandler(this IServiceCollection services) => services
        .AddSingleton(provider =>
        {
            var activities = provider.GetRequiredService<IReadOnlyRepository<Activity, Guid>>();
            var handler = new ActivityChangeEventHandler(activities);
            provider.GetRequiredService<IIntegrationEventSubscribePoint>().Subscribe(handler);
            return handler;
        })
        .AddSingleton<IIntegrationEventHandler<ActivityChangedEvent>>(provider => provider
            .GetRequiredService<ActivityChangeEventHandler>())
        .AddSingleton<IActivityWatcherProvider>(provider => provider
            .GetRequiredService<ActivityChangeEventHandler>());
}
