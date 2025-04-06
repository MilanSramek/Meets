namespace Meets.Scheduler.Votes;

internal sealed class VoteItemType : ObjectType<VoteItemModel>
{
    protected override void Configure(IObjectTypeDescriptor<VoteItemModel> voteItem)
    {
        voteItem
            .Name("VoteItem");

        voteItem
            .Field(_ => _.Date)
            .Type<NonNullType<DateType>>();
        voteItem
            .Field(_ => _.Statement)
            .Type<NonNullType<VoteItemStatementType>>();
    }
}
