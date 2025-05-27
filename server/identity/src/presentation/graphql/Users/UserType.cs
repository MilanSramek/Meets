namespace Meets.Identity.Users;

internal sealed class UserType : ObjectType<UserModel>
{
    protected override void Configure(IObjectTypeDescriptor<UserModel> descriptor)
    {
        descriptor
            .Name("User");

        descriptor
            .Field(_ => _.Id)
            .Type<NonNullType<UuidType>>();
        descriptor
            .Field(_ => _.UserName)
            .Type<NonNullType<StringType>>();
    }
}
