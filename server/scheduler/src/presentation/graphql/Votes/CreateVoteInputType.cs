namespace Meets.Scheduler.Votes;

internal sealed class CreateVoteInputType : InputObjectType<CreateVoteModel>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateVoteModel> createVoteInput)
    {
        createVoteInput
            .Name("CreateVoteInput");

        createVoteInput
            .Field(_ => _.PollId)
            .Type<NonNullType<UuidType>>();
        createVoteInput
            .Field(_ => _.Items)
            .Type<NonNullType<ListType<NonNullType<CreateUpdateVoteItemInputType>>>>();
    }
}
