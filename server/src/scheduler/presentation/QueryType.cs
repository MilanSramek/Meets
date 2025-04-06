using Meets.Scheduler.Happenings;

namespace Meets.Scheduler;

internal class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field(_ => _.GetHappeningAsync(default!, default!, default!))
            .Type<NonNullType<HappeningType>>()
            .Description("Happening with a specified ID.");

        descriptor
            .Field(_ => _.GetHappeningsAsync(default!, default!))
            .Type<NonNullType<ListType<NonNullType<HappeningType>>>>()
            .Description("All happenings.");
    }
}
