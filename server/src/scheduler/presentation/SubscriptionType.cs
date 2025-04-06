using Meets.Scheduler.Happenings;
using Meets.Scheduler.Polls;

namespace Meets.Scheduler;

internal sealed class SubscriptionType : ObjectType
{
    protected override void Configure(IObjectTypeDescriptor descriptor)
    {
        descriptor
            .Field("happening")
            .Type<HappeningType>()
            .Argument("id", arg => arg.Type<NonNullType<UuidType>>())
            .Description("Subscribe to a happening by its identifier.")
            .Resolve(ctx => ctx.GetEventMessage<HappeningModel>())
            .Subscribe(ctx =>
            {
                var happeningId = ctx.ArgumentValue<Guid>("id");
                var cancellationToken = ctx.RequestAborted;
                var watcherProvider = ctx.Service<HappeningWatcherProvider>();

                return watcherProvider(happeningId, cancellationToken);
            });

        descriptor
            .Field("poll")
            .Type<PollType>()
            .Argument("id", arg => arg.Type<NonNullType<UuidType>>())
            .Description("Subscribe to a poll by its identifier.")
            .Resolve(ctx => ctx.GetEventMessage<PollType>())
            .Subscribe(ctx =>
            {
                var pollId = ctx.ArgumentValue<Guid>("id");
                var cancellationToken = ctx.RequestAborted;
                var watcherProvider = ctx.Service<PollWatcherProvider>();

                return watcherProvider(pollId, cancellationToken);
            });
    }
}
