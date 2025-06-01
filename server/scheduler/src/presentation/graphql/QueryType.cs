using Meets.Scheduler.Activities;

namespace Meets.Scheduler;

internal class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field(_ => _.GetActivityAsync(default!, default!, default!))
            .Type<NonNullType<ActivityType>>()
            .Description("Activity with a specified ID.");

        descriptor
            .Field(_ => _.GetActivitiesAsync(default!, default!))
            .Type<NonNullType<ListType<NonNullType<ActivityType>>>>()
            .Description("All activities.")
            .Authorize();
    }
}
