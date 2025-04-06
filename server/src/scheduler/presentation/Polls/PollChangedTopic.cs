using Meets.Common.Domain;
using Meets.Common.Presentation.Graphql.Subscriptions;
using Meets.Common.Tools.Disposables;
using Meets.Common.Tools.Observables;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Polls;

internal static class PollChangedTopic
{
    public static IServiceCollection AddPollWatch(this IServiceCollection services)
    {
        return services
            .AddVotePollTopicProducer()
            .AddPollTopicConsumer();
    }

    private static IServiceCollection AddVotePollTopicProducer(this IServiceCollection services)
    {
        return services
            .AddObserverTopicSender<VoteCreatedEvent, PollChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        return new PollChangedTopicEvent(vote.PollId, default);
                    };
                },
                (_, @event) => PollChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteChangedEvent, PollChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        return new PollChangedTopicEvent(vote.PollId, default);
                    };
                },
                (_, @event) => PollChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteDeletedEvent, PollChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        return new PollChangedTopicEvent(vote.PollId, default);
                    };
                },
                (_, @event) => PollChangedTopicEvent.GetTopicName(@event.Id));
    }

    private static IServiceCollection AddPollTopicConsumer(this IServiceCollection services)
    {
        return services
            .AddDataWatcherProvider<PollModel, Guid, PollChangedTopicEvent>(
                PollChangedTopicEvent.GetTopicName,
                serviceProvider => async (pollId, pollEvent, cancellationToken) =>
                {
                    var polls = serviceProvider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    var poll = await polls.GetAsync(pollId, cancellationToken);
                    return poll.MapToModel();
                })
            .AddTransient<PollWatcherProvider>(provider => (pollId, cancellationToken) =>
            {
                var watcherProvider = provider
                    .GetRequiredService<IDataWatcherProvider<PollModel, Guid>>();
                return watcherProvider.GetWatcherAsync(pollId, cancellationToken);
            });
    }

    public static IAsyncDisposable InitializePollWatch(this IServiceProvider provider)
    {
        var voteCreateObservable = provider
            .GetRequiredService<IAsyncObservable<VoteCreatedEvent>>();
        var voteCreateObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteCreatedEvent, PollChangedTopicEvent>>();

        var voteChangeObservable = provider
             .GetRequiredService<IAsyncObservable<VoteChangedEvent>>();
        var voteChangeObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteChangedEvent, PollChangedTopicEvent>>();

        var voteDeleteObservable = provider
             .GetRequiredService<IAsyncObservable<VoteDeletedEvent>>();
        var voteDeleteObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteDeletedEvent, PollChangedTopicEvent>>();

        return Disposable.Combine(
            voteCreateObservable.SubscribeAsync(voteCreateObserver),
            voteChangeObservable.SubscribeAsync(voteChangeObserver),
            voteDeleteObservable.SubscribeAsync(voteDeleteObserver));
    }
}
