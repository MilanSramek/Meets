using Meets.Scheduler.Activities;
using Meets.Scheduler.Votes;

namespace Meets.Scheduler;

internal class MutationType : ObjectType<Mutation>
{
    protected override void Configure(IObjectTypeDescriptor<Mutation> descriptor)
    {
        descriptor
            .Field(_ => _.CreateActivityAsync(default!, default!, default!, default!))
            .Name("createActivity")
            .Type<NonNullType<ActivityType>>()
            .Argument("request", _ => _.Type<NonNullType<CreateActivityRequestType>>())
            .Description("Creates a new activity.");

        descriptor
            .Field(_ => _.UpdateActivityAsync(default, default!, default!, default!))
            .Name("updateActivity")
            .Type<NonNullType<ActivityType>>()
            .Description("Updates an activity with a specified ID.");

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
