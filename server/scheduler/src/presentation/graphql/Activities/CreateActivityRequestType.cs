namespace Meets.Scheduler.Activities;

internal sealed class CreateActivityRequestType
    : InputObjectType<CreateActivityRequest>
{
    protected override void Configure(
        IInputObjectTypeDescriptor<CreateActivityRequest> descriptor)
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
