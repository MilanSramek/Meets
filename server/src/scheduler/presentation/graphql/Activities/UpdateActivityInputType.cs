namespace Meets.Scheduler.Activities;

internal sealed class UpdateActivityInputType : InputObjectType<UpdateActivityInterInput>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateActivityInterInput> descriptor)
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
