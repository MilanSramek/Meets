namespace Meets.Scheduler.Activities;

public sealed class CreateActivityInputType : InputObjectType<CreateActivityInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<CreateActivityInput> descriptor)
    {
        descriptor
            .Name("CreateActivityInput");

        descriptor
            .Field(t => t.Name)
            .Type<NonNullType<StringType>>();
        descriptor
            .Field(t => t.Description)
            .Type<StringType>();
    }
}
