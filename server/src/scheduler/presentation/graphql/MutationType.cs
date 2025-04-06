using Meets.Scheduler.Happenings;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler;

internal class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field(_ => _.CreateHappeningAsync(default!, default!, default!))
            .Name("createHappening")
            .Type<NonNullType<HappeningType>>()
            .Argument("input", _ => _.Type<NonNullType<CreateHappeningInputType>>())
            .Description("Creates a new happening.");

        descriptor
            .Field(_ => _.UpdateHappeningAsync(default, default!, default!, default!))
            .Name("updateHappening")
            .Type<NonNullType<HappeningType>>()
            .Description("Updates an happening with a specified ID.");

        descriptor
            .Field(_ => _.AddVoteAsync(default!, default!, default!))
            .Name("addVote")
            .Type<NonNullType<VoteType>>()
            .Argument("input", _ => _.Type<NonNullType<CreateVoteInputType>>());

        descriptor
            .Field(_ => _.UpdateVoteAsync(default!, default!, default!, default!))
            .Name("updateVote")
            .Type<NonNullType<VoteType>>()
            .Argument("items", _ => _.Type<NonNullType<ListType<NonNullType<CreateUpdateVoteItemInputType>>>>());
    }
}
