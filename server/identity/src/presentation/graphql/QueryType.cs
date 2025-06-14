using Meets.Identity.Users;

namespace Meets.Identity;

internal class QueryType : ObjectType<Query>
{
    protected override void Configure(IObjectTypeDescriptor<Query> descriptor)
    {
        descriptor
            .Field(_ => _.GetUserAsync(default!, default!, default!))
            .Type<NonNullType<UserType>>()
            .Description("User with a specified ID.");
    }
}
