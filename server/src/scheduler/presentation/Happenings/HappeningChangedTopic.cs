using Meets.Common.Domain;
using Meets.Common.Presentation.Graphql.Subscriptions;
using Meets.Common.Tools.Disposables;
using Meets.Common.Tools.Observables;
using Meets.Scheduler.Polls;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Happenings;

internal static class HappeningChangedTopic
{
    public static IServiceCollection AddHappeningWatch(this IServiceCollection services)
    {
        return services
            .AddHappeningTopicProducer()
            .AddHappeningTopicConsumer();
    }

    private static IServiceCollection AddHappeningTopicProducer(this IServiceCollection services)
    {
        services
            .AddObserverTopicSender<HappeningChangedEvent, HappeningChangedTopicEvent>(
                @event => new(@event.Id, @event.Version),
                (_, @event) => HappeningChangedTopicEvent.GetTopicName(@event.Id));

        return services
            .AddObserverTopicSender<VoteCreatedEvent, HappeningChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new HappeningChangedTopicEvent(poll.HappeningId, default);
                    };
                },
                (_, @event) => HappeningChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteChangedEvent, HappeningChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new HappeningChangedTopicEvent(poll.HappeningId, default);
                    };
                },
                (_, @event) => HappeningChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteDeletedEvent, HappeningChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new HappeningChangedTopicEvent(poll.HappeningId, default);
                    };
                },
                (_, @event) => HappeningChangedTopicEvent.GetTopicName(@event.Id));
    }

    private static IServiceCollection AddHappeningTopicConsumer(this IServiceCollection services)
    {
        return services
            .AddDataWatcherProvider<HappeningModel, Guid, HappeningChangedTopicEvent>(
                HappeningChangedTopicEvent.GetTopicName,
                serviceProvider => async (happeningId, happeningEvent, cancellationToken) =>
                {
                    var happenings = serviceProvider.GetRequiredService<IReadOnlyRepository<Happening, Guid>>();
                    var happening = await happenings.GetAsync(happeningId, cancellationToken);
                    return happening.MapToModel();
                })
            .AddTransient<HappeningWatcherProvider>(provider => (happeningId, cancellationToken) =>
            {
                var watcherProvider = provider
                    .GetRequiredService<IDataWatcherProvider<HappeningModel, Guid>>();
                return watcherProvider.GetWatcherAsync(happeningId, cancellationToken);
            });
    }

    public static IAsyncDisposable InitializeHappeningWatch(this IServiceProvider provider)
    {
        var happeningObservable = provider
            .GetRequiredService<IAsyncObservable<HappeningChangedEvent>>();
        var happeningObserver = provider
            .GetRequiredService<IObserverTopicSender<HappeningChangedEvent, HappeningChangedTopicEvent>>();

        var voteCreateObservable = provider
            .GetRequiredService<IAsyncObservable<VoteCreatedEvent>>();
        var voteCreateObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteCreatedEvent, HappeningChangedTopicEvent>>();

        var voteChangeObservable = provider
             .GetRequiredService<IAsyncObservable<VoteChangedEvent>>();
        var voteChangeObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteChangedEvent, HappeningChangedTopicEvent>>();

        var voteDeleteObservable = provider
             .GetRequiredService<IAsyncObservable<VoteDeletedEvent>>();
        var voteDeleteObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteDeletedEvent, HappeningChangedTopicEvent>>();

        return Disposable.Combine(
            happeningObservable.SubscribeAsync(happeningObserver),
            voteCreateObservable.SubscribeAsync(voteCreateObserver),
            voteChangeObservable.SubscribeAsync(voteChangeObserver),
            voteDeleteObservable.SubscribeAsync(voteDeleteObserver));
    }
}
