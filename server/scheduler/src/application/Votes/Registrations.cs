using Meets.Common.Domain;
using Meets.Common.Tools.Observables;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Votes;

internal static class Registrations
{
    public static IServiceCollection AddVote(this IServiceCollection services) => services
        .AddScoped<IVoteCreationService, VoteService>()
        .AddScoped<IVoteUpdateService, VoteService>()
        .AddVoteChangeEventHandler();

    private static IServiceCollection AddVoteChangeEventHandler(this IServiceCollection services) => services
        .AddSingleton(provider =>
        {
            var handler = new VoteEventHandler();

            provider.GetRequiredService<IIntegrationEventSubscribePoint>()
                .Subscribe<VoteCreatedEvent>(handler);
            provider.GetRequiredService<IIntegrationEventSubscribePoint>()
                .Subscribe<VoteChangedEvent>(handler);
            provider.GetRequiredService<IIntegrationEventSubscribePoint>()
                .Subscribe<VoteDeletedEvent>(handler);

            return handler;
        })
        .AddSingleton<IIntegrationEventHandler<VoteCreatedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>())
        .AddSingleton<IIntegrationEventHandler<VoteChangedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>())
        .AddSingleton<IIntegrationEventHandler<VoteDeletedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>())
        .AddSingleton<IAsyncObservable<VoteCreatedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>())
        .AddSingleton<IAsyncObservable<VoteChangedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>())
        .AddSingleton<IAsyncObservable<VoteDeletedEvent>>(provider => provider
            .GetRequiredService<VoteEventHandler>());
}
