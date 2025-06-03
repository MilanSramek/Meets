using Meets.Scheduler.Polls;

namespace Meets.Scheduler.Activities;


internal sealed class ActivityType : ObjectType<ActivityModel>
{
    protected override void Configure(IObjectTypeDescriptor<ActivityModel> descriptor)
    {
        descriptor
            .Name("Activity");

        descriptor
            .Field(_ => _.Id)
            .Type<NonNullType<UuidType>>();
        descriptor
            .Field(_ => _.Name)
            .Type<NonNullType<StringType>>();
        descriptor
            .Field(_ => _.Description)
            .Type<StringType>();
        descriptor
            .Field(_ => _.OwnerId)
            .Type<NonNullType<UuidType>>();
        descriptor
            .Field<ActivityResolver>(_ => _.GetPollAsync(default!, default!, default))
            .Type<NonNullType<PollType>>();
    }
}
