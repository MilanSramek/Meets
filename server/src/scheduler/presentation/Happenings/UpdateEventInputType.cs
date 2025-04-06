namespace Meets.Scheduler.Happenings;

internal sealed class UpdateEventInputType : InputObjectType<UpdateHappeningInterInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateHappeningInterInput> descriptor)
    {
        descriptor
            .Name("UpdateEventInput");

        descriptor
            .Field(t => t.Name)
            .Type<StringType>();
        descriptor
            .Field(t => t.Description)
            .Type<StringType>();
    }
}
