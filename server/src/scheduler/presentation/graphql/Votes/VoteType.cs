namespace Meets.Scheduler.Votes;

internal sealed class VoteType : ObjectType<VoteModel>
{
    protected override void Configure(IObjectTypeDescriptor<VoteModel> vote)
    {
        vote
            .Name("Vote");

        vote
            .Field(_ => _.Id)
            .Type<NonNullType<UuidType>>();
        vote
            .Field(_ => _.Items)
            .Type<NonNullType<ListType<NonNullType<VoteItemType>>>>();
    }
}
