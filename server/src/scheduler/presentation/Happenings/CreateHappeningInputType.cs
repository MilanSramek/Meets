namespace Meets.Scheduler.Happenings;

public sealed class CreateHappeningInputType : InputObjectType<CreateHappeningInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateHappeningInput> descriptor)
    {
        descriptor
            .Name("CreateEventInput");

        descriptor
            .Field(t => t.Name)
            .Type<NonNullType<StringType>>();
        descriptor
            .Field(t => t.Description)
            .Type<StringType>();
    }
}
