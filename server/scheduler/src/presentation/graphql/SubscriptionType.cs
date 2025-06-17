using Meets.Scheduler.Activities;
using Meets.Scheduler.Polls;

namespace Meets.Scheduler;

internal sealed class SubscriptionType : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("activity")
            .Type<ActivityType>()
            .Argument("id", arg => arg.Type<NonNullType<UuidType>>())
            .Description("Subscribe to a activity by its identifier.")
            .Resolve(ctx => ctx.GetEventMessage<ActivityModel>())
            .Subscribe(ctx =>
            {
                var activityId = ctx.ArgumentValue<Guid>("id");
                var cancellationToken = ctx.RequestAborted;
                var watcherProvider = ctx.Service<ActivityWatcherProvider>();

                return watcherProvider(activityId, cancellationToken);
            });

        descriptor
            .Field("poll")
            .Type<PollType>()
            .Argument("id", arg => arg.Type<NonNullType<UuidType>>())
            .Description("Subscribe to a poll by its identifier.")
            .Resolve(ctx => ctx.GetEventMessage<PollModel>())
            .Subscribe(ctx =>
            {
                var pollId = ctx.ArgumentValue<Guid>("id");
                var cancellationToken = ctx.RequestAborted;
                var watcherProvider = ctx.Service<PollWatcherProvider>();

                return watcherProvider(pollId, cancellationToken);
            });
    }
}
