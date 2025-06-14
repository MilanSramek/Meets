namespace Meets.Scheduler.Activities;

internal sealed class UpdateActivityInputType : InputObjectType<UpdateActivityInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateActivityInput> descriptor)
    {
        descriptor
            .Name("UpdateActivityInput");

        descriptor
            .Field(t => t.Name)
            .Type<StringType>();
        descriptor
            .Field(t => t.Description)
            .Type<StringType>();
    }
}
