namespace Meets.Scheduler.Activities;

internal sealed class UpdateActivityRequestType : InputObjectType<UpdateActivityRequest>
{
    protected override void Configure(IInputObjectTypeDescriptor<UpdateActivityRequest> descriptor)
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
