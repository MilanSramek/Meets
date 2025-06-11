using HotChocolate.Resolvers;

using Meets.Common.Application.Identity;
using Meets.Identity.Users;

using Microsoft.AspNetCore.Authorization;

namespace Meets.Identity.Authorization;

internal sealed class TheUserHandler2 :
    AuthorizationHandler<TheUserRequirement, IResolverContext>
{
    private readonly IIdentityContext _identityContext;

    public TheUserHandler2(IIdentityContext identityContext)
    {
        _identityContext = identityContext;
    }

    protected override Task HandleRequirementAsync(
        AuthorizationHandlerContext context,
        TheUserRequirement requirement,
        IResolverContext resolverContext)
    {
        var user = resolverContext.Parent<UserModel>();
        if (_identityContext.UserId == user.Id)
        {
            context.Succeed(requirement);
            return Task.CompletedTask;
        }

        context.Fail();
        return Task.CompletedTask;
    }
}