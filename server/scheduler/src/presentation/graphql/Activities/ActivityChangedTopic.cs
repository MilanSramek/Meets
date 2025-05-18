using Meets.Common.Domain;
using Meets.Common.Presentation.GraphQL.Subscriptions;
using Meets.Common.Tools.Disposables;
using Meets.Common.Tools.Observables;
using Meets.Scheduler.Polls;
using Meets.Scheduler.Votes;

using Microsoft.Extensions.DependencyInjection;

namespace Meets.Scheduler.Activities;

internal static class ActivityChangedTopic
{
    public static IServiceCollection AddActivityWatch(this IServiceCollection services)
    {
        return services
            .AddActivityTopicProducer()
            .AddActivityTopicConsumer();
    }

    private static IServiceCollection AddActivityTopicProducer(this IServiceCollection services)
    {
        services
            .AddObserverTopicSender<ActivityChangedEvent, ActivityChangedTopicEvent>(
                @event => new(@event.Id, @event.Version),
                (_, @event) => ActivityChangedTopicEvent.GetTopicName(@event.Id));

        return services
            .AddObserverTopicSender<VoteCreatedEvent, ActivityChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new ActivityChangedTopicEvent(poll.ActivityId, default);
                    };
                },
                (_, @event) => ActivityChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteChangedEvent, ActivityChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new ActivityChangedTopicEvent(poll.ActivityId, default);
                    };
                },
                (_, @event) => ActivityChangedTopicEvent.GetTopicName(@event.Id))
            .AddObserverTopicSender<VoteDeletedEvent, ActivityChangedTopicEvent>(
                provider =>
                {
                    var votes = provider.GetRequiredService<IReadOnlyRepository<Vote, Guid>>();
                    var polls = provider.GetRequiredService<IReadOnlyRepository<Poll, Guid>>();
                    return async (@event, cancellationToken) =>
                    {
                        var vote = await votes.GetAsync(@event.Id, cancellationToken);
                        var poll = await polls.GetAsync(vote.PollId, cancellationToken);
                        return new ActivityChangedTopicEvent(poll.ActivityId, default);
                    };
                },
                (_, @event) => ActivityChangedTopicEvent.GetTopicName(@event.Id));
    }

    private static IServiceCollection AddActivityTopicConsumer(this IServiceCollection services)
    {
        return services
            .AddDataWatcherProvider<ActivityModel, Guid, ActivityChangedTopicEvent>(
                ActivityChangedTopicEvent.GetTopicName,
                serviceProvider => async (activityId, ActivityEvent, cancellationToken) =>
                {
                    var activities = serviceProvider.GetRequiredService<IReadOnlyRepository<Activity, Guid>>();
                    var activity = await activities.GetAsync(activityId, cancellationToken);
                    return activity.MapToModel();
                })
            .AddTransient<ActivityWatcherProvider>(provider => (activityId, cancellationToken) =>
            {
                var watcherProvider = provider
                    .GetRequiredService<IDataWatcherProvider<ActivityModel, Guid>>();
                return watcherProvider.GetWatcherAsync(activityId, cancellationToken);
            });
    }

    public static IAsyncDisposable InitializeActivityWatch(this IServiceProvider provider)
    {
        var activityObservable = provider
            .GetRequiredService<IAsyncObservable<ActivityChangedEvent>>();
        var activityObserver = provider
            .GetRequiredService<IObserverTopicSender<ActivityChangedEvent, ActivityChangedTopicEvent>>();

        var voteCreateObservable = provider
            .GetRequiredService<IAsyncObservable<VoteCreatedEvent>>();
        var voteCreateObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteCreatedEvent, ActivityChangedTopicEvent>>();

        var voteChangeObservable = provider
             .GetRequiredService<IAsyncObservable<VoteChangedEvent>>();
        var voteChangeObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteChangedEvent, ActivityChangedTopicEvent>>();

        var voteDeleteObservable = provider
             .GetRequiredService<IAsyncObservable<VoteDeletedEvent>>();
        var voteDeleteObserver = provider
            .GetRequiredService<IObserverTopicSender<VoteDeletedEvent, ActivityChangedTopicEvent>>();

        return Disposable.Combine(
            activityObservable.SubscribeAsync(activityObserver),
            voteCreateObservable.SubscribeAsync(voteCreateObserver),
            voteChangeObservable.SubscribeAsync(voteChangeObserver),
            voteDeleteObservable.SubscribeAsync(voteDeleteObserver));
    }
}
