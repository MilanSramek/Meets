using Meets.Scheduler.Votes;

namespace Meets.Scheduler.Polls;

internal sealed class PollType : ObjectType<PollModel>
{
    protected override void Configure(IObjectTypeDescriptor<PollModel> descriptor)
    {
        descriptor
            .Name("Poll");

        descriptor
            .Field(_ => _.Id)
            .Type<NonNullType<UuidType>>();
        descriptor
            .Field<PollResolver>(_ => _.GetVotesAsync(default!, default!, default))
            .Type<NonNullType<ListType<NonNullType<VoteType>>>>();
    }
}
