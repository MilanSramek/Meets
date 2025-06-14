namespace Meets.Scheduler.Votes;

internal sealed class CreateUpdateVoteItemInputType : InputObjectType<CreateUpdateVoteItemModel>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateUpdateVoteItemModel> descriptor)
    {
        descriptor
            .Name("CreateUpdateVoteItemInput");

        descriptor
            .Field(_ => _.Date)
            .Type<NonNullType<IdType>>();
        descriptor
            .Field(_ => _.Statement)
            .Type<NonNullType<VoteItemStatementType>>();
    }
}
