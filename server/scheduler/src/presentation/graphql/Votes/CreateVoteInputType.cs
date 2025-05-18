namespace Meets.Scheduler.Votes;

internal sealed class CreateVoteInputType : InputObjectType<CreateVoteInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateVoteInput> createVoteInput)
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
